# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import io
import json
import logging
import os
import os.path

import click
from kamonohashi.op import rest

from kamonohashi.cli import configuration
from kamonohashi.cli import object_storage
from kamonohashi.cli import pprint
from kamonohashi.cli import util


@click.group()
def preprocessing():
    """Create and manage KAMONOHASHI preprocessings"""


@preprocessing.command('list')
@click.option('--count', type=click.IntRange(1, 10000), default=1000, show_default=True, help='Maximum number of data to list')
@click.option('--id', help='id')
@click.option('--name', help='name')
@click.option('--created-at', help='created at')
@click.option('--memo', help='memo')
def list_preprocessings(count, id, name, created_at, memo):
    """List preprocessings filtered by conditions"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    per_page = 1000
    command_args = {
        'id': id,
        'name': name,
        'memo': memo,
        'created_at': created_at,
    }
    args = {key: value for key, value in command_args.items() if value is not None}
    if count <= per_page:
        result = api.list_preprocessings(per_page=count, **args)
    else:
        total_pages = (count - 1) // per_page + 1
        result = []
        for page in range(1, total_pages + 1):
            page_result = api.list_preprocessings(page=page, **args)
            result.extend(page_result)
            if len(page_result) < per_page:
                break

    pprint.pp_table(['id', 'name', 'created_at', 'memo'],
                    [[x.id, x.name, x.created_at, x.memo] for x in result[:count]])


@preprocessing.command()
@click.argument('id', type=int)
@click.option('-d', '--destination', type=click.Path(dir_okay=False), help='A file path of the output as a json file')
def get(id, destination):
    """Get details of a preprocessing"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    if destination is None:
        result = api.get_preprocessing(id)
        pprint.pp_dict(util.to_dict(result))
    else:
        with util.release_conn(api.get_preprocessing(id, _preload_content=False)) as result:
            logging.info('open %s', destination)
            with open(destination, 'wb') as f:
                logging.info('begin io %s', destination)
                f.write(result.data)
                logging.info('end io %s', destination)
        print('save', id, 'as', destination)


@preprocessing.command()
@click.option('-f', '--file', required=True, type=click.Path(exists=True, dir_okay=False),
              help="""{
  "name": @name,
  "entryPoint": @entryPoint,
  "containerImage": {
    "registryId": @registryId,
    "image": @image,
    "tag": "@tag,
  },
  "gitModel": {
    "gitId": @gitId,
    "repository": @repository,
    "owner": @owner,
    "branch": @branch,
    "commitId": @commitId,
  },
  "memo": @memo,
  "cpu": @cpu,
  "memory": @memory,
  "gpu": @gpu
}""")
def create(file):
    """Create a new preprocessing"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    logging.info('open %s', file)
    with io.open(file, 'r', encoding='utf-8') as f:
        logging.info('begin io %s', file)
        json_dict = json.load(f)
        logging.info('end io %s', file)
    result = api.create_preprocessing(model=json_dict)
    print('created', result.id)


@preprocessing.command()
@click.argument('id', type=int)
@click.option('-f', '--file', required=True, type=click.Path(exists=True, dir_okay=False),
              help="""{
  "name": @name,
  "entryPoint": @entryPoint,
  "containerImage": {
    "registryId": @registryId,
    "image": @image,
    "tag": "@tag,
  },
  "gitModel": {
    "gitId": @gitId,
    "repository": @repository,
    "owner": @owner,
    "branch": @branch,
    "commitId": @commitId,
  },
  "memo": @memo,
  "cpu": @cpu,
  "memory": @memory,
  "gpu": @gpu
}""")
def update(id, file):
    """Update a preprocessing"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    logging.info('open %s', file)
    with io.open(file, 'r', encoding='utf-8') as f:
        logging.info('begin io %s', file)
        json_dict = json.load(f)
        logging.info('end io %s', file)
    result = api.update_preprocessing(id, model=json_dict)
    print('updated', result.id)


@preprocessing.command('update-meta-info')
@click.argument('id', type=int)
@click.option('-n', '--name', help='A name to update')
@click.option('-m', '--memo', help='A memo to update')
@click.option('-c', '--cpu', type=int, help='A cpu to update')
@click.option('-mem', '--memory', type=int, help='A memory to update')
@click.option('-g', '--gpu', type=int, help='A gpu to update')
def patch(id, name, memo, cpu, memory, gpu):
    """Update meta information of a preprocessing"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    model = rest.PreprocessingApiModelsEditInputModel(name=name, memo=memo, cpu=cpu, memory=memory, gpu=gpu)
    result = api.patch_preprocessing(id, model=model)
    print('meta-info updated', result.id)


@preprocessing.command()
@click.argument('id', type=int)
def delete(id):
    """Delete a preprocesssing"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    api.delete_preprocessing(id)
    print('deleted', id)


