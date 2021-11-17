# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import logging

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
@click.option('-d', '--destination', type=click.Path(dir_okay=False), help='A file path of the output as a json file')
def get(id, destination):
    """Get details of data"""
    api = rest.DataApi(configuration.get_api_client())
    if destination is None:
        result = api.get_data(id)
        pprint.pp_dict(util.to_dict(result))
    else:
        with util.release_conn(api.get_data(id, _preload_content=False)) as result:
            logging.info('open %s', destination)
            with open(destination, 'wb') as f:
                logging.info('begin io %s', destination)
                f.write(result.data)
                logging.info('end io %s', destination)
        print('save', id, 'as', destination)


@data.command()
@click.option('-n', '--name', required=True, help='A name of the data')
@click.option('-f', '--file', type=click.Path(exists=True, dir_okay=False), required=True, multiple=True,
              help='A file path you want to upload  [multiple]')
@click.option('-m', '--memo', help='Free text that can helpful to explain the data')
@click.option('-t', '--tags', multiple=True, help='Attributes to the data  [multiple]')
def create(name, file, memo, tags):
    """Create new data"""
    api = rest.DataApi(configuration.get_api_client())
    check_upload_files(len(file))
    model = rest.DataApiModelsCreateInputModel(
        memo=memo,
        name=name,
        tags=list(tags)
    )
    result = api.create_data(body=model)
    do_upload_files(api, result.id, file)
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
    result = api.update_data(id, body=model)
    print('updated', result.id)


@data.command()
@click.argument('id', type=int)
def delete(id):
    """Delete data"""
    api = rest.DataApi(configuration.get_api_client())
    api.delete_data(id, _request_timeout=heavy_request_timeout(api.api_client))
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
    result = api.list_data_files(id)
    check_upload_files(len(result) + len(file))
    do_upload_files(api, id, file)


@data.command('delete-file')
@click.argument('id', type=int)
@click.option('-f', '--file-id', type=int, required=True, help='A file id you want to delete')
def delete_file(id, file_id):
    """Delete a file of data"""
    api = rest.DataApi(configuration.get_api_client())
    api.delete_data_file(id, file_id)
    print('deleted', file_id)


def heavy_request_timeout(api_client):
    """Calculate a timeout for time-consuming api call
    :param rest.ApiClient api_client:
    """
    default_timeout = api_client.rest_client.pool_manager.connection_pool_kw['timeout']
    return None if default_timeout is None else default_timeout * 30


def check_upload_files(file_count):
    """
    :param int file_count:
    """
    max_files_per_data = 10000
    if max_files_per_data < file_count:
        raise Exception('Maximum number of files per data is {max_files_per_data}.'
                        .format(max_files_per_data=max_files_per_data))


def do_upload_files(api, id, file):
    """
    :param rest.DataApi api:
    :param int id: data id
    :param list[string] file:
    """
    model = rest.DataApiModelsAddFilesInputModel(files=[])
    for x in file:
        upload_info = object_storage.upload_file(api.api_client, x, 'Data')
        model.files.append(rest.ComponentsAddFileInputModel(file_name=upload_info.file_name,
                                                            stored_path=upload_info.stored_path))
    api.add_data_file(id, body=model, _request_timeout=heavy_request_timeout(api.api_client))
