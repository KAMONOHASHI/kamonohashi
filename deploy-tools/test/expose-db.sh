#!/bin/bash

KQI_NODE=$1

get_ip(){
  local IP_BY_DNS=$(host $1 | awk '/has address/ { print $4 }')
  if [ -z "$IP_BY_DNS" ]; then
    # 名前解決した結果からループバックを除き先頭のIPを選択
    # hostコマンドはhostsを見ない。getentは/etc/nsswitch.confに従って解決する
    local IP=$(getent hosts $1 | awk '{print $1}' | grep -v 127* | head -1)
  else
    local IP=$IP_BY_DNS
  fi
  echo $IP
}

NODE_IP=$(get_ip $1)



cat<<EOF | kubectl apply -f -
kind: Service
apiVersion: v1
metadata:
  name: postgres-expose
  namespace: kqi-system
spec:
  selector:
    app: postgres
  ports:
  - port: 5432
    targetPort: 5432
  externalIPs: ["$NODE_IP"]

EOF