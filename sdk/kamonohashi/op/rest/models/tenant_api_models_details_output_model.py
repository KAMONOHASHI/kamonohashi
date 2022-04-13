# coding: utf-8

"""
    KAMONOHASHI API

    A platform for deep learning  # noqa: E501

    OpenAPI spec version: v2
    Contact: kamonohashi-support@jp.nssol.nipponsteel.com
    Generated by: https://github.com/swagger-api/swagger-codegen.git
"""


import pprint
import re  # noqa: F401

import six


class TenantApiModelsDetailsOutputModel(object):
    """NOTE: This class is auto generated by the swagger code generator program.

    Do not edit the class manually.
    """

    """
    Attributes:
      swagger_types (dict): The key is attribute name
                            and the value is attribute type.
      attribute_map (dict): The key is attribute name
                            and the value is json key in definition.
    """
    swagger_types = {
        'available_infinite_time_notebook': 'bool',
        'created_at': 'str',
        'created_by': 'str',
        'default_git_id': 'int',
        'default_registry_id': 'int',
        'display_name': 'str',
        'git_ids': 'list[int]',
        'id': 'int',
        'modified_at': 'str',
        'modified_by': 'str',
        'name': 'str',
        'registry_ids': 'list[int]',
        'storage_id': 'int',
        'storage_path': 'str',
        'user_group_ids': 'list[int]'
    }

    attribute_map = {
        'available_infinite_time_notebook': 'availableInfiniteTimeNotebook',
        'created_at': 'createdAt',
        'created_by': 'createdBy',
        'default_git_id': 'defaultGitId',
        'default_registry_id': 'defaultRegistryId',
        'display_name': 'displayName',
        'git_ids': 'gitIds',
        'id': 'id',
        'modified_at': 'modifiedAt',
        'modified_by': 'modifiedBy',
        'name': 'name',
        'registry_ids': 'registryIds',
        'storage_id': 'storageId',
        'storage_path': 'storagePath',
        'user_group_ids': 'userGroupIds'
    }

    def __init__(self, available_infinite_time_notebook=None, created_at=None, created_by=None, default_git_id=None, default_registry_id=None, display_name=None, git_ids=None, id=None, modified_at=None, modified_by=None, name=None, registry_ids=None, storage_id=None, storage_path=None, user_group_ids=None):  # noqa: E501
        """TenantApiModelsDetailsOutputModel - a model defined in Swagger"""  # noqa: E501

        self._available_infinite_time_notebook = None
        self._created_at = None
        self._created_by = None
        self._default_git_id = None
        self._default_registry_id = None
        self._display_name = None
        self._git_ids = None
        self._id = None
        self._modified_at = None
        self._modified_by = None
        self._name = None
        self._registry_ids = None
        self._storage_id = None
        self._storage_path = None
        self._user_group_ids = None
        self.discriminator = None

        if available_infinite_time_notebook is not None:
            self.available_infinite_time_notebook = available_infinite_time_notebook
        if created_at is not None:
            self.created_at = created_at
        if created_by is not None:
            self.created_by = created_by
        if default_git_id is not None:
            self.default_git_id = default_git_id
        if default_registry_id is not None:
            self.default_registry_id = default_registry_id
        if display_name is not None:
            self.display_name = display_name
        if git_ids is not None:
            self.git_ids = git_ids
        if id is not None:
            self.id = id
        if modified_at is not None:
            self.modified_at = modified_at
        if modified_by is not None:
            self.modified_by = modified_by
        if name is not None:
            self.name = name
        if registry_ids is not None:
            self.registry_ids = registry_ids
        if storage_id is not None:
            self.storage_id = storage_id
        if storage_path is not None:
            self.storage_path = storage_path
        if user_group_ids is not None:
            self.user_group_ids = user_group_ids

    @property
    def available_infinite_time_notebook(self):
        """Gets the available_infinite_time_notebook of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The available_infinite_time_notebook of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: bool
        """
        return self._available_infinite_time_notebook

    @available_infinite_time_notebook.setter
    def available_infinite_time_notebook(self, available_infinite_time_notebook):
        """Sets the available_infinite_time_notebook of this TenantApiModelsDetailsOutputModel.


        :param available_infinite_time_notebook: The available_infinite_time_notebook of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: bool
        """

        self._available_infinite_time_notebook = available_infinite_time_notebook

    @property
    def created_at(self):
        """Gets the created_at of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The created_at of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: str
        """
        return self._created_at

    @created_at.setter
    def created_at(self, created_at):
        """Sets the created_at of this TenantApiModelsDetailsOutputModel.


        :param created_at: The created_at of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: str
        """

        self._created_at = created_at

    @property
    def created_by(self):
        """Gets the created_by of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The created_by of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: str
        """
        return self._created_by

    @created_by.setter
    def created_by(self, created_by):
        """Sets the created_by of this TenantApiModelsDetailsOutputModel.


        :param created_by: The created_by of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: str
        """

        self._created_by = created_by

    @property
    def default_git_id(self):
        """Gets the default_git_id of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The default_git_id of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: int
        """
        return self._default_git_id

    @default_git_id.setter
    def default_git_id(self, default_git_id):
        """Sets the default_git_id of this TenantApiModelsDetailsOutputModel.


        :param default_git_id: The default_git_id of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: int
        """

        self._default_git_id = default_git_id

    @property
    def default_registry_id(self):
        """Gets the default_registry_id of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The default_registry_id of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: int
        """
        return self._default_registry_id

    @default_registry_id.setter
    def default_registry_id(self, default_registry_id):
        """Sets the default_registry_id of this TenantApiModelsDetailsOutputModel.


        :param default_registry_id: The default_registry_id of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: int
        """

        self._default_registry_id = default_registry_id

    @property
    def display_name(self):
        """Gets the display_name of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The display_name of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: str
        """
        return self._display_name

    @display_name.setter
    def display_name(self, display_name):
        """Sets the display_name of this TenantApiModelsDetailsOutputModel.


        :param display_name: The display_name of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: str
        """

        self._display_name = display_name

    @property
    def git_ids(self):
        """Gets the git_ids of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The git_ids of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: list[int]
        """
        return self._git_ids

    @git_ids.setter
    def git_ids(self, git_ids):
        """Sets the git_ids of this TenantApiModelsDetailsOutputModel.


        :param git_ids: The git_ids of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: list[int]
        """

        self._git_ids = git_ids

    @property
    def id(self):
        """Gets the id of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The id of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: int
        """
        return self._id

    @id.setter
    def id(self, id):
        """Sets the id of this TenantApiModelsDetailsOutputModel.


        :param id: The id of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: int
        """

        self._id = id

    @property
    def modified_at(self):
        """Gets the modified_at of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The modified_at of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: str
        """
        return self._modified_at

    @modified_at.setter
    def modified_at(self, modified_at):
        """Sets the modified_at of this TenantApiModelsDetailsOutputModel.


        :param modified_at: The modified_at of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: str
        """

        self._modified_at = modified_at

    @property
    def modified_by(self):
        """Gets the modified_by of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The modified_by of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: str
        """
        return self._modified_by

    @modified_by.setter
    def modified_by(self, modified_by):
        """Sets the modified_by of this TenantApiModelsDetailsOutputModel.


        :param modified_by: The modified_by of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: str
        """

        self._modified_by = modified_by

    @property
    def name(self):
        """Gets the name of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The name of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: str
        """
        return self._name

    @name.setter
    def name(self, name):
        """Sets the name of this TenantApiModelsDetailsOutputModel.


        :param name: The name of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: str
        """

        self._name = name

    @property
    def registry_ids(self):
        """Gets the registry_ids of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The registry_ids of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: list[int]
        """
        return self._registry_ids

    @registry_ids.setter
    def registry_ids(self, registry_ids):
        """Sets the registry_ids of this TenantApiModelsDetailsOutputModel.


        :param registry_ids: The registry_ids of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: list[int]
        """

        self._registry_ids = registry_ids

    @property
    def storage_id(self):
        """Gets the storage_id of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The storage_id of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: int
        """
        return self._storage_id

    @storage_id.setter
    def storage_id(self, storage_id):
        """Sets the storage_id of this TenantApiModelsDetailsOutputModel.


        :param storage_id: The storage_id of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: int
        """

        self._storage_id = storage_id

    @property
    def storage_path(self):
        """Gets the storage_path of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The storage_path of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: str
        """
        return self._storage_path

    @storage_path.setter
    def storage_path(self, storage_path):
        """Sets the storage_path of this TenantApiModelsDetailsOutputModel.


        :param storage_path: The storage_path of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: str
        """

        self._storage_path = storage_path

    @property
    def user_group_ids(self):
        """Gets the user_group_ids of this TenantApiModelsDetailsOutputModel.  # noqa: E501


        :return: The user_group_ids of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :rtype: list[int]
        """
        return self._user_group_ids

    @user_group_ids.setter
    def user_group_ids(self, user_group_ids):
        """Sets the user_group_ids of this TenantApiModelsDetailsOutputModel.


        :param user_group_ids: The user_group_ids of this TenantApiModelsDetailsOutputModel.  # noqa: E501
        :type: list[int]
        """

        self._user_group_ids = user_group_ids

    def to_dict(self):
        """Returns the model properties as a dict"""
        result = {}

        for attr, _ in six.iteritems(self.swagger_types):
            value = getattr(self, attr)
            if isinstance(value, list):
                result[attr] = list(map(
                    lambda x: x.to_dict() if hasattr(x, "to_dict") else x,
                    value
                ))
            elif hasattr(value, "to_dict"):
                result[attr] = value.to_dict()
            elif isinstance(value, dict):
                result[attr] = dict(map(
                    lambda item: (item[0], item[1].to_dict())
                    if hasattr(item[1], "to_dict") else item,
                    value.items()
                ))
            else:
                result[attr] = value
        if issubclass(TenantApiModelsDetailsOutputModel, dict):
            for key, value in self.items():
                result[key] = value

        return result

    def to_str(self):
        """Returns the string representation of the model"""
        return pprint.pformat(self.to_dict())

    def __repr__(self):
        """For `print` and `pprint`"""
        return self.to_str()

    def __eq__(self, other):
        """Returns true if both objects are equal"""
        if not isinstance(other, TenantApiModelsDetailsOutputModel):
            return False

        return self.__dict__ == other.__dict__

    def __ne__(self, other):
        """Returns true if both objects are not equal"""
        return not self == other
