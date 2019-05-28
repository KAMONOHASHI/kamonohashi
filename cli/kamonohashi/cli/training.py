# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import os.path

import click
from kamonohashi.op import rest

from kamonohashi.cli import configuration
from kamonohashi.cli import object_storage
from kamonohashi.cli import pprint
from kamonohashi.cli import util


@click.group()
def training():
    """Create and manage KAMONOHASHI training"""


@training.command('list')
@click.option('--count', type=click.IntRange(1, 10000), default=1000, show_default=True, help='Maximum number of data to list')
@click.option('--id', help='id')
@click.option('--name', help='name')
@click.option('--started-at', help='started at')
@click.option('--data-set', help='data set')
@click.option('--memo', help='memo')
@click.option('--status', help='status')
@click.option('--entry-point', help='entry point')
def list_training(count, id, name, started_at, data_set, memo, status, entry_point):
    """List training filtered by conditions"""
    api = rest.TrainingApi(configuration.get_api_client())
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
        result = api.list_training(per_page=count, **args)
    else:
        total_pages = (count - 1) // per_page + 1
        result = []
        for page in range(1, total_pages + 1):
            page_result = api.list_training(page=page, **args)
            result.extend(page_result)
            if len(page_result) < per_page:
                break

    pprint.pp_table(['id', 'name', 'started_at', 'dataset', 'memo', 'status'],
                    [[x.id, x.name, x.created_at, x.data_set.name, x.memo, x.status] for x in result[:count]])


@training.command()
@click.argument('id', type=int)
def get(id):
    """Get details of training"""
    api = rest.TrainingApi(configuration.get_api_client())
    result = api.get_training(id)
    pprint.pp_dict(util.to_dict(result))


@training.command()
@click.option('-n', '--name', required=True, help='A name of the training')
@click.option('-rid', '--registry-id', type=int, help='A docker registry id')
@click.option('-rim', '--registry-image', required=True, help='A docker image name you want to run')
@click.option('-rt', '--registry-tag', required=True, help='A tag of the docker image')
@click.option('-d', '--data-set-id', type=int, required=True, help='A dataset id you want to use for the training')
@click.option('-e', '--entry-point', help='Training execution command')
@click.option('-gid', '--git-id', type=int, help='A git id')
@click.option('-go', '--git-owner', required=True,
              help="The owner of the repository which contains source codes you want to execute. Usually owner is the "
                   "first path of the repository's url. In case of this url, "
                   "https://github.com/kamonohashi/kamonohashi-cli, kamonohashi is a owner name.")
@click.option('-gr', '--git-repository', required=True,
              help="The repository name of the repository whichi contains your source codes. Usually repository name is"
                   " the second path of the repository's url. In case of this url,"
                   " https://github.com/kamonohashi/kamonohashi-cli, kamonohashi-cli is a repository name.")
@click.option('-gb', '--git-branch',
              help='The branch of your git repository. If you omit this option, master branch is used.')
@click.option('-gc', '--git-commit',
              help='The git commit of your source code. If you omit this option, the latest one is used.')
@click.option('-c', '--cpu', type=int, required=True, help='A number of core you want to assign to this training')
@click.option('-mem', '--memory', type=int, required=True, help='How much memory(GB) you want to assign to this training')
@click.option('-g', '--gpu', type=int, required=True, help='A number of GPUs you want to assign to this training')
@click.option('-p', '--partition',
              help='A cluster partition. Partition is an arbitrary string but typically is a type of GPU or cluster.')
@click.option('-m', '--memo', help='A memo of this training.')
@click.option('-pid', '--parent-id',
              help='A parent id of this training. Currently, the system only makes a relationship to the parent training but do nothing.')
@click.option('-o', '--options', type=(str, str), multiple=True,
              help='Options of this training. The options are stored in the environment variables  [multiple]')
