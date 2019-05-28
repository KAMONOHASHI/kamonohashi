# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import logging
import logging.handlers
import os
import sys

import click
import pkg_resources
import six
from kamonohashi.op.rest.rest import ApiException

from kamonohashi.cli import account
from kamonohashi.cli import data
from kamonohashi.cli import data_set
from kamonohashi.cli import inference
from kamonohashi.cli import preprocessing
from kamonohashi.cli import training

name = 'kamonohashi-cli'
version = pkg_resources.get_distribution(name).version
sdk_name = 'kamonohashi-sdk'
sdk_version = pkg_resources.get_distribution(sdk_name).version


def configure_logger():
    """Configure root logger."""
    log_dir_path = os.path.expanduser(os.path.join('~', '.kqi'))
    log_file_path = os.path.join(log_dir_path, 'kqi.log')

    if not os.path.exists(log_dir_path):
        os.makedirs(log_dir_path)

    logger = logging.getLogger()
    logger.setLevel(logging.INFO)
    formatter = logging.Formatter('%(asctime)s [%(levelname)s] %(filename)s:%(lineno)d - %(message)s',
                                  datefmt='%Y-%m-%d %H:%M:%S')
    encoding = None if six.PY2 else 'utf-8'
    handler = logging.handlers.TimedRotatingFileHandler(log_file_path, when='D', backupCount=10, encoding=encoding)
    handler.setFormatter(formatter)
    logger.addHandler(handler)


@click.group(context_settings=dict(help_option_names=['-h', '--help']),
             help='KAMONOHASHI Command Line Interface. version: {version}'.format(version=version))
def kqi():
    pass


kqi.add_command(account.account)
kqi.add_command(data_set.data_set, 'dataset')
kqi.add_command(data.data)
kqi.add_command(inference.inference)
kqi.add_command(training.training)
kqi.add_command(preprocessing.preprocessing)


def kqi_main():
    configure_logger()
    try:
        logging.info('%s: %s', name, version)
        logging.info('%s: %s', sdk_name, sdk_version)
        logging.info('sys.platform: %s', sys.platform)
        logging.info('sys.version: %s', sys.version)
        if 0 < len(sys.argv):
            logging.info('sys.argv[0]: %s', sys.argv[0])
        kqi()
        return 0
    except ApiException as error:
        if six.PY3 and error.body and isinstance(error.body, bytes):
            error.body = error.body.decode('utf-8')
        print('[ERROR]', error.status, error.reason)
        print(error.body)
        logging.exception(error)
    except Exception as error:
        print('[ERROR]', error)
        logging.exception(error)
    return 1


if __name__ == "__main__":
    # for pydevd & python2 glitch
    kqi(sys.argv[1:])
