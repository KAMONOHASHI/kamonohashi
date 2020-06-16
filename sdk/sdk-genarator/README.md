# SDK/CLI開発者向けドキュメント

SDKは手でコーディングした部分と[Swagger Code Generator](https://github.com/swagger-api/swagger-codegen)で生成した部分からなる。kamonohasi/op直下のファイルは手でコーディングしている。kamonohashi/op/rest以下のファイルはジェネレータが生成したものである。

## コード生成

コードを生成するには以下の手順を実行する。実行にはjava及びpython(3.5以降)が必要である。

1. Swagger Codegen の.jarをコピー
2. コード生成に使用したいバージョンのswagger.jsonをコピー
3. generate-sdk.shを実行

outputにはswagger.jsonのコメントを削除したコードとドキュメントが生成される。output.with_commentにはswagger.jsonのコメントがそのまま反映されたコードとドキュメントが生成される。

リリースするのはoutput以下のファイルである。outputのkamonohashi以下のコードを、開発者のローカルリポジトリのkamonohashi以下と置き換える。

また、output/setup.pyには、生成されたコードが依存するパッケージのバージョン情報がある。これを開発者のローカルリポジトリにあるSDKとCLIのsetup.pyに反映させる。ただし、使用していないパッケージはSDKとCLIのsetup.pyには記述しない。例えば、output/setup.pyではpython-dateutilが依存パッケージとなっているが、これはSDKでもCLIでも未使用である。また、certifiはCLIでは未使用である。

output.with_comment以下のファイル、特にドキュメントは、sdk/cli開発で適宜参照する。リリースはしない。
