# -*- coding: utf-8 -*-

import urllib3

from kamonohashi.op import rest


def login(server, user, password, tenant_id=None, timeout=30, retries=False, expires_in=None, proxy=None):
    api_client = _get_api_client_noauth(proxy)
    api_client.rest_client.pool_manager.connection_pool_kw['timeout'] = timeout
    api_client.rest_client.pool_manager.connection_pool_kw['retries'] = retries
    api_client.configuration.host = server
    api = rest.AccountApi(api_client)
    model = rest.AccountApiModelsLoginInputModel(user_name=user, password=password, tenant_id=tenant_id, expires_in=expires_in)
    result = api.login(model=model)

    api_client.configuration.api_key_prefix['Authorization'] = 'Bearer'
    api_client.configuration.api_key['Authorization'] = result.token
    return api_client, result


def switch_tenant(api_client, tenant_id, expires_in=None):
    api = rest.AccountApi(api_client)
    result = api.switch_tenant(tenant_id, expires_in=expires_in)

    new_api_client = _get_api_client_noauth(api_client.configuration.proxy)
    new_api_client.rest_client.pool_manager.connection_pool_kw['timeout'] = api_client.rest_client.pool_manager.connection_pool_kw['timeout']
    new_api_client.rest_client.pool_manager.connection_pool_kw['retries'] = api_client.rest_client.pool_manager.connection_pool_kw['retries']
    new_api_client.configuration.host = api_client.configuration.host
    new_api_client.configuration.api_key_prefix['Authorization'] = api_client.configuration.api_key_prefix['Authorization']
    new_api_client.configuration.api_key['Authorization'] = result.token
    return new_api_client, result


def _get_api_client_noauth(proxy):
    if proxy:
        configuration = rest.Configuration()
        configuration.proxy = proxy
        api_client = rest.ApiClient(configuration=configuration)
        auth = urllib3.util.url.parse_url(proxy).auth
        if auth:
            proxy_headers = urllib3.util.request.make_headers(proxy_basic_auth=auth)
            api_client.rest_client.pool_manager.connection_pool_kw['_proxy_headers'] = proxy_headers
        return api_client
    return rest.ApiClient()
