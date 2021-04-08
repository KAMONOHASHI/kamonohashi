# リリース手順
## リリースするソースコードの指定
* GitHubでバージョン名リリースを作成(テストが終わるまでプレリリース)
  * 例: 1.0.1
* git fetch upstream && git merge upstream/(バージョンブランチ) を実行
  * 例: git merge upstream/1.0
## コンテナのビルド & リリース
* build-docker/build.shの実行
* docker loginでDockerHubにログイン
* build-docker/push.shの実行
## Python Packageのビルド & リリース
* build-pypi/setup.sh build の実行
* PyPiにログイン
* build-pypi/setup.sh test-upload の実行
* pip installの確認
* build-pypi/setup.sh master-upload の実行

# developリリース
* tagをつけずに`build-docker/build.sh`を実行するとdevelopタグでコンテナが作られる
