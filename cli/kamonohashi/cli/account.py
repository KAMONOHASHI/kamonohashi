# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import click
from kamonohashi.op import rest

from kamonohashi.cli import configuration
from kamonohashi.cli import pprint


@click.group()
def account():
    """Manage your KAMONOHASHI account"""


@account.command()
@click.option('-s', '--server', help='API server url like http://platypus')
@click.option('-u', '--user', prompt=True, confirmation_prompt=False, help='User name')
@click.option('-p', '--password', prompt=True, confirmation_prompt=False, hide_input=True, help='Password')
@click.option('-t', '--tenant', type=int, help='A tenant id')
@click.option('--proxy', help='HTTP proxy in the form http://user:password@host:port')
def login(server, user, password, tenant, proxy):
    """Login to KAMONOHASHI system"""
    config_file = configuration.try_read_file()
    if (not server) and (not proxy) and config_file.get('server'):
        server = config_file.get('server')
        proxy = config_file.get('proxy')
    else:
        server = server or click.prompt('Server')
    server = server.rstrip('/')
    api = rest.AccountApi(configuration.get_api_client_noauth(config_file, server, proxy))
    expire_days = config_file.get('expireDays')
    expires_in = expire_days * 24 * 3600 if expire_days is not None else None
    model = rest.AccountApiModelsLoginInputModel(user_name=user, password=password, tenant_id=tenant, expires_in=expires_in)
    result = api.login(model=model)

    configuration.update_config_file(server=server, proxy=proxy, token=result.token)
    print('user name:', result.user_name)
    pprint.pp_primitive('tenant:', result.tenant_name)
    print('expires in:', result.expires_in, 'seconds')
    print('token:', result.token)


@account.command('switch-tenant')
@click.argument('tenant-id', type=int)
def switch_tenant(tenant_id):
    """Switch to another tenant"""
    api = rest.AccountApi(configuration.get_api_client())
    config_file = configuration.try_read_file()
    expire_days = config_file.get('expireDays')
    expires_in = expire_days * 24 * 3600 if expire_days is not None else None
    result = api.switch_tenant(tenant_id, expires_in=expires_in)
    configuration.update_config_file(token=result.token)
    print('user name:', result.user_name)
    pprint.pp_primitive('selected tenant:', result.tenant_id, result.tenant_name)
    print('expires in:', result.expires_in, 'seconds')


@account.command('get')
def get_account():
    """Get a summary of your account"""
    api = rest.AccountApi(configuration.get_api_client())
    result = api.get_account()
    print('user name:', result.user_name)
    pprint.pp_primitive('selected tenant:', result.selected_tenant.id, result.selected_tenant.display_name)
    print('assigned tenant:')
    for tenant in result.tenants:
        selected = '*' if tenant.id == result.selected_tenant.id else ' '
        pprint.pp_primitive('   ', selected, tenant.id, tenant.display_name)
