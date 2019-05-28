# -*- coding: utf-8 -*-

from __future__ import print_function, absolute_import, with_statement

import unicodedata

import six


def pp_primitive(*objs, **kwargs):
    separator = ''
    for x in objs:
        print(separator, sep='', end='')
        try:
            print(x, sep='', end='')
        except UnicodeError:
            x = x.encode('unicode-escape').decode('utf-8')
            print(x, sep='', end='')
        separator = kwargs.get('sep', ' ')
    print(end=kwargs.get('end', '\n'))


def indent(level):
    return ' ' * (level * 2)


def indent_element(level):
    return ' ' * ((level - 1) * 2) + '- '


def escape_primitive(obj):
    if isinstance(obj, bool):
        return 'true' if obj else 'false'
    if obj is None:
        return ''
    return obj


def pp_key_value(padding, key, value, level):
    print(padding, key, ':', sep='', end='')
    if isinstance(value, dict):
        print()
        pp_dict(value, level + 1)
    elif isinstance(value, list):
        print()
        pp_list(value, level + 1)
    else:
        print(' ', end='')
        pp_primitive(escape_primitive(value))


def pp_list(obj, level):
    for element in obj:
        if isinstance(element, dict):
            padding = indent_element(level + 1)
            for key, value in element.items():
                pp_key_value(padding, key, value, level + 1)
                padding = indent(level + 1)
        elif isinstance(element, list):
            pp_list(element, level + 1)
        else:
            print(indent_element(level + 1), end='')
            pp_primitive(escape_primitive(element))


def pp_dict(obj, level=0):
    """Pretty-print json dictionary.
    :param dict obj: json dictionary
    :param int level: indent level
    """
    for key, value in obj.items():
        pp_key_value(indent(level), key, value, level)


def text(obj):
    """
    :rtype: six.text_type
    """
    if isinstance(obj, six.text_type):
        return obj
    if isinstance(obj, six.binary_type):
        return obj.decode('utf-8')
    if isinstance(obj, bool):
        return text('true') if obj else text('false')
    if obj is None:
        return text('')
    if isinstance(obj, list):
        return text(',').join(text(x) for x in obj)
    return six.text_type(obj)


def width_in_terminal(text):
    """
    :param six.text_type text:
    """
    try:
        return sum(2 if unicodedata.east_asian_width(x) in ('F', 'W', 'A') else 1 for x in text)
    except UnicodeError:
        return -1


def pp_table(header, data):
    """Pretty-print table.
    :param list header: list of header element
    :param list data: list of list of element
    """
    text_rows = [[text(element) for element in row] for row in [header] + data]
    table = []
    for row in text_rows:
        cells = [x.splitlines() for x in row]
        max_cell_len = max(len(x) for x in cells)
        for i in range(max_cell_len):
            table.append([x[i] if i < len(x) else '' for x in cells])

    column_width = [max(width_in_terminal(row[i]) for row in table) for i in range(len(header))]

    for row in table:
        column_sep = ''
        for element, width in zip(row, column_width):
            padding = ' ' * (width - width_in_terminal(element))
            pp_primitive(column_sep, element, padding, sep='', end='')
            column_sep = ' '
        print()