@preprocessing.command('list-histories')
@click.argument('id', type=int)
def list_histories(id):
    """List histories of a preprocessing"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    result = api.list_preprocessing_histories(id)
    pprint.pp_table(['data_id', 'data_name', 'created_at', 'status'],
                    [[x.data_id, x.data_name, x.created_at, x.status] for x in result])


@preprocessing.command('build-history')
@click.argument('id', type=int)
@click.option('-did', '--data-id', type=int, required=True, help='A source data id')
@click.option('-s', '--source', type=click.Path(exists=True, file_okay=False), required=True, help='A directory path to the processed data')
@click.option('-m', '--memo', help='Free text that can helpful to explain the data')
@click.option('-t', '--tags', multiple=True, help='Attributes to the data  [multiple]')
def build_history(id, data_id, source, memo, tags):
    """Build history structure of a preprocessing"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    api.create_preprocessing_history(id, data_id)

    for entry in os.listdir(source):
        if os.path.isdir(os.path.join(source, entry)):
            uploaded_files = []
            for root, _, files in os.walk(os.path.join(source, entry)):
                for file in files:
                    upload_info = object_storage.upload_file(api.api_client, os.path.join(root, file), 'Data')
                    uploaded_files.append(rest.ComponentsAddFileInputModel(file_name=upload_info.file_name,
                                                                           stored_path=upload_info.stored_path))
            model = rest.PreprocessingApiModelsAddOutputDataInputModel(files=uploaded_files, name=entry,
                                                                       memo=memo, tags=list(tags))
            api.add_preprocessing_history_files(id, data_id, model=model)

    result = api.complete_preprocessing_history(id, data_id)
    print('built ', result.preprocess_id, '.', result.data_id, sep='')


@preprocessing.command('build-history-files')
@click.argument('id', type=int)
@click.option('-did', '--data-id', type=int, required=True, help='A source data id')
@click.option('-s', '--source', type=click.Path(exists=True, file_okay=False), required=True, help='A directory path to the processed data')
@click.option('-m', '--memo', help='Free text that can helpful to explain the data')
@click.option('-t', '--tags', multiple=True, help='Attributes to the data  [multiple]')
def build_history_files(id, data_id, source, memo, tags):
    """Build file structure for existing history"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    for entry in os.listdir(source):
        if os.path.isdir(os.path.join(source, entry)):
            uploaded_files = []
            for root, _, files in os.walk(os.path.join(source, entry)):
                for file in files:
                    upload_info = object_storage.upload_file(api.api_client, os.path.join(root, file), 'Data')
                    uploaded_files.append(rest.ComponentsAddFileInputModel(file_name=upload_info.file_name,
                                                                           stored_path=upload_info.stored_path))
            model = rest.PreprocessingApiModelsAddOutputDataInputModel(files=uploaded_files, name=entry,
                                                                       memo=memo, tags=list(tags))
            api.add_preprocessing_history_files(id, data_id, model=model)

    api.complete_preprocessing_history(id, data_id)


@preprocessing.command('delete-history')
@click.argument('id', type=int)
@click.option('-did', '--data-id', type=int, required=True, help='A source data id')
def delete_history(id, data_id):
    """Delete a history of a preprocessing"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    api.delete_preprocessing_history(id, data_id)
    print('deleted ', id, '.', data_id, sep='')


@preprocessing.command('halt-history')
@click.argument('id', type=int)
@click.option('-did', '--data-id', type=int, required=True, help='A source data id')
def halt_history(id, data_id):
    """Halt a preprocessing of a history"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    result = api.halt_preprocessing_history(id, data_id)
    print('halted ', result.preprocess_id, '.', result.data_id, sep='')


@preprocessing.command()
@click.argument('id', type=int)
@click.option('-did', '--data-id', type=int, required=True, multiple=True, help='A source data id  [multiple]')
@click.option('-c', '--cpu', type=int, required=True, help='A number of core you want to assign to this preprocessing')
@click.option('-mem', '--memory', type=int, required=True, help='How much memory(GB) you want to assign to this preprocessing')
@click.option('-g', '--gpu', type=int, required=True, help='A number of GPUs you want to assign to this preprocessing')
@click.option('-p', '--partition',
              help='A cluster partition. Partition is an arbitrary string but typically is a type of GPU or cluster.')
@click.option('-o', '--options', type=(str, str), multiple=True,
              help='Options of this preprocessing. The options are stored in the environment variables  [multiple]')
def run(id, data_id, cpu, memory, gpu, partition, options):
    """Run a preprocessing"""
    api = rest.PreprocessingApi(configuration.get_api_client())
    option_dict = {key: value for key, value in options} if options else None
    for x in data_id:
        model = rest.PreprocessingApiModelsRunPreprocessHistoryInputModel(
            cpu=cpu, data_id=x, gpu=gpu, memory=memory, options=option_dict, partition=partition)
        result = api.run_preprocessing(id, model=model)
        print('started ', result.preprocess_id, '.', result.data_id, sep='')
