#!/bin/bash

TOKEN_NAME=$(kubectl get secret  -n kqi-system | grep platypus-token | awk '{print $1}')

TOKEN=$(kubectl get secret $TOKEN_NAME -n kqi-system -o yaml | grep token: | awk '{print $2}')

echo "--------------- platypus token --------------"
echo $TOKEN | base64 -d

echo ""
echo "----------------------------------------------"