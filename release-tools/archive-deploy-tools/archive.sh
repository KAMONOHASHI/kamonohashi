#/bin/bash

readonly GIT_TAG=$(git tag --points-at HEAD)

readonly SCRIPT_DIR=$(cd $(dirname $0); pwd)
# パスの文字列結合には安全のために%/をつけておく
# ref: https://qiita.com/magicant/items/03c1fd1f1296f3e58171
# TODO: 他のbashスクリプトにも反映
readonly EXCLUDE_LIST=${SCRIPT_DIR%/}/exclude.list

readonly REPO_ROOT_DIR=$(cd ${SCRIPT_DIR%/}/../..; pwd)

readonly KQI_CHART_DIR="deploy-tools/kamonohashi/charts/kamonohashi"
readonly KQI_CRED_CHART_DIR="deploy-tools/kamonohashi/charts/kamonohashi-credentials"

main(){
   # git tagがない場合に引数からバージョンを取るワークアラウンド。一貫性のために使用は避けるべき
   local VERSION=${GIT_TAG:-$1}
   VERSION=$VERSION envsubst < ${KQI_CHART_DIR%/}/Chart.yaml-template > ${KQI_CHART_DIR%/}/Chart.yaml
   VERSION=$VERSION envsubst < ${KQI_CRED_CHART_DIR%/}/Chart.yaml-template > ${KQI_CRED_CHART_DIR%/}/Chart.yaml
   tar -X $EXCLUDE_LIST -zcf deploy-tools-$VERSION.tar.gz deploy-tools/
}
# このスクリプトをどこから実行しても大丈夫なように
# main実行時のみモノリポジトリトップに移動
(cd $REPO_ROOT_DIR && main $1)