#!/usr/bin/env bash
#
#  Build and distribute KAMONOHASHI sdk and cli packages

shopt -s -o errexit

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" > /dev/null 2>&1 && pwd)"
sdk="${script_dir}/../../sdk"
cli="${script_dir}/../../cli"

dist_clean() {
    rm -f "${sdk}/VERSION"
    rm -rf "${sdk}/dist"
    rm -rf "${sdk}/build"
    rm -rf "${sdk}/kamonohashi_sdk.egg-info"
    find "${sdk}" -name __pycache__ -type d -execdir rm -rf {} +
    find "${sdk}" -name '*.pyc' -type f -execdir rm -f {} +
    rm -f "${cli}/VERSION"
    rm -rf "${cli}/dist"
    rm -rf "${cli}/build"
    rm -rf "${cli}/kamonohashi_cli.egg-info"
    find "${cli}" -name __pycache__ -type d -execdir rm -rf {} +
    find "${cli}" -name '*.pyc' -type f -execdir rm -f {} +
}

case "$1" in
  clean)
    dist_clean
    ;;
  list)  
    ls "${sdk}/dist"
    ls "${cli}/dist"
    ;;
  build)
    dist_clean
    version="${2:-"$(git -C "${script_dir}" tag --points-at HEAD)"}"
    version="${version:?"No version specified"}"
    echo "${version}" > "${sdk}/VERSION"
    echo "${version}" > "${cli}/VERSION"
    (cd "${sdk}" && python setup.py sdist bdist_wheel --universal > /dev/null)
    (cd "${cli}" && python setup.py sdist bdist_wheel --universal > /dev/null)
    ls "${sdk}/dist"
    ls "${cli}/dist"
    ;;
  test-upload)
    twine upload -r testpypi "${sdk}/dist/*"
    twine upload -r testpypi "${cli}/dist/*"
    ;;
  master-upload)
    twine upload -r pypi "${sdk}/dist/*"
    twine upload -r pypi "${cli}/dist/*"
    ;;
  *)
    echo "$(basename "${BASH_SOURCE[0]}") clean|list|build [VERSION]|test-upload|master-upload"
esac
