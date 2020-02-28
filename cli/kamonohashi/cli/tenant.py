# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import click

from kamonohashi.cli import configuration
from kamonohashi.cli import pprint
from kamonohashi.cli import util
from kamonohashi.op import rest


@click.group()
def tenant():
    """Create and manage KAMONOHASHI tenants"""


@tenant.command('list')
def list_tenants():
    """List tenants"""
    api = rest.TenantApi(configuration.get_api_client())
    result = api.list_tenants()
    pprint.pp_table(['id', 'name', 'display_name', 'storage_path'],
                    [[x.id, x.name, x.display_name, x.storage_path] for x in result])


@tenant.command()
@click.argument('id', type=int)
def get(id):
    """Get details of a tenant"""
    api = rest.TenantApi(configuration.get_api_client())
    result = api.get_tenant(id)
    pprint.pp_dict(util.to_dict(result))


@tenant.command()
@click.option('-n', '--name', required=True, help='A name of the tenant')
@click.option('-dn', '--display-name', required=True, help='A display name of the tenant')
@click.option('-gid', '--git-ids', type=int, multiple=True, required=True, help='Git id  [multiple]')
@click.option('-dgid', '--default-git-id', type=int, help='A default gid id')
@click.option('-rid', '--registry-ids', type=int, multiple=True, required=True, help='Registry id  [multiple]')
@click.option('-drid', '--default-registry-id', type=int, help='A default registry id')
@click.option('-sid', '--storage-id', type=int, required=True, help='A storage id')
def create(name, display_name, git_ids, default_git_id, registry_ids, default_registry_id, storage_id):
    """Create a new tenant"""
    api = rest.TenantApi(configuration.get_api_client())
    model = rest.TenantApiModelsCreateInputModel(
        tenant_name=name,
        display_name=display_name,
        git_ids=list(git_ids),
        default_git_id=default_git_id,
        registry_ids=list(registry_ids),
        default_registry_id=default_registry_id,
        storage_id=storage_id,
    )
    result = api.create_tenant(model=model)
    print('created', result.id)


@tenant.command()
@click.argument('id', type=int)
@click.option('-dn', '--display-name', required=True, help='A display name of the tenant')
@click.option('-gid', '--git-ids', type=int, multiple=True, required=True, help='Git id  [multiple]')
@click.option('-dgid', '--default-git-id', type=int, help='A default gid id')
@click.option('-rid', '--registry-ids', type=int, multiple=True, required=True, help='Registry id  [multiple]')
@click.option('-drid', '--default-registry-id', type=int, help='A default registry id')
@click.option('-sid', '--storage-id', type=int, required=True, help='A storage id')
def update(id, display_name, git_ids, default_git_id, registry_ids, default_registry_id, storage_id):
    """Update a tenant"""
    api = rest.TenantApi(configuration.get_api_client())
    model = rest.TenantApiModelsEditInputModel(
        display_name=display_name,
        git_ids=list(git_ids),
        default_git_id=default_git_id,
        registry_ids=list(registry_ids),
        default_registry_id=default_registry_id,
        storage_id=storage_id,
    )
    result = api.update_tenant(id, model=model)
    print('updated', result.id)


@tenant.command()
@click.argument('id', type=int)
def delete(id):
    """Delete a tenant"""
    api = rest.TenantApi(configuration.get_api_client())
    result = api.delete_tenant(id)
    print('deleted', id)
    pprint.pp_dict(util.to_dict(result))
