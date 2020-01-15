#!/bin/bash

readonly HELP_URL="https://kamonohashi.ai/docs/install-and-update"
readonly SCRIPT_DIR=$(cd $(dirname $0); pwd)

# 関数定義

show_logo(){
echo -e "\e[36;1m"
cat << "EOD"
--------------------------------------------------------------------------------------------
            ..NNNMMMMNNgJ..         #   #      ##     #     #    #####    #      #    ##### 
        ..NMMMMMMMMMMMMMMMMNa,      #  #       ##     ##   ##   #     #   ##     #   #     #
      .dMMMMMMMMMMMMMMMMMMMM"!      # #       #  #    # # # #   #     #   # #    #   #     #
    .dMMMMMMMMMMMMMMMMMMMD`         ###       #  #    # # # #   #     #   #  #   #   #     #
   .MMMMMMMMMMMMMMMMMMM'`           # #      #    #   #  #  #   #     #   #   #  #   #     #
  .MMMMMMMMMMMMMMMMMMM'    .MM      #  #     ######   #     #   #     #   #    # #   #     #
  MMMMMMMH""MMMMMMMMMM{    MMM'     #   #    #    #   #     #   #     #   #     ##   #     #
 .MMMMM'       _7"WMMY"             #    #   #    #   #     #    #####    #      #    ##### 
 dMMMM!                                                                                     
 dMMM#                              #    #     ##       ####     #    #      #              
 JMMMN                              #    #     ##      #         #    #      #              
 .MMMMp                             #    #    #  #     #         #    #      #              
  ?MMMMMNJ..                        ######    #  #      ##       ######      #              
   TMMMMMMMMMMMMMMMMMMMMMMMMNN..    #    #   #    #       ##     #    #      #              
    ?MMMMMMMMMMMMMMMMMMMMMMMMMMMMt  #    #   ######         #    #    #      #              
      TMMMMMMMMMMMMMMMMMMMMMMMMM!   #    #   #    #    #    #    #    #      #              
        7MMMMMMMMMMMMMMMMMMMM"`     #    #   #    #    #####     #    #      #              
           ?YMMMMMMMMMMMMB"`                                               クラスタ構築ツール                  
--------------------------------------------------------------------------------------------
EOD
echo -e "\e[m"
}

show_step(){
  echo ""
  echo -e "\e[36;1m##################################################"
  echo "$1"
  echo -e "##################################################\e[m"
  echo ""
  sleep 2
}

ask_node_conf(){
  echo -en "\e[33mKubernetes masterをデプロイするサーバ名: \e[m"; read KUBE_MASTER
  echo -en "\e[33mKAMONOHASHIをデプロイするサーバ名: \e[m"; read KQI_NODE
  echo -en "\e[33mStorageをデプロイするサーバ名: \e[m"; read STORAGE
  echo -en "\e[33mGPU サーバ名(,区切りで複数可): \e[m"; read GPU_NODES_COMMA
}

ask_ssh_conf(){
  echo -en "\e[33mSSHユーザー名: \e[m"; read SSH_USER
  ask_ssh_password
  ask_sudo_password
}

ask_ssh_password(){
  echo -en "\e[33mSSHパスワード(キーを使用する場合は未入力でEnter): \e[m"; read -s SSH_PASS
  echo "" # read -sは改行しないので改行 
  if [ $SSH_PASS ]; then
    echo -en "\e[33m確認のためSSHパスワードをもう一度入力してください: \e[m"; read -s SSH_PASS_2
    echo "" # read -sは改行しないので改行 
    if [[ $SSH_PASS != $SSH_PASS_2 ]]; then
      echo "ERROR: SSHパスワードと再入力パスワードが一致しません。"
      ask_ssh_password
    fi
  fi
}

ask_sudo_password(){
  echo -en "\e[33mSUDOパスワード(不要なら未入力でEnter): \e[m"; read -s SUDO_PASS
  echo "" # read -sは改行しないので改行 
  if [ $SUDO_PASS ]; then
    echo -en "\e[33m確認のためSUDOパスワードをもう一度入力してください: \e[m"; read -s SUDO_PASS_2
    echo "" # read -sは改行しないので改行 
    if [[ $SUDO_PASS != $SUDO_PASS_2 ]]; then
      echo "ERROR: SUDOパスワードと再入力パスワードが一致しません。"
      ask_sudo_password
    fi
  fi
}

ask_admin_conf(){
  echo -en "\e[33mKAMONOHASHIのadminパスワード(8文字以上): \e[m"; read -s PASSWORD
  echo "" # read -sは改行しないので改行
  echo -en "\e[33m確認のためadminパスワードをもう一度入力してください: \e[m"; read -s PASSWORD_2
  echo "" # read -sは改行しないので改行

  if [[ $PASSWORD != $PASSWORD_2 ]]; then
    echo "ERROR: KAMONOHASHIのadminパスワードと再入力パスワードが一致しません。"
    ask_admin_conf
  elif [ ${#PASSWORD} -lt 8 ]; then
    echo "ERROR: KAMONOHASHIのadminパスワードは8文字以上を入力してください。"
    ask_admin_conf
  elif expr $PASSWORD : "[0-9]*" > /dev/null; then
    echo "ERROR: KAMONOHASHIのadminパスワードは数字のみの設定はできません。"
    ask_admin_conf
  fi
}

ask_proxy_conf(){
  echo -en "\e[33mプロキシを設定しますか？ [y/N]: \e[m"; read PROXY_FLAG
  case $PROXY_FLAG in
   [Yy]* ) 
     echo -en "\e[33mhttp_proxy: \e[m"; read HTTP_PROXY
     echo -en "\e[33mhttps_proxy: \e[m"; read HTTPS_PROXY

     echo -en "\e[33m no_proxy が自動生成されます。proxy除外対象を追記しますか？ [y/N]: \e[m"; read NO_PROXY_FLG
     case $NO_PROXY_FLG in
       [Yy]* ) 
           echo -en "\e[33m追記するproxy除外対象: \e[m"; read ADDITIONAL_NO_PROXY
       ;;
       *) 
       ;;
     esac
     ;;   
   *) ;;
  esac
}

