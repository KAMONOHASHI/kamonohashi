# -*- coding: utf-8 -*-

import os
import os.path

from kamonohashi.op import rest
from kamonohashi.op.rest.rest import ApiException


def upload_file(api_client, file_path, file_type):
    """Upload a file to object storage.
    :param rest.ApiClient api_client:
    :param str file_path: source file path
    :param str file_type: file type in object storage
    :rtype: rest.StorageLogicModelsMultiPartUploadModel
    """
    chunk_size = 1024 * 1024 * 16
    length = (os.path.getsize(file_path) - 1) // chunk_size + 1

    api = rest.StorageApi(api_client)
    upload_info = api.get_upload_paramater(os.path.basename(file_path), length, file_type)

    part_e_tags = []
    pool_manager = api_client.rest_client.pool_manager
    with open(file_path, 'rb') as f:
        for i in range(length):
            data = f.read(chunk_size)
            response = pool_manager.request('PUT', upload_info.uris[i], body=data,
                                            headers={'Content-Type': 'application/x-www-form-urlencoded'})
            e_tag = response.getheader('ETag')
            if not (200 <= response.status <= 299 and e_tag):
                raise ApiException(http_resp=response)
            part_e_tags.append('{}+{}'.format(i + 1, e_tag))

    model = rest.StorageLogicModelsCompleteMultiplePartUploadInputModel(
        key=upload_info.key,
        part_e_tags=part_e_tags,
        upload_id=upload_info.upload_id,
    )
    api.complete_upload(model=model)
    return upload_info


def download_file(pool_manager, url, dir_path, file_name):
    """Download a file from object storage.
    :param urllib3.PoolManager pool_manager:
    :param str url: download url
    :param str dir_path: destination directory path
    :param str file_name: destination file name
    """
    file_name = os.path.basename(file_name)
    chunk_size = 1024 * 1024 * 16

    if not os.path.exists(dir_path):
        os.makedirs(dir_path)
    file_path = os.path.join(dir_path, file_name)

    response = pool_manager.request('GET', url, preload_content=False)
    try:
        if not 200 <= response.status <= 299:
            raise ApiException(http_resp=response)

        with open(file_path, 'wb') as f:
            for chunk in response.stream(chunk_size):
                f.write(chunk)
    finally:
        response.release_conn()
