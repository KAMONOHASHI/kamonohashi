# 使用手順
* 設定ファイルを編集します(後述)
* 構築ツールのインストール: `./deploy-kqi.sh prepare`の実行
* 構築実施: `./deploy-kqi.sh deploy`の実行

# 設定方法
* conf-templateディレクトリのファイルを次のコマンドでコピーします
* mkdir conf && cp -i conf-template/* conf/ 

## 編集方法
conf-template/sample/のサンプルを参照
### 必須項目
|項目|内容|例|
|:---|:---|:---|
|${INGRESS_NODE}|k8s ingress controllerの配置ノード|kqi-node1|
|${INGRESS_NODE_IP}|${INGRESS_NODE}で指定したマシンのIP|10.1.1.1|
|${VIRTUAL_HOST}|ブラウザでアクセスさせるホスト名|kamonohashi.ai|
|${KQI_NODE}|kamonohashiの各種コンテナの配置ノード|kqi-node1|

### オプション項目
settings.ymlのコメントアウトをはずして設定します

#### プロキシ設定
|項目|内容|例|
|:---|:---|:---|
|http_proxy|http_proxy。infra/conf/vars.ymlと同じにする|"http://proxy.my-corp.local"|
|https_proxy|https_proxy。infra/conf/vars.ymlと同じにする|"http://proxy.my-corp.local"|
|no|no_proxy。infra/conf/vars.ymlと同じにする|"localhost,127.0.0.1,k8s,.local"|
#### active directory(LDAP)の設定
```
appsettings:
```
の以下のプロパティを編集します

|項目|内容|例|
|:---|:---|:---|
|ActiveDirectoryOptions__Domain|LDAPドメイン|"my-org.my-corp.co.local"|
|ActiveDirectoryOptions__BaseDn|LDAP DN|"DC=my-org,DC=my-corp,DC=co,DC=local"|
|ActiveDirectoryOptions__BaseOu|LDAP OU|"\"\"" (OUなしの場合)|
|ActiveDirectoryOptions__Server|LDAP Server|"ad01"|

#### バックアップの設定
* /var/lib/kamonohashi/postgres/backup/に出力されるKAMONOHASHIのpostgres backupファイルの設定です。
* Storageのバックアップはkamonohashiでは行いませんので、Storageの/var/lib/kamonohashi/nfsを別途保護してください
```
appsettings:
```
の以下のプロパティを編集します

|項目|内容|例|
|:---|:---|:---|
|BackupPostgresTimerOptions__WeeklyTimeSchedule|曜日と時間で指定します。;区切りで複数指定できます|"Sun=01:00:00;Mon=01:00:00"|
|BackupPostgresTimerOptions__MaxNumberOfBackupFiles|バックアップの世代数です。指定数を超えるバックアップは古い順に削除されます|"1"|

### コンテナのリソースの修正
```
resources:
```
の各コンテナの以下のプロパティを編集します

|項目|内容|例|
|:---|:---|:---|
|CPU|予約するcpuリソース。k8sの指定と同じフォーマットが可能|2|
|メモリ|予約するメモリリソース。k8sの指定と同じフォーマットが可能|2Mi|
