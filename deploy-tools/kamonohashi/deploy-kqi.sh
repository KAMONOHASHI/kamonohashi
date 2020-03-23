#!/bin/bash

readonly HELM_VERSION="v2.16.1"
readonly SCRIPT_DIR=$(cd $(dirname $0); pwd)

show_help() {
    echo "available args: prepare, deploy, clean, update, credentials, upgrade, help"
}

prepare(){
    kubectl apply -f helm-rbac-config.yml
    curl https://raw.githubusercontent.com/kubernetes/helm/master/scripts/get | bash /dev/stdin --version $HELM_VERSION
    helm init --service-account tiller --upgrade --force-upgrade --wait
    kubectl apply -f kqi-namespace.yml
}

set_credentials(){
    if [ -z "$PASSWORD" ] || [ -z "$DB_PASSWORD" ] || [ -z "$STORAGE_PASSWORD" ]; then
      echo -en "\e[33mAdmin Passwordを入力: \e[m"; read -s PASSWORD
      echo -en "\n\e[33mDB Passwordを入力: \e[m"; read -s DB_PASSWORD
      echo -en "\n\e[33mStorage Secret Keyを入力: \e[m"; read -s STORAGE_PASSWORD
      echo "" # read -sは改行しないので改行 
    fi  
    SET_ARGS="password=$PASSWORD,db_password=$DB_PASSWORD,storage_secretkey=$STORAGE_PASSWORD"
    helm upgrade kamonohashi-credentials charts/kamonohashi-credentials -i --set $SET_ARGS --namespace kqi-system
}

deploy(){
    helm dependency update charts/kamonohashi
    helm upgrade kamonohashi charts/kamonohashi -f conf/settings.yml -i --namespace kqi-system
}

update(){
    helm dependency update charts/kamonohashi
    helm upgrade -i kamonohashi charts/kamonohashi -f conf/settings.yml --namespace kqi-system
}

clean(){
    helm delete --purge kamonohashi
}

main(){
  cd $SCRIPT_DIR
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

main $@