# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import click
from kamonohashi.op import rest

from kamonohashi.cli import configuration


@click.group()
def inference():
    """Create and manage KAMONOHASHI inference"""


@inference.command()
@click.argument('id', type=int)
def halt(id):
    """Halt inference"""
    api = rest.InferenceApi(configuration.get_api_client())
    result = api.halt_inference(id)
    print('halted', result.id)


@inference.command()
@click.argument('id', type=int)
def complete(id):
    """Complete inference"""
    api = rest.InferenceApi(configuration.get_api_client())
    result = api.complete_inference(id)
    print('completed', result.id)
