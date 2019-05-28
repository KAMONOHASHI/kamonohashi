#/bin/bash

readonly SCRIPT_DIR=$(cd $(dirname $0); pwd)
readonly REPO_ROOT_DIR="$SCRIPT_DIR/../.."
readonly GIT_TAG=$(git tag --points-at HEAD)
readonly VERSION=${GIT_TAG:-"develop"}

docker push kamonohashi/web-api:$VERSION
docker push kamonohashi/cli:$VERSION
docker push kamonohashi/web-pages:$VERSION
