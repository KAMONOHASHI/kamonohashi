# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import click
from kamonohashi.op import rest

from kamonohashi.cli import configuration


@click.group()
def experiment():
    """Create and manage KAMONOHASHI experiment"""

@experiment.command()
@click.argument('id', type=int)
def halt(id):
    """Halt experiment"""
    api = rest.ExperimentApi(configuration.get_api_client())
    result = api.halt_experiment(id)
    print('halted', result.id)