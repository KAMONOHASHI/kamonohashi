using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Nssol.Platypus
{
    /// <summary>
    /// エントリポイントをもつクラス
    /// </summary>
    public class Program
    {
        /// <summary>
        /// プログラムのエントリポイント
        /// </summary>
        /// <param name="args">実行時引数</param>
        public static void Main(string[] args)
        {
            // https://github.com/aspnet/EntityFrameworkCore/issues/9033
            // var host = BuildWebHost(args);
            var host = BuildWebHost2(args);

            ConfigureLog4Net(@"./log4net.config");

            host.Run();
        }

        private static IWebHost BuildWebHost2(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .UseStartup<Startup>()
                .UseUrls("http://*:5000")
                .Build();

        /// <summary>
        /// log4net の設定ファイルを読み込み、環境変数を置換する
        /// ※本来log4netの機能だけでできるはず。.netcore の問題か動かないので自作した。
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <returns>読み込み結果</returns>
        private static Stream ConvertLogConfigEnvVariables(string filename)
        {
            using (Stream fs = new FileStream(filename, FileMode.Open))
            using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
            {
                string content = reader.ReadToEnd();
                Regex r = new Regex(@"\$\{\s*(.*)\s*\}");

                StringBuilder buf = new StringBuilder();
                int lastIndex = 0;
                MatchCollection mc = r.Matches(content);
                foreach (Match m in mc)
                {
                    buf.Append(content.Substring(lastIndex, m.Index - lastIndex));
                    string envName = m.Groups[1].Value;
                    string envValue = Environment.GetEnvironmentVariable(envName);
                    if (!string.IsNullOrEmpty(envValue))
                    {
                        buf.Append(envValue);
                    }
                    lastIndex = m.Index + m.Length;
                }
                buf.Append(content.Substring(lastIndex));
                return new MemoryStream(Encoding.UTF8.GetBytes(buf.ToString()));
            }
        }

        /// <summary>
        /// Log4Netを構成します。
        /// </summary>
        /// <param name="log4netFileName">log4netconfigファイルパス</param>
        private static void ConfigureLog4Net(string log4netFileName)
        {
            var assembly = typeof(LogManager).GetTypeInfo().Assembly;
            var repository = LogManager.GetRepository(assembly);
            XmlConfigurator.Configure(repository, ConvertLogConfigEnvVariables(log4netFileName));
        }

    }
}
