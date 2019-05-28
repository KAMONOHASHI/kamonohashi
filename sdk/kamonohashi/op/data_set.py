# -*- coding: utf-8 -*-

import os.path

from kamonohashi.op import object_storage
from kamonohashi.op import rest


def get(api_client, id):
    api = rest.DataSetApi(api_client)
    result = api.get_dataset(id)
    return result


def create(api_client, model):
    api = rest.DataSetApi(api_client)
    result = api.create_dataset(model=model)
    return result


def update(api_client, id, model):
    api = rest.DataSetApi(api_client)
    result = api.update_dataset(id, model=model)
    return result


def update_meta_info(api_client, id, name=None, memo=None):
    api = rest.DataSetApi(api_client)
    model = rest.DataSetApiModelsEditInputModel(name=name, memo=memo)
    result = api.patch_dataset(id, model=model)
    return result


def delete(api_client, id):
    api = rest.DataSetApi(api_client)
    api.delete_dataset(id)


def download_files(api_client, id, dir_path):
    api = rest.DataSetApi(api_client)
    result = api.list_dataset_files(id, with_url=True)
    pool_manager = api_client.rest_client.pool_manager
    for entry in result.entries:
        for file in entry.files:
            destination_dir_path = os.path.join(dir_path, entry.type, str(file.id))
            object_storage.download_file(pool_manager, file.url, destination_dir_path, file.file_name)
