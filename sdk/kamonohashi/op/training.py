# -*- coding: utf-8 -*-

from kamonohashi.op import object_storage
from kamonohashi.op import rest


def get(api_client, id):
    api = rest.TrainingApi(api_client)
    result = api.get_training(id)
    return result


def create(api_client, name, registry_image, registry_tag, data_set_id, entry_point, git_owner, git_repository, cpu, memory, gpu,
           git_branch=None, git_commit=None, partition=None, memo=None, parent_id=None, options=(), registry_id=None, git_id=None):
    api = rest.TrainingApi(api_client)
    container_image = rest.ComponentsContainerImageInputModel(image=registry_image, registry_id=registry_id, tag=registry_tag)
    git_model = rest.ComponentsGitCommitInputModel(branch=git_branch, commit_id=git_commit, git_id=git_id, owner=git_owner, repository=git_repository)
    option_dict = {key: value for key, value in options} if options else None
    model = rest.TrainingApiModelsCreateInputModel(
        container_image=container_image, cpu=cpu, data_set_id=data_set_id, entry_point=entry_point, git_model=git_model,
        gpu=gpu, memo=memo, memory=memory, name=name, options=option_dict, parent_id=parent_id, partition=partition)
    result = api.create_training(model=model)
    return result


def update(api_client, id, memo=None, favorite=None):
    api = rest.TrainingApi(api_client)
    model = rest.TrainingApiModelsEditInputModel(memo=memo, favorite=favorite)
    result = api.update_training(id, model=model)
    return result


def upload_file(api_client, id, file_path):
    api = rest.TrainingApi(api_client)
    attached_info = object_storage.upload_file(api_client, file_path, 'TrainingHistoryAttachedFiles')
    model = rest.ComponentsAddFileInputModel(file_name=attached_info.file_name, stored_path=attached_info.stored_path)
    result = api.add_training_file(id, model=model)
    return result


def download_files(api_client, id, dir_path):
    api = rest.TrainingApi(api_client)
    result = api.list_training_files(id, with_url=True)
    pool_manager = api_client.rest_client.pool_manager
    for x in result:
        object_storage.download_file(pool_manager, x.url, dir_path, x.file_name)