def create(name, registry_image, registry_tag, data_set_id, entry_point,
           git_owner, git_repository, git_branch, git_commit, cpu, memory, gpu, partition, memo,
           parent_id, options, registry_id, git_id):
    """Submit new training"""
    api = rest.TrainingApi(configuration.get_api_client())
    container_image = rest.ComponentsContainerImageInputModel(image=registry_image, registry_id=registry_id, tag=registry_tag)
    git_model = rest.ComponentsGitCommitInputModel(branch=git_branch, commit_id=git_commit, git_id=git_id, owner=git_owner, repository=git_repository)
    option_dict = {key: value for key, value in options} if options else None
    model = rest.TrainingApiModelsCreateInputModel(
        container_image=container_image, cpu=cpu, data_set_id=data_set_id, entry_point=entry_point, git_model=git_model,
        gpu=gpu, memo=memo, memory=memory, name=name, options=option_dict, parent_id=parent_id, partition=partition)
    result = api.create_training(model=model)
    print('created', result.id)


@training.command()
@click.argument('id', type=int)
def delete(id):
    """Delete training"""
    api = rest.TrainingApi(configuration.get_api_client())
    api.delete_training(id)
    print('deleted', id)


@training.command()
@click.argument('id', type=int)
@click.option('-m', '--memo', help='A memo to update')
@click.option('-fav/-unfav', '--favorite/--un-favorite', default=None, help='A favorite to update')
def update(id, memo, favorite):
    """Update training"""
    api = rest.TrainingApi(configuration.get_api_client())
    model = rest.TrainingApiModelsEditInputModel(memo=memo, favorite=favorite)
    result = api.update_training(id, model=model)
    print('updated', result.id)


@training.command('upload-file')
@click.argument('id', type=int)
@click.option('-f', '--file-path', type=click.Path(exists=True, dir_okay=False), help='A file path you want to upload')
def upload_file(id, file_path):
    """Upload a file to training"""
    api = rest.TrainingApi(configuration.get_api_client())
    attached_info = object_storage.upload_file(api.api_client, file_path, 'TrainingHistoryAttachedFiles')
    model = rest.ComponentsAddFileInputModel(file_name=attached_info.file_name, stored_path=attached_info.stored_path)
    api.add_training_file(id, model=model)


@training.command('list-files')
@click.argument('id', type=int)
def list_files(id):
    """List files of training"""
    api = rest.TrainingApi(configuration.get_api_client())
    result = api.list_training_files(id)
    pprint.pp_table(['file_id', 'file_name'],
                    [[x.file_id, x.file_name] for x in result])


@training.command('download-files')
@click.argument('id', type=int)
@click.option('-d', '--destination', type=click.Path(exists=True, file_okay=False), required=True,
              help='A path to the output files')
def download_files(id, destination):
    """Download files of training"""
    api = rest.TrainingApi(configuration.get_api_client())
    result = api.list_training_files(id, with_url=True)
    pool_manager = api.api_client.rest_client.pool_manager
    for x in result:
        object_storage.download_file(pool_manager, x.url, destination, x.file_name)


@training.command('download-container-files')
@click.argument('id', type=int)
@click.option('-d', '--destination', type=click.Path(exists=True, file_okay=False), required=True,
              help='A path to the output files')
@click.option('-s', '--source', help='A path to the source root in the container')
def download_container_files(id, destination, source):
    """Download files in a container"""
    api = rest.TrainingApi(configuration.get_api_client())
    pool_manager = api.api_client.rest_client.pool_manager

    def download_entries(path):
        result = api.list_training_container_files(id, path=path, with_url=True)
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


@training.command('delete-file')
@click.argument('id', type=int)
@click.option('-f', '--file-id', type=int, required=True, help='A file id you want to delete')
def delete_file(id, file_id):
    """Delete a file of training"""
    api = rest.TrainingApi(configuration.get_api_client())
    api.delete_training_file(id, file_id)
    print('deleted', file_id)


@training.command()
@click.argument('id', type=int)
def halt(id):
    """Halt training"""
    api = rest.TrainingApi(configuration.get_api_client())
    result = api.halt_training(id)
    print('halted', result.id)


@training.command()
@click.argument('id', type=int)
def complete(id):
    """Complete training"""
    api = rest.TrainingApi(configuration.get_api_client())
    result = api.complete_training(id)
    print('completed', result.id)
