using Nssol.Platypus.ServiceModels.ClusterManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.KubernetesModels
{
    /// <summary>
    /// サービスを作成した時の返り値
    /// </summary>
    /// <remarks>
    /// Jsonサンプル
    /// {
    ///   "kind": "Service",
    ///   "apiVersion": "v1",
    ///   "metadata": {
    ///     "name": "tensorboard-0011-22000086-20180219201419885119",
    ///     "namespace": "kimitsu",
    ///     "selfLink": "/api/v1/namespaces/kimitsu/services/tensorboard-0011-22000086-20180219201419885119",
    ///     "uid": "c8a70f7c-1566-11e8-bd08-00155d0bfa03",
    ///     "resourceVersion": "3091305",
    ///     "creationTimestamp": "2018-02-19T11:19:43Z"
    ///   },
    ///   "spec": {
    ///     "ports": [
    ///       {
    ///         "name": "ssh",
    ///         "protocol": "TCP",
    ///         "port": 22,
    ///         "targetPort": 22,
    ///         "nodePort": 30533
    ///       },
    ///       {
    ///         "name": "tensorboard",
    ///         "protocol": "TCP",
    ///         "port": 6006,
    ///         "targetPort": 6006,
    ///         "nodePort": 30711
    ///       }
    ///     ],
    ///     "selector": { "app": "tensorboard-0011-22000086-20180219201419885119" },
    ///     "clusterIP": "10.233.52.139",
    ///     "type": "NodePort",
    ///     "sessionAffinity": "None",
    ///     "externalTrafficPolicy": "Cluster"
    ///   },
    ///   "status": { "loadBalancer": {} }
    /// }
    /// </remarks>
    public class CreateServiceOutputModel
    {
        public SpecModel Spec { get; set; }

        public class SpecModel
        {
            public List<PortMappingModel> Ports { get; set; }
        }
    }
}
