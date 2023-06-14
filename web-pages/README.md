# web-pages

## 開発環境整備方法

1. node のインストール v16.19

- [node 公式ページ](https://nodejs.org/ja/download/)からインストーラをダウンロードし、インストールする。

2. VSCode のセットアップ

- [Visual Studio Code](https://azure.microsoft.com/ja-jp/products/visual-studio-code/)から VS Code をインストールする。
- 下記プラグインをインストールする。
  - Vetur
  - ESLint
  - Prettier - Code formatter

3. 下記の内容を記載した`.env.local`を web-pages ディレクトリ配下に配置(KAMONOHASHI ホストの情報を記載。tensorboard, notebook, ポート開放時の URL に利用。このファイルを配置しない場合は、アクセス先が localhost となり、これらの機能をテストできない)

```
VUE_APP_KAMONOHASHI_HOST='KAMONOHASHI-hostname'
```

4. web-pages ディレクトリをルートディレクトリとして、vscode で開く(ルートディレクトリで開かないと、.vscode 内の設定が読み込まれないため)

5. `npm install`を実行

6. 不要な所に改行してみた状態で上書き保存すると、改行が削除されることを確認(Prettier が効いているかを確かめる)

7. サーバー側を起動

8. `npm run serve`でフロント開発用サーバを起動

9. http://localhost:8080 にアクセスできることを確認

10. (任意)Chrome でのデバッグを容易にするため Vue.js devtools をインストールする

## API に変更が加わった場合

1. swagger の画面から`swagger.json`を取得し、モノリポジトリ直下に配置

2. `npm run codegen`を実行(javaがinstallされていること)

3. `src/codegen/api.ts`が自動生成される

4. `src/codegen/api.ts`を`src/codegen/api.generate.ts`にリネームする

5. `src/api`にsrc/codegen/api.generate.ts`を配置する

6. API を追加する場合は`src/api/api.ts`を編集する
