# coding: utf-8

"""
    KAMONOHASHI API

    A platform for deep learning  # noqa: E501

    OpenAPI spec version: v1
    Contact: kamonohashi-support@jp.nssol.nipponsteel.com
    Generated by: https://github.com/swagger-api/swagger-codegen.git
"""


import pprint
import re  # noqa: F401

import six


class DataApiModelsAddFilesInputModelFile(object):
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
        'file_name': 'str',
        'stored_path': 'str'
    }

    attribute_map = {
        'file_name': 'fileName',
        'stored_path': 'storedPath'
    }

    def __init__(self, file_name=None, stored_path=None):  # noqa: E501
        """DataApiModelsAddFilesInputModelFile - a model defined in Swagger"""  # noqa: E501

        self._file_name = None
        self._stored_path = None
        self.discriminator = None

        if file_name is not None:
            self.file_name = file_name
        if stored_path is not None:
            self.stored_path = stored_path

    @property
    def file_name(self):
        """Gets the file_name of this DataApiModelsAddFilesInputModelFile.  # noqa: E501


        :return: The file_name of this DataApiModelsAddFilesInputModelFile.  # noqa: E501
        :rtype: str
        """
        return self._file_name

    @file_name.setter
    def file_name(self, file_name):
        """Sets the file_name of this DataApiModelsAddFilesInputModelFile.


        :param file_name: The file_name of this DataApiModelsAddFilesInputModelFile.  # noqa: E501
        :type: str
        """

        self._file_name = file_name

    @property
    def stored_path(self):
        """Gets the stored_path of this DataApiModelsAddFilesInputModelFile.  # noqa: E501


        :return: The stored_path of this DataApiModelsAddFilesInputModelFile.  # noqa: E501
        :rtype: str
        """
        return self._stored_path

    @stored_path.setter
    def stored_path(self, stored_path):
        """Sets the stored_path of this DataApiModelsAddFilesInputModelFile.


        :param stored_path: The stored_path of this DataApiModelsAddFilesInputModelFile.  # noqa: E501
        :type: str
        """

        self._stored_path = stored_path

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
        if issubclass(DataApiModelsAddFilesInputModelFile, dict):
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
        if not isinstance(other, DataApiModelsAddFilesInputModelFile):
            return False

        return self.__dict__ == other.__dict__

    def __ne__(self, other):
        """Returns true if both objects are not equal"""
        return not self == other
