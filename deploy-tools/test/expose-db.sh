#!/bin/bash

cat<<EOF | kubectl apply -f -
kind: Service
apiVersion: v1
metadata:
  name: postgres-expose
  namespace: kqi-system
spec:
  selector:
    app: postgres
  type: NodePort
  ports:
  - port: 5432
    nodePort: 30000
EOF

echo "use port 30000"