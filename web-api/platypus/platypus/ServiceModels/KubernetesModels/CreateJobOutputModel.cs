namespace Nssol.Platypus.ServiceModels.KubernetesModels
{
    public class CreateJobOutputModel
    {
        public MetadataModel Metadata { get; set; }

        public class MetadataModel
        {
            public string Name { get; set; }
        }
    }
}