ask_conf(){
  echo "クラスタの構成情報を入力してください"
  echo "詳細は ${HELP_URL} を参照してください"

  ask_node_conf
  ask_ssh_conf
  ask_proxy_conf
  ask_admin_conf
}

# インベントリを引数で扱うのは困難なので、ファイル生成する
generate_ansible_inventory(){

  GPU_NODES=$(echo -e "${GPU_NODES_COMMA//,/\\n}")

  KUBE_MASTER=$KUBE_MASTER \
  ETCD=$KUBE_MASTER \
  KQI_NODE=$KQI_NODE \
  OBJECT_STORAGE=$STORAGE \
  NFS=$STORAGE \
  GPU_NODES=$GPU_NODES \
  SSH_USER=$SSH_USER \
  envsubst < infra/conf-template/inventory > infra/conf/inventory
}

append_proxy_ansible_vars(){
  # 改行の挿入
  echo "" >> infra/conf/vars.yml
  if [ ! -z "$HTTP_PROXY" ]; then
    echo "http_proxy: $HTTP_PROXY" >> infra/conf/vars.yml
  fi
  if [ ! -z "$HTTPS_PROXY" ]; then
    echo "https_proxy: $HTTPS_PROXY" >> infra/conf/vars.yml
  fi
  # ansible側でno_proxyは組み立てる
  if [ ! -z "$ADDITIONAL_NO_PROXY" ]; then
    echo "additional_no_proxy: $ADDITIONAL_NO_PROXY" >> infra/conf/vars.yml
  fi
}

generate_ansible_conf(){
  generate_ansible_inventory
  cp infra/conf-template/vars.yml infra/conf/vars.yml
  append_proxy_ansible_vars
}

get_ip(){
  local IP_BY_DNS=$(host $1 | awk '/has address/ { print $4 }')
  if [ -z "$IP_BY_DNS" ]; then
    # 名前解決した結果からループバックを除き先頭のIPを選択
    # hostコマンドはhostsを見ない。getentは/etc/nsswitch.confに従って解決する
    local IP=$(getent hosts $1 | awk '{print $1}' | grep -v ^127.* | head -1)
  else
    local IP=$IP_BY_DNS
  fi
  echo $IP
}

