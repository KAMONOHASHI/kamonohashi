# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import click
from kamonohashi.op import rest

from kamonohashi.cli import configuration
from kamonohashi.cli import object_storage
from kamonohashi.cli import pprint
from kamonohashi.cli import util


@click.group()
def data():
    """Create and manage KAMONOHASHI data"""


@data.command('list')
@click.option('--count', type=click.IntRange(1, 10000), default=1000, show_default=True,
              help='Maximum number of data to list')
@click.option('--id', help='id')
@click.option('--name', help='name')
@click.option('--memo', help='memo')
@click.option('--created-at', help='created at')
@click.option('--created-by', help='created by')
@click.option('--tag', multiple=True, help='tag  [multiple]')
def list_data(count, id, name, memo, created_at, created_by, tag):
    """List data filtered by conditions"""
    api = rest.DataApi(configuration.get_api_client())
    per_page = 1000
    command_args = {
        'id': id,
        'name': name,
        'memo': memo,
        'created_at': created_at,
        'created_by': created_by,
        'tag': tag,
    }
    args = {key: value for key, value in command_args.items() if value is not None}
    if count <= per_page:
        result = api.list_data(per_page=count, **args)
    else:
        total_pages = (count - 1) // per_page + 1
        result = []
        for page in range(1, total_pages + 1):
            page_result = api.list_data(page=page, **args)
            result.extend(page_result)
            if len(page_result) < per_page:
                break

    pprint.pp_table(['id', 'name', 'created_at', 'created_by', 'memo', 'tags'],
                    [[x.id, x.name, x.created_at, x.created_by, x.memo, x.tags] for x in result[:count]])


@data.command()
@click.argument('id', type=int)
def get(id):
    """Get details of data"""
    api = rest.DataApi(configuration.get_api_client())
    result = api.get_data(id)
    pprint.pp_dict(util.to_dict(result))


@data.command()
@click.option('-n', '--name', required=True, help='A name of the data')
@click.option('-f', '--file', type=click.Path(exists=True, dir_okay=False), required=True, multiple=True,
              help='A file path you want to upload  [multiple]')
@click.option('-m', '--memo', help='Free text that can helpful to explain the data')
@click.option('-t', '--tags', multiple=True, help='Attributes to the data  [multiple]')
def create(name, file, memo, tags):
    """Create new data"""
    api = rest.DataApi(configuration.get_api_client())
    model = rest.DataApiModelsCreateInputModel(
        memo=memo,
        name=name,
        tags=list(tags)
    )
    result = api.create_data(model=model)
    for x in file:
        upload_info = object_storage.upload_file(api.api_client, x, 'Data')
        model = rest.ComponentsAddFileInputModel(file_name=upload_info.file_name,
                                                 stored_path=upload_info.stored_path)
        api.add_data_file(result.id, model=model)
    print('created', result.id)


@data.command()
@click.argument('id', type=int)
@click.option('-n', '--name', help='A name of the data')
@click.option('-m', '--memo', help='Free text that can helpful to explain the data')
@click.option('-t', '--tags', multiple=True, help='Attributes to the data  [multiple]')
def update(id, name, memo, tags):
    """Update data"""
    api = rest.DataApi(configuration.get_api_client())
    model = rest.DataApiModelsEditInputModel(name=name, memo=memo, tags=list(tags))
    result = api.update_data(id, model=model)
    print('updated', result.id)


@data.command()
@click.argument('id', type=int)
def delete(id):
    """Delete data"""
    api = rest.DataApi(configuration.get_api_client())
    api.delete_data(id)
    print('deleted', id)


@data.command('list-files')
@click.argument('id', type=int)
def list_files(id):
    """List files of data"""
    api = rest.DataApi(configuration.get_api_client())
    result = api.list_data_files(id)
    pprint.pp_table(['file_id', 'file_name'],
                    [[x.file_id, x.file_name] for x in result])


@data.command('download-files')
@click.argument('id', type=int)
@click.option('-d', '--destination', type=click.Path(exists=True, file_okay=False), required=True,
              help='A path to the output files')
def download_files(id, destination):
    """Download files of data"""
    api = rest.DataApi(configuration.get_api_client())
    result = api.list_data_files(id, with_url=True)
    pool_manager = api.api_client.rest_client.pool_manager
    for x in result:
        object_storage.download_file(pool_manager, x.url, destination, x.file_name)


@data.command('upload-files')
@click.argument('id', type=int)
@click.option('-f', '--file', type=click.Path(exists=True, dir_okay=False), required=True, multiple=True,
              help='A file path you want to upload  [multiple]')
def upload_files(id, file):
    """Upload files to data"""
    api = rest.DataApi(configuration.get_api_client())
    for x in file:
        upload_info = object_storage.upload_file(api.api_client, x, 'Data')
        model = rest.ComponentsAddFileInputModel(file_name=upload_info.file_name, stored_path=upload_info.stored_path)
        api.add_data_file(id, model=model)
