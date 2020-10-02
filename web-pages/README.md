# web-pages

## 開発環境整備方法
1. nodeのインストール v12.16
- [node公式ページ](https://nodejs.org/ja/download/)からインストーラをダウンロードし、インストールする。

2. VSCodeのセットアップ
- [Visual Studio Code](https://azure.microsoft.com/ja-jp/products/visual-studio-code/)からVS Codeをインストールする。
- 下記プラグインをインストールする。
  - Vetur
  - ESLint
  - Prettier - Code formatter

3. 下記の内容を記載した`.env.local`をweb-pagesディレクトリ配下に配置(KAMONOHASHIホストの情報を記載。tensorboard, notebook, ポート開放時のURLに利用。このファイルを配置しない場合は、アクセス先がlocalhostとなり、これらの機能をテストできない)
```
VUE_APP_KAMONOHASHI_HOST='KAMONOHASHI-hostname'
```

4. web-pagesディレクトリをルートディレクトリとして、vscodeで開く(ルートディレクトリで開かないと、.vscode内の設定が読み込まれないため)

5. `npm install`を実行

6. 不要な所に改行してみた状態で上書き保存すると、改行が削除されることを確認(Prettierが効いているかを確かめる)

7. サーバー側を起動

8. `npm run serve`でフロント開発用サーバを起動

9. http://localhost:8080 にアクセスできることを確認

10. (任意)Chromeでのデバッグを容易にするためVue.js devtoolsをインストールする