append_proxy_helm_conf(){
  # 改行の挿入
  echo "" >> kamonohashi/conf/settings.yml
  if [ ! -z "$HTTP_PROXY" ]; then
    echo "http_proxy: '$HTTP_PROXY'" >> kamonohashi/conf/settings.yml
  fi
  if [ ! -z "$HTTPS_PROXY" ]; then
    echo "https_proxy: '$HTTPS_PROXY'" >> kamonohashi/conf/settings.yml
  fi  
  if [ ! -z "$HTTP_PROXY" ] || [ ! -z "$HTTPS_PROXY" ]; then
    # kamonohashiコンテナに設定するNO_PROXYの生成
    KUBE_MASTER_IP=$(get_ip $KUBE_MASTER)
    STORAGE_IP=$(get_ip $STORAGE)
    NO_PROXY=$KUBE_MASTER,$KUBE_MASTER_IP,$STORAGE,$STORAGE_IP,localhost,127.0.0.1,.local
    if [ ! -z "$ADDITIONAL_NO_PROXY" ]; then
      NO_PROXY=$NO_PROXY,$ADDITIONAL_NO_PROXY
    fi
    echo "no_proxy: '$NO_PROXY'" >> kamonohashi/conf/settings.yml
  fi
}

generate_helm_conf(){
  INGRESS_NODE_IP=$(get_ip $KQI_NODE) 
  mkdir -p kamonohashi/conf/ 

  KQI_NODE=$KQI_NODE \
  INGRESS_NODE=$KQI_NODE \
  INGRESS_NODE_IP=$INGRESS_NODE_IP \
  VIRTUAL_HOST=$KQI_NODE \
  NODES=${GPU_NODES_COMMA} \
  OBJECT_STORAGE=$STORAGE \
  OBJECT_STORAGE_PORT=9000 \
  OBJECT_STORAGE_ACCESSKEY=admin \
  NFS_STORAGE=$STORAGE \
  NFS_PATH=/var/lib/kamonohashi/nfs \
  envsubst < kamonohashi/conf-template/settings.yml > kamonohashi/conf/settings.yml
    
  append_proxy_helm_conf
}

generate_conf(){
  generate_ansible_conf
  generate_helm_conf
}

wait_helm_ready(){
  NEXT_WAIT_TIME=0
  until helm list 2>/dev/null || [ $NEXT_WAIT_TIME -eq 40 ]; do
     NEXT_WAIT_TIME=$(expr $NEXT_WAIT_TIME + 1)
     sleep 5
  done 
  if [ $NEXT_WAIT_TIME -eq 40 ]; then
    echo "ERROR: Helm Tiller Serverへ接続できませんでした"
    exit 1
  fi
}

deploy(){ 
  show_logo
  show_step "STEP[1/5] クラスタ設定の入力"
  
  if [ -e conf/settings ]; then
    echo -e "\e[33m過去の設定ファイルが残っています。\e[m";
    echo -en "\e[33mクラスタを削除して再作成しますか？ [y/n]: \e[m"; read answer
     case $answer in
       [Yy]* )
         # インフラの削除 エラーが起きても「|| /bin/true」によりset -eで終了しない
         infra/deploy-kqi-infra.sh clean || /bin/true ;;
       *) echo -e "\e[33m終了します\e[m" &&  exit 0 ;;
     esac  
  fi
  
  ask_conf
  generate_conf

  export http_proxy=$HTTP_PROXY
  export https_proxy=$HTTPS_PROXY
  export no_proxy=$NO_PROXY

  show_step "STEP[2/5] インフラ部分構築用ツールのインストール"
  infra/deploy-kqi-infra.sh prepare
  
  show_step "STEP[3/5] インフラ部分構築(20分ほどお待ちください)"
  PASSWORD=$PASSWORD SSH_PASS="$SSH_PASS" SUDO_PASS="$SUDO_PASS" infra/deploy-kqi-infra.sh deploy
  
  show_step "STEP[4/5] KAMONOHASHI構築用ツールのインストール"
  kamonohashi/deploy-kqi.sh prepare
  wait_helm_ready

  show_step "STEP[5/5] KAMONOHASHI構築"
  PASSWORD=$PASSWORD DB_PASSWORD=$PASSWORD STORAGE_PASSWORD=$PASSWORD kamonohashi/deploy-kqi.sh credentials
  kamonohashi/deploy-kqi.sh deploy
  
  show_step "構築完了"
  echo -e "\e[33m http://${KQI_NODE} にブラウザでアクセスしてください\e[m"
}

clean(){
  infra/deploy-kqi-infra.sh clean
}

show_help(){
  "available args: deploy, clean, help"
}

main(){
  set -e
  case $1 in
    deploy) deploy ;;
    clean) clean ;;
    help) show_help ;;
    *) show_help ;;
  esac
}

# このスクリプトをどこから実行しても大丈夫なように
# main実行時のみSCRIPT_DIRに移動
(cd $SCRIPT_DIR && main $1)
