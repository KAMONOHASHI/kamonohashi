# リリース手順

## リリースするソースコードの指定

- GitHub でバージョン名リリースを作成(テストが終わるまでプレリリース)
  - 例: 1.0.1
- git fetch upstream && git merge upstream/(バージョンブランチ) を実行
  - 例: git merge upstream/1.0

## コンテナのビルド & リリース

- build-docker/build.sh の実行
- docker login で DockerHub にログイン
- build-docker/push.sh の実行

## Python Package のビルド & リリース

- build-pypi/setup.sh build の実行
- PyPi にログイン
- build-pypi/setup.sh test-upload の実行
- pip install の確認
- build-pypi/setup.sh master-upload の実行

# develop リリース

- tag をつけずに`build-docker/build.sh`を実行すると develop タグでコンテナが作られる

# Notebook Image のビルド & リリース

ノートブック機能のデフォルトイメージを作成するための Dockerfile が`build-notebook-image`に配置されている。
毎回のリリースごとには実施せず、Python のマイナーバージョンのサポート終了や、コンテナ内で利用するパッケージに破壊的な変更が入った場合等にリリースを行う。
