#!/bin/bash

readonly HELM_VERSION="v2.13.0"
readonly SCRIPT_DIR=$(cd $(dirname $0); pwd)

show_help() {
    echo "available args: prepare, deploy, clean, update, credentials, upgrade, help"
}


prepare(){
    # helmのインストール
    curl https://raw.githubusercontent.com/kubernetes/helm/master/scripts/get | bash /dev/stdin --version $HELM_VERSION
    kubectl create -f helm-rbac-config.yml
    helm init --service-account tiller --upgrade
    # kqi-system namespaceの作成
    kubectl create namespace kqi-system
}

set_credentials(){
    if [ -z "$PASSWORD" ] || [ -z "$DB_PASSWORD" ] || [ -z "$STORAGE_PASSWORD" ]; then
      echo -en "\e[33mAdmin Passwordを入力: \e[m"; read -s PASSWORD
      echo -en "\n\e[33mDB Passwordを入力: \e[m"; read -s DB_PASSWORD
      echo -en "\n\e[33mStorage Secret Keyを入力: \e[m"; read -s STORAGE_PASSWORD
      echo "" # read -sは改行しないので改行 
    fi  
    SET_ARGS="password=$PASSWORD,db_password=$DB_PASSWORD,storage_secretkey=$STORAGE_PASSWORD"
    helm install charts/kamonohashi-credentials --set $SET_ARGS -n kamonohashi-credentials --namespace kqi-system
}

deploy(){
    helm dependency update charts/kamonohashi
    helm install charts/kamonohashi -f conf/settings.yml -n kamonohashi --namespace kqi-system
}

update(){
    helm dependency update charts/kamonohashi
    helm upgrade -i kamonohashi charts/kamonohashi -f conf/settings.yml --namespace kqi-system
}

clean(){
    helm delete --purge kamonohashi
}

main(){
  case $1 in
    prepare) prepare ;;
    deploy) deploy ;;
    update) update;;
    credentials) set_credentials;;
    clean) clean ;;
    help) show_help ;;
    *) show_help ;;
  esac
}

# このスクリプトをどこから実行しても大丈夫なように
# main実行時のみSCRIPT_DIRに移動
(cd $SCRIPT_DIR && main $1)
