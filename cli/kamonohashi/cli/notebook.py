# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import click
from kamonohashi.op import rest

from kamonohashi.cli import configuration


@click.group()
def notebook():
    """Create and manage KAMONOHASHI notebook"""


@notebook.command()
@click.argument('id', type=int)
def halt(id):
    """Halt notebook"""
    api = rest.NotebookApi(configuration.get_api_client())
    result = api.halt_notebook(id)
    print('halted', result.id)

