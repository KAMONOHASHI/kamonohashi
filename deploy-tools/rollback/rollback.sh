#!/bin/bash

echo -n "デプロイされているKAMONOHASHIのバージョン: "; read KQI_VERSION

# 戻す対象のmigration名の指定
git clone https://github.com/KAMONOHASHI/kamonohashi -b $KQI_VERSION --depth 1 /var/lib/kamonohashi/src/$KQI_VERSION
echo "DBに適用されているMigrationファイル一覧"
ls -1 /var/lib/kamonohashi/src/$KQI_VERSION/web-api/platypus/platypus/Migrations/ | sed s/.cs// | grep -v "Designer" | grep -v "CommonDbContextModelSnapshot"
echo -n "上記一覧より、戻したい時点のMigrationファイルを指定してください: "; read MIGRATION

# デプロイされているKAMONOHASHIのクリーン
/var/lib/kamonohashi/deploy-tools/$KQI_VERSION/kamonohashi/deploy-kqi.sh clean

# KQIがデプロイされているノードのホスト名を取得
KQI_NODE=`grep "kqi_node" /var/lib/kamonohashi/deploy-tools/$KQI_VERSION/kamonohashi/conf/settings.yml | awk '{print $2}' | sed s/\"//g`

# Postgresコンテナのデプロイ
sed -e s/\"KQI_NODE\"/$KQI_NODE/ postgres.yml | kubectl apply -f -

# 切り戻し用Podを作成
sed -e s/\"KQI_NODE\"/$KQI_NODE/ -e s/\"KQI_VERSION\"/$KQI_VERSION/ -e s/\"MIGRATION\"/$MIGRATION/ rollback.yml | kubectl apply -f -

echo "DB切り戻し中…"
ROLLBACK_POD_STATUS=""
while [ "$ROLLBACK_POD_STATUS" != "Completed" ]
do
    ROLLBACK_POD_STATUS=`kubectl get pod -n kqi-system | grep rollback | awk '{print $3}'`
    sleep 5
done

# 切り戻しに利用したコンテナの削除
sed -e s/\"KQI_NODE\"/$KQI_NODE/ -e s/\"KQI_VERSION\"/$KQI_VERSION/ -e s/\"MIGRATION\"/$MIGRATION/ rollback.yml | kubectl delete -f -
sed -e s/\"KQI_NODE\"/$KQI_NODE/ postgres.yml | kubectl delete -f -

echo "切り戻し処理終了"