#!/bin/bash

readonly KUBESPLAY_VERSION="v2.10.0"
readonly SCRIPT_DIR=$(cd $(dirname $0); pwd)
readonly REPO_ROOT_DIR="$SCRIPT_DIR/../.."

show_help() {
    echo "available args: prepare, deploy, clean, help, scale, remove-node" 1>&2
    exit 1
}

ask_ssh_conf(){
    echo -en "\e[33mSSHパスワード(キーを使用する場合は未入力でEnter): \e[m"; read -s SSH_PASS
    echo "" # read -sは改行しないので改行 
    echo -en "\e[33mSUDOパスワード(不要なら未入力でEnter): \e[m"; read -s SUDO_PASS
    echo "" # read -sは改行しないので改行 
}

# 設定ファイルに書き込まない内容
ask_extra_conf_if_not_defined(){
  if [ -z "$PASSWORD" ]; then
    echo -en "\e[33mAdmin Password: \e[m"; read -s PASSWORD
    echo "" # read -sは改行しないので改行
    ask_ssh_conf
  fi
}

make_extra_arg(){
  EXTRA_ARG="ansible_ssh_pass=$SSH_PASS ansible_sudo_pass=$SUDO_PASS password=$PASSWORD"  
}

install_ansible(){
    sudo -E apt-get update
    # build-essential libssl-dev libffi-dev python-devはcryptographyに必要
    # https://qiita.com/takahirono7/items/571541d40bf23cd48fdb 
    sudo -E apt-get install -y python3-pip sshpass build-essential libssl-dev libffi-dev python-dev
    # requirements.txtでの依存パッケージインストールではansible2.8.0が入ってしまうので回避する
    sudo -E pip3 install -U ansible==2.7.10 Jinja2==2.10.1 netaddr==0.7.19 pbr==5.1.3 hvac==0.8.2 
    sudo mkdir -p /var/log/kamonohashi/
    sudo touch /var/log/kamonohashi/kubesplay.log
    sudo chmod 666 /var/log/kamonohashi/kubesplay.log
}

prepare(){
    git clone -b ${KUBESPLAY_VERSION} https://github.com/kubernetes-sigs/kubespray.git
    install_ansible
}

deploy(){
    ask_extra_conf_if_not_defined
    make_extra_arg
    ansible-playbook -i conf/inventory -e "$EXTRA_ARG" -e @conf/vars.yml create-cluster.yml -b  
}

scale(){
    ask_ssh_conf
    make_extra_arg
    ansible-playbook -i conf/inventory -e "$EXTRA_ARG" -e @conf/vars.yml kubespray/scale.yml -b 
}

run_test(){
    ask_ssh_conf
    make_extra_arg
    ansible-playbook -i conf/inventory -e "$EXTRA_ARG"  -e @conf/vars.yml -e @conf/test-vars.yml test.yml -b 
}

clean(){
    ask_ssh_conf
    make_extra_arg
    ansible-playbook -i conf/inventory -e "$EXTRA_ARG"  -e @conf/vars.yml clean-cluster.yml -b 
}

remove_node(){
    ask_ssh_conf
    make_extra_arg
    ansible-playbook -i conf/inventory -e "$EXTRA_ARG"  -e @conf/vars.yml kubespray/remove-node.yml -b
}

main(){
  case $1 in
    prepare) prepare ;;
    deploy) deploy ;; 
    scale) scale ;;
    test) run_test ;;
    clean) clean ;;
    remove-node) remove_node ;;
    help) show_help ;;
    *) show_help ;;
  esac
}

# このスクリプトをどこから実行しても大丈夫なように
# main実行時のみSCRIPT_DIRに移動
(cd $SCRIPT_DIR && main $1)

