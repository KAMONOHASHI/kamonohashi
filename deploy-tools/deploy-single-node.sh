#!/bin/bash

readonly KUBE_VERSION=v1.15.3
readonly KQI_VERSION=1.1.5 


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

ask_password(){
  echo -en "\e[33mKAMONOHASHIのadminパスワード(8文字以上): \e[m"; read -s PASSWORD  
  echo "" # read -sは改行しないので改行 
  echo -en "\e[33m確認のためadminパスワードをもう一度入力してください: \e[m"; read -s PASSWORD_2
  echo "" # read -sは改行しないので改行 
}

ask_config(){
  readonly HOSTNAME=$(hostname)
  readonly HOST_IP=$(get_ip $HOSTNAME)
  ask_password
  ask_proxy_conf
}

check_admin_password(){
  if [ $PASSWORD != $PASSWORD_2 ]; then
    echo "ERROR: KAMONOHASHIのadminパスワードと再入力パスワードが一致しません。"
    exit 1
  elif [ ${#PASSWORD} -lt 8 ]; then
    echo "ERROR: KAMONOHASHIのadminパスワードは8文字以上を入力してください。"
    exit 1
  elif expr $PASSWORD : "[0-9]*" > /dev/null; then
    echo "ERROR: KAMONOHASHIのadminパスワードは数字のみの設定はできません。"
    exit 1
  fi
}

install_docker(){
  sudo apt-get install  -y   apt-transport-https     ca-certificates     curl     gnupg-agent     software-properties-common
  curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
  add-apt-repository    "deb [arch=amd64] https://download.docker.com/linux/ubuntu \
   $(lsb_release -cs) \
   stable"
  apt-get update && apt-get install -y docker-ce docker-ce-cli containerd.io
}

install_nvidia_docker(){
  distribution=$(. /etc/os-release;echo $ID$VERSION_ID)
  curl -s -L https://nvidia.github.io/nvidia-docker/gpgkey | sudo apt-key add -
  curl -s -L https://nvidia.github.io/nvidia-docker/$distribution/nvidia-docker.list | sudo tee /etc/apt/sources.list.d/nvidia-docker.list
  apt-get update && sudo apt-get install -y nvidia-container-runtime

  cat <<EOF > /etc/docker/daemon.json
{
    "default-runtime": "nvidia",
    "runtimes": {
        "nvidia": {
            "path": "/usr/bin/nvidia-container-runtime",
            "runtimeArgs": []
        }
    }
}
EOF

  systemctl restart docker
}

install_kubectl(){
  curl -LO https://storage.googleapis.com/kubernetes-release/release/$KUBE_VERSION/bin/linux/amd64/kubectl
  chmod +x ./kubectl
  sudo mv ./kubectl /usr/local/bin/kubectl
}

install_minikube(){
  curl -Lo minikube https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64 && chmod +x minikube
  sudo cp minikube /usr/local/bin && rm minikube
  minikube start --vm-driver none --kubernetes-version=$KUBE_VERSION
}

install_nvidia_device_plugin(){
  kubectl apply -f https://raw.githubusercontent.com/NVIDIA/k8s-device-plugin/master/nvidia-device-plugin.yml
}

install_nfs_server(){
  apt install -y nfs-kernel-server
  echo "/var/lib/kamonohashi/nfs *(rw,sync,no_subtree_check,no_root_squash)" > /etc/exports

  mkdir -p /var/lib/kamonohashi/nfs
  systemctl restart nfs-kernel-server
}

install_minio(){
  wget https://dl.min.io/server/minio/release/linux-amd64/minio
  chmod +x minio
  mv minio /bin/
  cat <<EOF > /etc/systemd/system/minio.service
[Unit]
Description=Minio NAS Gateway
Documentation=https://docs.minio.io
Wants=network-online.target
After=network-online.target
AssertFileIsExecutable=/bin/minio

[Service]
EnvironmentFile=/etc/default/minio
ExecStart=/bin/minio gateway nas /var/lib/kamonohashi/nfs
Restart=on-failure
LimitNOFILE=65536
TimeoutStopSec=infinity
SendSIGKILL=no

[Install]
WantedBy=multi-user.target
EOF

  cat <<EOF > /etc/default/minio
MINIO_ACCESS_KEY=admin
MINIO_SECRET_KEY=$PASSWORD
EOF
systemctl enable minio
systemctl start minio
}

install_nfs_client(){
  apt install -y nfs-common
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


install_kamonohashi(){
  wget -O /tmp/deploy-tools-$KQI_VERSION.tar.gz https://github.com/KAMONOHASHI/kamonohashi/releases/download/$KQI_VERSION/deploy-tools-$KQI_VERSION.tar.gz
  mkdir -p /var/lib/kamonohashi/deploy-tools/$KQI_VERSION/
  cd /var/lib/kamonohashi/deploy-tools/$KQI_VERSION/
  tar --strip=1 -xf /tmp/deploy-tools-$KQI_VERSION.tar.gz
  kamonohashi/deploy-kqi.sh prepare
  apt-get install -y socat
  
  wait_helm_ready

  PASSWORD=$PASSWORD DB_PASSWORD=$PASSWORD STORAGE_PASSWORD=$PASSWORD kamonohashi/deploy-kqi.sh credentials
  cp -r kamonohashi/conf-template/ kamonohashi/conf

  cat <<EOF > kamonohashi/conf/settings.yml 
# リバースプロキシ設定
ingress:
  controller:
    nodeSelector:
      kubernetes.io/hostname: "$HOSTNAME"
    service:
      externalIPs:
       - "$HOST_IP"
# 外部からkamonohashiのweb画面にブラウザアクセスする際のhost名
virtualHosts:
  - "$HOSTNAME"

# kamonohashiをデプロイするnode
kqi_node: "$HOSTNAME"

appsettings:
  DeployOptions__GpuNodes: "$HOSTNAME"
  DeployOptions__ObjectStorageNode: "$HOSTNAME"
  DeployOptions__ObjectStoragePort: "9000"
  DeployOptions__ObjectStorageAccessKey: "admin"
  DeployOptions__NfsStorage: "$HOSTNAME"
  DeployOptions__NfsPath: "/var/lib/kamonohashi/nfs"

EOF

  if [ ! -z "$HTTP_PROXY" ]; then
    echo "http_proxy: '$HTTP_PROXY'" >> kamonohashi/conf/settings.yml
  fi
  if [ ! -z "$HTTPS_PROXY" ]; then
    echo "https_proxy: '$HTTPS_PROXY'" >> kamonohashi/conf/settings.yml
  fi  
  if [ ! -z "$HTTP_PROXY" ] || [ ! -z "$HTTPS_PROXY" ]; then
    # kamonohashiコンテナに設定するNO_PROXYの生成
    NO_PROXY=$HOSTNAME,$HOST_IP,localhost,127.0.0.1,.local
    if [ ! -z "$ADDITIONAL_NO_PROXY" ]; then
      NO_PROXY=$NO_PROXY,$ADDITIONAL_NO_PROXY
    fi
    echo "no_proxy: '$NO_PROXY'" >> kamonohashi/conf/settings.yml
  fi

  kamonohashi/deploy-kqi.sh deploy

}

deploy(){
  ask_config
  check_admin_password

  # docker未インストール時のみ実行
  if ! which docker >/dev/null 2>&1; then
    install_docker
  fi
  # nvidia-container-runtime未インストール時のみ実行
  if ! which nvidia-container-runtime >/dev/null 2>&1;  then
    install_nvidia_docker  
  fi

  install_kubectl
  install_minikube
  install_nvidia_device_plugin
  install_nfs_server
  install_minio
  install_nfs_client
  install_kamonohashi

  echo ""
  echo "構築完了"
  echo "http://${HOSTNAME}へアクセスしてください"
}

clean(){
  minikube stop
  minikube delete
  apt remove --purge -y nfs-kernel-server nfs-common
  # 引数がclean all の場合のみdocker, nvidia-dockerもアンインストール
  if [ "$1" = "all" ]; then
    apt remove --purge -y nvidia-container-runtime docker-ce docker-ce-cli containerd.io
  fi

  systemctl stop minio
  systemctl disable minio
  rm -f /etc/systemd/minio.service
  rm -f /usr/local/bin/minio
  rm -f /usr/local/bin/kubectl
}

show_help(){
  echo "available args: deploy, clean, clean all, help"
}


set -e
case $1 in
  deploy) deploy ;;
  clean) clean $2;;
  help) show_help ;;
  *) show_help ;;
esac
