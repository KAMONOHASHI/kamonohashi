# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import logging
import os
import os.path

import click
from kamonohashi.op import rest
from kamonohashi.op.rest.rest import ApiException

from kamonohashi.cli import util


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
    with click.progressbar(length=length, label=os.path.basename(file_path)) as bar:
        logging.info('open %s', file_path)
        with open(file_path, 'rb') as f:
            logging.info('begin io %s', file_path)
            for i in range(length):
                data = f.read(chunk_size)
                response = pool_manager.request('PUT', upload_info.uris[i], body=data,
                                                headers={'Content-Type': 'application/x-www-form-urlencoded'})
                e_tag = response.getheader('ETag')
                if not (200 <= response.status <= 299 and e_tag):
                    raise ApiException(http_resp=response)
                part_e_tags.append('{}+{}'.format(i + 1, e_tag))
                bar.update(1)
            logging.info('end io %s', file_path)

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

    with util.release_conn(pool_manager.request('GET', url, preload_content=False)) as response:
        if not 200 <= response.status <= 299:
            raise ApiException(http_resp=response)

        content_length = response.getheader('Content-Length')
        if content_length:
            length = (int(content_length) - 1) // chunk_size + 1
            with click.progressbar(length=length, label=file_name) as bar:
                logging.info('open %s', file_path)
                with open(file_path, 'wb') as f:
                    logging.info('begin io %s', file_path)
                    for chunk in response.stream(chunk_size):
                        f.write(chunk)
                        bar.update(1)
                    logging.info('end io %s', file_path)
        else:
            print('Downloading', file_name)
            logging.info('open %s', file_path)
            with open(file_path, 'wb') as f:
                logging.info('begin io %s', file_path)
                for chunk in response.stream(chunk_size):
                    f.write(chunk)
                logging.info('end io %s', file_path)
