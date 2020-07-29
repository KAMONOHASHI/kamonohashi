# -*- coding: utf-8 -*-
#
#  Filter swagger.json for kamonohashi-sdk

import json
import sys

def traverse(obj, callback):
    def traverse_list(obj):
        for value in obj:
            if isinstance(value, dict):
                traverse_dict(value)
            elif isinstance(value, list):
                traverse_list(value)

    def traverse_dict(obj):
        for key, value in obj.items():
            if isinstance(value, str):
                callback(obj, key, value)
            elif isinstance(value, dict):
                traverse_dict(value)
            elif isinstance(value, list):
                traverse_list(value)

    traverse_dict(obj)

def remove_unused_api_and_rename_operation_id(spec):
    used_api = {
        '/api/v1/account': {
            'get': 'get_account',
        },
        '/api/v1/account/login': {
            'post': 'login',
        },
        '/api/v1/account/tenants/{tenantId}/token': {
            'post': 'switch_tenant',
        },
        '/api/v1/admin/tenants': {
            'get': 'list_tenants',
            'post': 'create_tenant',
        },
        '/api/v1/admin/tenants/{id}': {
            'get': 'get_tenant',
            'put': 'update_tenant',
            'delete': 'delete_tenant',
        },
        '/api/v1/data': {
            'get': 'list_data',
            'post': 'create_data',
        },
        '/api/v1/data/{id}': {
            'get': 'get_data', 
            'put': 'update_data',
            'delete': 'delete_data',
        },
        '/api/v1/data/{id}/files': {
            'get': 'list_data_files', 
            'post': 'add_data_file',
        },
        '/api/v1/data/{id}/files/{fileId}': {
            'delete': 'delete_data_file',
        },
        '/api/v1/datasets': {
            'get': 'list_datasets', 
            'post': 'create_dataset',
        },
        '/api/v1/datasets/{id}': {
            'get': 'get_dataset', 
            'put': 'update_dataset', 
            'delete': 'delete_dataset', 
            'patch': 'patch_dataset',
        },
        '/api/v1/datasets/{id}/files': {
            'get': 'list_dataset_files',
        },
        '/api/v1/datasets/{id}/pathpairs': {
            'get': 'list_dataset_pathpairs',
        },
        '/api/v1/datatypes': {
            'get': 'list_dataset_datatypes',
        },
        '/api/v1/inferences': {
            'get': 'list_inference', 
        },
        '/api/v1/inferences/{id}/container-files': {
            'get': 'list_inference_container_files',
        },
        '/api/v1/inferences/{id}/complete': {
            'post': 'complete_inference',
        },
        '/api/v1/inferences/{id}/halt': {
            'post': 'halt_inference',
        },
        '/api/v1/inferences/run': {
            'post': 'create_inference',
        },
        '/api/v1/inferences/{id}': {
            'get': 'get_inference', 
            'delete': 'delete_inference', 
            'put': 'update_inference',
        },
        '/api/v1/inferences/{id}/files': {
            'get': 'list_inference_files',
            'post': 'add_inference_file',
        },
        '/api/v1/inferences/{id}/files/{fileId}': {
            'delete': 'delete_inference_file',
        },
        '/api/v1/training': {
            'get': 'list_training', 
        },
        '/api/v1/training/run': {
            'post': 'create_training',
        },
        '/api/v1/training/{id}': {
            'get': 'get_training', 
            'delete': 'delete_training', 
            'put': 'update_training',
        },
        '/api/v1/training/{id}/complete': {
            'post': 'complete_training',
        },
        '/api/v1/training/{id}/container-files': {
            'get': 'list_training_container_files',
        },
        '/api/v1/training/{id}/files': {
            'get': 'list_training_files',
            'post': 'add_training_file',
        },
        '/api/v1/training/{id}/files/{fileId}': {
            'delete': 'delete_training_file',
        },
        '/api/v1/training/{id}/halt': {
            'post': 'halt_training',
        },
        '/api/v1/training/{id}/tensorboard': {
            'delete': 'halt_tensorboard',
        },
        '/api/v1/preprocessings': {
            'get': 'list_preprocessings', 
            'post': 'create_preprocessing',
        },
        '/api/v1/preprocessings/{id}': {
            'get': 'get_preprocessing',
            'put': 'update_preprocessing',
            'delete': 'delete_preprocessing', 
            'patch': 'patch_preprocessing',
        },
        '/api/v1/preprocessings/{id}/histories': {
            'get': 'list_preprocessing_histories',
        },
        '/api/v1/preprocessings/{id}/histories/{dataId}': {
            'post': 'create_preprocessing_history', 
            'delete': 'delete_preprocessing_history',
        },
        '/api/v1/preprocessings/{id}/histories/{dataId}/data': {
            'post': 'add_preprocessing_history_files',
        },
        '/api/v1/preprocessings/{id}/histories/{dataId}/complete': {
            'post': 'complete_preprocessing_history',
        },
        '/api/v1/preprocessings/{id}/histories/{dataId}/halt': {
            'post': 'halt_preprocessing_history',
        },
        '/api/v1/preprocessings/{id}/run': {
            'post': 'run_preprocessing',
        },
        '/api/v1/notebook/{id}/halt': {
            'post': 'halt_notebook',
        },
        '/api/v1/upload/complete': {
            'post': 'complete_upload',
        },
        '/api/v1/upload/parameter': {
            'get': 'get_upload_paramater',
        },
    }

    paths = spec['paths']

    for x in list(paths.keys()):
        if x in used_api:
            api_def = paths[x]
            used_method = used_api[x]
            for y in list(api_def.keys()):
                if y in used_method:
                    api_def[y]['operationId'] = used_method[y]
                else:
                    del api_def[y]
        else:
            del paths[x]

