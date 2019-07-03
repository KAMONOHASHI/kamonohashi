using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure.Types
{
    public enum RegistryServiceType
    {
        None = 0,
        DockerHub = 1,
        GitLab = 2,
        PrivateDockerRegistry = 3,
        NvidiaGPUCloud = 4
    }
}
