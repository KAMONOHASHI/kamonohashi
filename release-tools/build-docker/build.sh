#/bin/bash

readonly GIT_TAG=$(git tag --points-at HEAD)
readonly TAG=${GIT_TAG:-"develop"}
readonly VERSION=${GIT_TAG:-"develop:-`git rev-parse HEAD`}
readonly SCRIPT_DIR=$(cd $(dirname $0); pwd)
readonly REPO_ROOT_DIR="$SCRIPT_DIR/../.."

main(){
    docker build --network host -f $SCRIPT_DIR/web-api-Dockerfile -t kamonohashi/web-api:$TAG --build-arg version=$VERSION web-api
    docker build --network host -f $SCRIPT_DIR/cli-Dockerfile -t kamonohashi/cli:$TAG --build-arg version=$VERSION .
    docker build --network host -f $SCRIPT_DIR/web-pages-Dockerfile -t kamonohashi/web-pages:$TAG --build-arg version=$VERSION web-pages
}

# このスクリプトをどこから実行しても大丈夫なように
# main実行時のみモノリポジトリトップに移動
(cd $REPO_ROOT_DIR && main)
