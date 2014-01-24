using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Utils.Log;

namespace McMDK.Utils
{
    public class Define
    {
        /// <summary>
        /// 実行ファイルパス
        /// </summary>
        public static string FilePath
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        public static string GetVersion()
        {
            return Version + "." + Release;
        }

        public static Logger GetLogger()
        {
            if(logger == null)
            {
                logger = new Logger("McMDK");
            }
            return logger;
        }

        static readonly string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

        static Logger logger;

        public static readonly string Version = "2.0.0";

        public static readonly int Release = 25;

        public static readonly string PluginDirectory = CurrentDirectory + "\\plugins";

        public static readonly string ProjectDirectory = CurrentDirectory + "\\projects";

        public static readonly string LogDirectory = CurrentDirectory + "\\logs";

        public static readonly string TempDirectory = CurrentDirectory + "\\temp";

        public static readonly string ProtectPass = "Mi8dEppKhXck95rgmNfyc3AXd";

        public static readonly string NewVersionUrl = "http://tuyapin.net/mcmdk/application/version.xml";

        public static readonly string ForgeVersionsUrl = "http://tuyapin.net/mcmdk/application/forge/{0}.xml";

        public static readonly string MinecraftVersionUrl = "http://tuyapin.net/mcmdk/application/minecraft.xml";

        public static readonly string MinecraftForgeUrl = "http://files.minecraftforge.net/minecraftforge/minecraftforge-src-{0}-{1}.zip";

        public static readonly string ForgeGradleUrl = "http://files.minecraftforge.net/maven/net/minecraftforge/forge/{0}-{1}/forge-{0}-{1}-src.zip";

        public static readonly string CoderPackUrl = "http://mcp.ocean-labs.de/files/archive/mcp{0}.zip";

        public static readonly string MinecraftLoginUrl = "https://login.minecraft.net/?user={0}&password={1}&version=14";

        public static readonly string UpdateMd5Uri = "http://api.tuyapin.net/mcmdk/2/updatemd5.php";

        public static readonly string MinecraftJarUrl = "http://assets.minecraft.net/{0}/minecraft.jar";

        public static readonly string MinecraftSrvJarUrl = "http://assets.minecraft.net/{0}/minecraft_server.jar";

        public static readonly string LibrariesUrl = "http://tuyapin.net/mcmdk/application/libs/";

        public static readonly string MinecraftResUrl = "http://s3.amazonaws.com/MinecraftDownload/";

        public static readonly string PatchesUrl = "http://tuyapin.net/mcmdk/application/patch/{0}.zip";
    }
}
