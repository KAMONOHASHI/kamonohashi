using Microsoft.Extensions.Options;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Logic.Interfaces;
using System;
using System.Reflection;


namespace Nssol.Platypus.Logic
{
    public class VersionLogic : PlatypusLogicBase, IVersionLogic
    {
        private readonly DeployOptions deployOptions;
        public VersionLogic(ICommonDiLogic commonDiLogic, IOptions<DeployOptions> deployOptions) : base(commonDiLogic)
        {
            this.deployOptions = deployOptions.Value;
        }
        /// <summary>
        /// バージョン番号を取得する
        /// </summary>
        public string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            AssemblyInformationalVersionAttribute assemblyInformationalVersion =
                (AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute));

            // InformationalVersionはリリース時にコンパイルオプションで埋め込む。
            string assemblyVersion = assemblyInformationalVersion.InformationalVersion;

            // 開発時はversionをdevelopとする
            string version = deployOptions.Mode == "develop" ? "develop" : assemblyVersion;

            return version;
        }
    }
}
