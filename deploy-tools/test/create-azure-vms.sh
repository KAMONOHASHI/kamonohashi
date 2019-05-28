#!/bin/bash

RG=$1

az group create --name $RG --location eastus

az network vnet create \
  --name myVirtualNetwork \
  --resource-group $RG \
  --subnet-name default

createVM (){
  az vm create -g $RG \
    --name $1 \
    --image "Canonical:UbuntuServer:16.04-LTS:latest" \
    --size $2 \
    --admin-username platypus \
    --ssh-key-value ./id_rsa.pub
}

createVM k8s-master Standard_F2s_v2 &
createVM kqi Standard_F4s_v2 &
createVM gpu1 Standard_NC6 &
createVM gpu2 Standard_NC6 &
createVM storage Standard_F2s_v2 &