def remove_unused_model(spec):
    used_models = set()
    paths = spec['paths']
    definitions = spec['definitions']

    def callback(obj, key, value):
        if key == '$ref':
            model_name = value.replace('#/definitions/', '', 1)
            if model_name not in used_models:
                used_models.add(model_name)
                traverse(definitions[model_name], callback)

    traverse(paths, callback)

    for x in list(definitions.keys()):
        if x not in used_models:
            del definitions[x]

def rename_model(spec):
    rename = (
        ('Nssol.Platypus.ApiModels.', ''),
        ('Nssol.Platypus.Infrastructure.', ''),
        ('Nssol.Platypus.LogicModels.', ''),
        ('System.Collections.Generic.KeyValuePair`2[[System.String, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.String, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]', 'System.Collections.Generic.KeyValuePair'),
    )
    definitions = spec['definitions']
    for x in list(definitions.keys()):
        for old, new in rename:
            if x.startswith(old):
                model_def = definitions[x]
                definitions[x.replace(old, new, 1)] = model_def
                del definitions[x]

    def callback(obj, key, value):
        if key == '$ref':
            model_name = value.replace('#/definitions/', '', 1)
            for old, new in rename:
                if model_name.startswith(old):
                    obj[key] = value.replace(old, new, 1)

    traverse(spec, callback)

def remove_comment(spec):
    comment = []

    def callback(obj, key, value):
        if key in ('summary', 'description'):
            comment.append((obj, key))

    traverse(spec, callback)

    for obj, key in comment:
        del obj[key]

fin = sys.stdin
fout = sys.stdout
spec = json.load(fin)

remove_unused_api_and_rename_operation_id(spec)
remove_unused_model(spec)
rename_model(spec)
if len(sys.argv) <= 1 or sys.argv[1] != '--preserve-comment':
    remove_comment(spec)
spec['info']['description'] = 'A platform for deep learning'

json.dump(spec, fout, sort_keys=True, indent=2, ensure_ascii=False)
