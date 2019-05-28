# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import io
import json
import logging
import os.path

import click
from kamonohashi.op import rest

from kamonohashi.cli import configuration
from kamonohashi.cli import object_storage
from kamonohashi.cli import pprint
from kamonohashi.cli import util


@click.group()
def data_set():
    """Create and manage KAMONOHASHI datasets"""


@data_set.command('list')
@click.option('--count', type=click.IntRange(1, 10000), default=1000, show_default=True, help='Maximum number of data to list')
@click.option('--id', help='id')
@click.option('--name', help='name')
@click.option('--memo', help='memo')
@click.option('--created-at', help='created at')
def list_datasets(count, id, name, memo, created_at):
    """List datasets filtered by conditions"""
    api = rest.DataSetApi(configuration.get_api_client())
    per_page = 1000
    command_args = {
        'id': id,
        'name': name,
        'memo': memo,
        'created_at': created_at,
    }
    args = {key: value for key, value in command_args.items() if value is not None}
    if count <= per_page:
        result = api.list_datasets(per_page=count, **args)
    else:
        total_pages = (count - 1) // per_page + 1
        result = []
        for page in range(1, total_pages + 1):
            page_result = api.list_datasets(page=page, **args)
            result.extend(page_result)
            if len(page_result) < per_page:
                break

    pprint.pp_table(['id', 'name', 'created_at', 'memo'],
                    [[x.id, x.name, x.created_at, x.memo] for x in result[:count]])


@data_set.command()
@click.argument('id', type=int)
@click.option('-d', '--destination', type=click.Path(dir_okay=False), help='A file path of the output as a json file')
def get(id, destination):
    """Get details of a detaset"""
    api = rest.DataSetApi(configuration.get_api_client())
    if destination is None:
        result = api.get_dataset(id)
        pprint.pp_dict(util.to_dict(result))
    else:
        with util.release_conn(api.get_dataset(id, _preload_content=False)) as result:
            logging.info('open %s', destination)
            with open(destination, 'wb') as f:
                logging.info('begin io %s', destination)
                f.write(result.data)
                logging.info('end io %s', destination)
        print('save', id, 'as', destination)


@data_set.command()
@click.option('-f', '--file', required=True, type=click.Path(exists=True, dir_okay=False),
              help="""{
  "name": @name,
  "memo": @memo,
  "entries": {
    "additionalProp1": [
      {
        "id": @dataId
      }
    ],
    "additionalProp2": [
      {
        "id": @dataId
      }
    ],
    "additionalProp3": [
      {
        "id": @dataId
      }
    ]
  }
}""")
def create(file):
    """Create a new dataset"""
    api = rest.DataSetApi(configuration.get_api_client())
    logging.info('open %s', file)
    with io.open(file, 'r', encoding='utf-8') as f:
        logging.info('begin io %s', file)
        json_dict = json.load(f)
        logging.info('end io %s', file)
    result = api.create_dataset(model=json_dict)
    print('created', result.id)


@data_set.command()
@click.argument('id', type=int)
@click.option('-f', '--file', required=True, type=click.Path(exists=True, dir_okay=False),
              help="""{
  "name": @name,
  "memo": @memo,
  "entries": {
    "additionalProp1": [
      {
        "id": @dataId
      }
    ],
    "additionalProp2": [
      {
        "id": @dataId
      }
    ],
    "additionalProp3": [
      {
        "id": @dataId
      }
    ]
  }
}""")
def update(id, file):
    """Update a dataset"""
    api = rest.DataSetApi(configuration.get_api_client())
    logging.info('open %s', file)
    with io.open(file, 'r', encoding='utf-8') as f:
        logging.info('begin io %s', file)
        json_dict = json.load(f)
        logging.info('end io %s', file)
    result = api.update_dataset(id, model=json_dict)
    print('updated', result.id)


@data_set.command('update-meta-info')
@click.argument('id', type=int)
@click.option('-n', '--name', help='A new name')
@click.option('-m', '--memo', help='A new memo')
def update_meta_info(id, name, memo):
    """Update meta information of a dataset"""
    api = rest.DataSetApi(configuration.get_api_client())
    model = rest.DataSetApiModelsEditInputModel(name=name, memo=memo)
    result = api.patch_dataset(id, model=model)
    print('meta-info updated', result.id)


@data_set.command()
@click.argument('id', type=int)
def delete(id):
    """Delete a dataset"""
    api = rest.DataSetApi(configuration.get_api_client())
    api.delete_dataset(id)
    print('deleted', id)


@data_set.command('download-files')
@click.argument('id', type=int)
@click.option('-d', '--destination', type=click.Path(exists=True, file_okay=False), required=True,
              help='An output directory path')
@click.option('-t', '--type', 'data_type', type=click.Choice(['training', 'testing', 'validation']), multiple=True,
              help='A data type to download  [multiple]')
def download_files(id, destination, data_type):
    """Download files of a dataset"""
    api = rest.DataSetApi(configuration.get_api_client())
    result = api.list_dataset_files(id, with_url=True)
    pool_manager = api.api_client.rest_client.pool_manager
    for entry in result.entries:
        if not data_type or entry.type in data_type:
            for file in entry.files:
                destination_dir_path = os.path.join(destination, entry.type, str(file.id))
                object_storage.download_file(pool_manager, file.url, destination_dir_path, file.file_name)


@data_set.command('list-data-types')
def list_data_types():
    """List data types of a dataset"""
    api = rest.DataSetApi(configuration.get_api_client())
    result = api.list_dataset_datatypes()
    for x in result:
        print(x.name)
