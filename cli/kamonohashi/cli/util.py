# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import contextlib

import six


def to_dict_value(value):
    if hasattr(value, 'to_dict'):
        return to_dict(value)
    if isinstance(value, list):
        return [to_dict_value(x) for x in value]
    if isinstance(value, dict):
        return {k: to_dict_value(v) for k, v in value.items()}
    return value


def to_dict(model):
    """Returns a model properties as a dict.
    This works even when 'value of dict' is 'list of models'.
    """
    result = {}
    for attr, _ in six.iteritems(model.swagger_types):
        value = getattr(model, attr)
        result[attr] = to_dict_value(value)
    return result


@contextlib.contextmanager
def release_conn(response):
    """Call urllib3.response.HTTPResponse.release_conn().
    :param urllib3.response.HTTPResponse response:
    """
    try:
        yield response
    finally:
        response.release_conn()
