# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import os
import os.path

import click

from kamonohashi.cli import configuration
from kamonohashi.cli import object_storage
from kamonohashi.op import rest


@click.group()
def experiment():
    """Create and manage KAMONOHASHI experiment"""


@experiment.command('build-preprocess-files')
@click.argument('id', type=int)
@click.option('-s', '--source', type=click.Path(exists=True, file_okay=False), required=True,
              help='A directory path to the processed data')
@click.option('-m', '--memo', help='Free text that can helpful to explain the data')
@click.option('-t', '--tags', multiple=True, help='Attributes to the data  [multiple]')
def build_history_files(id, source, memo, tags):
    """Build file structure for experiment preprocess"""
    api = rest.ExperimentApi(configuration.get_api_client())
    for entry in os.listdir(source):
        if os.path.isdir(os.path.join(source, entry)):
            uploaded_files = []
            for root, _, files in os.walk(os.path.join(source, entry)):
                for file in files:
                    upload_info = object_storage.upload_file(api.api_client, os.path.join(root, file), 'Data')
                    uploaded_files.append(rest.ComponentsAddFileInputModel(file_name=upload_info.file_name,
                                                                           stored_path=upload_info.stored_path))
            model = rest.ExperimentApiModelsAddOutputDataInputModel(files=uploaded_files, name=entry,
                                                                    memo=memo, tags=list(tags))
            api.add_experiment_preprocessing_files(id, model=model)
