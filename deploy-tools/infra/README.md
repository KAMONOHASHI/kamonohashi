# 使用手順
* 設定ファイルを編集する(後述)
* 構築ツールのインストール: `./deploy-kqi-infra.sh prepare`の実行
* 構築実施: `./deploy-kqi-infra.sh deploy`の実行
* テスト: `./deploy-kqi-infra.sh test`の実行

# 設定方法
* conf-templateディレクトリのファイルを次のコマンドでコピーする
* cp -i conf-template/* conf/ 
## 編集方法
conf-template/sample/のサンプルを参照

### conf/inventory
* \[kube-master\]: kubernetesのmasterをインストールするホスト名を記載する
* \[etcd\]: kubernetesのmasterをインストールするホスト名を記載する
* \[gpu-node\]: gpuサーバーのホスト名を記載する
* \[kqi-node\]: kamonohashiのWeb UI用サーバーのホスト名を記載する
* \[cpu-node\]: GPU以外の機械学習用に使用するサーバのホスト名を記載する。GPUのみの場合空でよい。
* \[object-storage\]: minioを自動構築する場合はホスト名を記載。別途用意する場合は空でよい。
* \[nfs-server\]: nfs-serverを自動構築する場合はホスト名を記載。別途用意する場合は空でよい。
* \[all:vars\]: 
  * ansible_ssh_user: ansibleによる構築で使用するユーザー名
  * ansible_ssh_pass: 上記sshユーザーで使用するパスワード。キーを使用する場合はコメントアウトする
  * ansible_sudo_pass: 上記sshユーザーで使用するsudoパスワード。パスワード不要ならコメントアウトする
### conf/vars.yml
* http_proxy, https_proxy, no_proxy: プロキシ環境の場合は設定する。no_proxyには以下を含める
  * kube-master, minio, オンプレミスのdocker-registry, 
  * .localサフィックス
  * kube-masterのIP
  * minioのIP(DNSがない場合)
  * localhost,127.0.0.1
* docker_insecure_registries: オンプレミスのdocker-registryを使用する場合設定する
* kube_pods_subnet, kube_service_addresses, docker_bip: k8s内部セグメントを変更する場合は記載する。デフォルト値がオンプレミスと衝突する場合に変更する
* その他使用可能な設定は[kubespray](https://github.com/kubernetes-sigs/kubespray)を参照

### conf/test-vars.yml
* insecure registryのテストを行う場合はコメントアウトを外して記載する

# その他
* アップグレード
  * デプロイツールではインプレースアップグレードのみ想定しています
    * k8sのアンインストールとインストールを行います
    * ansibleのため、デプロイツールを介さずに行った設定ファイル編集は削除されます
    * LAMONOHASHIのアプリで利用したデータは引き続き利用できます
* GPUノード追加: inventoryにGPUを追加して`./deploy-kqi-infra.sh scale`の実行