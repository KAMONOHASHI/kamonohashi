# -*- coding: utf-8 -*-
import os.path

from setuptools import find_packages
from setuptools import setup

with open(os.path.join(os.path.dirname(__file__), 'VERSION')) as f:
    version = f.read().strip()

setup(
    name='kamonohashi-cli',
    version=version,
    description='KAMONOHASHI Command Line Interface',
    long_description='Python command line interface for KAMONOHASHI https://kamonohashi.ai/',
    author='NS Solutions Corporation',
    author_email='kamonohashi-support@jp.nssol.nipponsteel.com',
    url='https://github.com/KAMONOHASHI/kamonohashi',
    license='Apache License 2.0',
    packages=find_packages(),
    install_requires=[
        'click == 6.7',
        'urllib3 >= 1.23',
        'six >= 1.10',
        'kamonohashi-sdk == ' + version,
    ],
    entry_points={
        'console_scripts': ['kqi = kamonohashi.cli.kqi:kqi_main']
    },
)
