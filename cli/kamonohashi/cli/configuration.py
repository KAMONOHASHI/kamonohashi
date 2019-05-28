# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import json
import logging
import os.path

import urllib3
from kamonohashi.op import rest

config_file_path = os.path.expanduser(os.path.join('~', '.kqi', 'config.json'))


def read_file():
    """Read kamonohashi config file. If not exists or invalid, raise Exception.
    :rtype: dict
    """
    require_login = "Log into KAMONOHASHI first to use 'account login' command."

    if os.path.exists(config_file_path):
        logging.info('open config file %s', config_file_path)
        with open(config_file_path) as f:
            logging.info('begin io %s', config_file_path)
            config_file = json.load(f)
            logging.info('end io %s', config_file_path)
        if not {'server', 'token'} <= set(config_file.keys()):
            raise Exception('Invalid configuration file {config_file_path}. {require_login}'
                            .format(config_file_path=config_file_path, require_login=require_login))
        return config_file

    raise Exception('No configuration file {config_file_path}. {require_login}'
                    .format(config_file_path=config_file_path, require_login=require_login))


def try_read_file():
    """Read kamonohashi config file. If not exists, return an empty dictionary.
    :rtype: dict
    """
    try:
        logging.info('open config file %s', config_file_path)
        with open(config_file_path) as f:
            logging.info('begin io %s', config_file_path)
            config_file = json.load(f)
            logging.info('end io %s', config_file_path)
            return config_file
    except (OSError, IOError) as error:
        logging.info('try_read error %s', error)
        return {}


def update_config_file(**kwargs):
    """Update kamonohashi config file."""
    config_file = try_read_file()
    config_file.update(kwargs)
    config_file = {key: value for key, value in config_file.items() if value is not None}
    logging.info('open config file %s', config_file_path)
    with open(config_file_path, 'w') as f:
        logging.info('begin io %s', config_file_path)
        json.dump(config_file, f, indent=4)
        logging.info('end io %s', config_file_path)


def get_api_client():
    """Get rest.ApiClient.
    :rtype: rest.ApiClient
    """
    config_file = read_file()
    server = config_file['server']
    proxy = config_file.get('proxy')
    api_client = get_api_client_noauth(config_file, server, proxy)
    api_client.configuration.api_key_prefix['Authorization'] = 'Bearer'
    api_client.configuration.api_key['Authorization'] = config_file['token']
    return api_client


def get_api_client_noauth(config_file, server, proxy):
    """Get rest.ApiClient without Authorization.
    :param dict config_file:
    :param str server:
    :param str proxy:
    :rtype: rest.ApiClient
    """
    if proxy:
        configuration = rest.Configuration()
        configuration.proxy = proxy
        api_client = rest.ApiClient(configuration=configuration)
        auth = urllib3.util.url.parse_url(proxy).auth
        if auth:
            proxy_headers = urllib3.util.request.make_headers(proxy_basic_auth=auth)
            api_client.rest_client.pool_manager.connection_pool_kw['_proxy_headers'] = proxy_headers
    else:
        api_client = rest.ApiClient()
    api_client.rest_client.pool_manager.connection_pool_kw['timeout'] = config_file.get('timeout', 30)
    api_client.rest_client.pool_manager.connection_pool_kw['retries'] = False
    api_client.configuration.host = server
    return api_client
