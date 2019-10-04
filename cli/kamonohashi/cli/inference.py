# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import os.path

import click
from kamonohashi.op import rest

from kamonohashi.cli import configuration
from kamonohashi.cli import object_storage
from kamonohashi.cli import pprint


@click.group()
def inference():
    """Create and manage KAMONOHASHI inference"""


@inference.command('list')
@click.option('--count', type=click.IntRange(1, 10000), default=1000, show_default=True, help='Maximum number of data to list')
@click.option('--id', help='id')
@click.option('--name', help='name')
@click.option('--started-at', help='started at')
@click.option('--data-set', help='data set')
@click.option('--memo', help='memo')
@click.option('--status', help='status')
@click.option('--entry-point', help='entry point')
def list_inference(count, id, name, started_at, data_set, memo, status, entry_point):
    """List inference filtered by conditions"""
    api = rest.InferenceApi(configuration.get_api_client())
    per_page = 1000
    command_args = {
        'id': id,
        'name': name,
        'started_at': started_at,
        'data_set': data_set,
        'memo': memo,
        'status': status,
        'entry_point': entry_point,
    }
    args = {key: value for key, value in command_args.items() if value is not None}
    if count <= per_page:
        result = api.list_inference(per_page=count, **args)
    else:
        total_pages = (count - 1) // per_page + 1
        result = []
        for page in range(1, total_pages + 1):
            page_result = api.list_inference(page=page, **args)
            result.extend(page_result)
            if len(page_result) < per_page:
                break

    pprint.pp_table(['id', 'name', 'started_at', 'dataset', 'memo', 'value', 'status'],
                    [[x.id, x.name, x.created_at, x.data_set.name, x.memo, x.output_value, x.status] for x in result[:count]])


@inference.command('download-container-files')
@click.argument('id', type=int)
@click.option('-d', '--destination', type=click.Path(exists=True, file_okay=False), required=True,
              help='A path to the output files')
@click.option('-s', '--source', help='A path to the source root in the container')
def download_container_files(id, destination, source):
    """Download files in a container"""
    api = rest.InferenceApi(configuration.get_api_client())
    pool_manager = api.api_client.rest_client.pool_manager

    def download_entries(path):
        result = api.list_inference_container_files(id, path=path, with_url=True)
        for x in result.files:
            if os.path.isabs(path):
                _, tail = os.path.splitdrive(path)
                object_storage.download_file(pool_manager, x.url, destination + tail, x.file_name)
            else:
                object_storage.download_file(pool_manager, x.url, os.path.join(destination, path), x.file_name)
        for x in result.dirs:
            download_entries(os.path.join(path, x.dir_name))

    source = source if source is not None else '/'
    download_entries(source)


@inference.command()
@click.argument('id', type=int)
def halt(id):
    """Halt inference"""
    api = rest.InferenceApi(configuration.get_api_client())
    result = api.halt_inference(id)
    print('halted', result.id)


@inference.command()
@click.argument('id', type=int)
def complete(id):
    """Complete inference"""
    api = rest.InferenceApi(configuration.get_api_client())
    result = api.complete_inference(id)
    print('completed', result.id)
