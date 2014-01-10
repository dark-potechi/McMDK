using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK
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

        public static readonly string Version = "2.0.0";

        public static readonly int Release = 25;

        public static readonly string PluginDirectory = "plugins";

        public static readonly string ProjectDirectory = "projects";

        public static readonly string ProtectPass = "Mi8dEppKhXck95rgmNfyc3AXd";

        public static readonly string NewVersionUrl = "http://tuyapin.net/mcmdk/application/version.xml";

        public static readonly string MinecraftForgeUrl = "http://files.minecraftforge.net/minecraftforge/minecraftforge-src-{0}-{1}.zip";

        public static readonly string ForgeGradleUrl = "http://files.minecraftforge.net/maven/net/minecraftforge/forge/{0}-{1}/forge-{0}-{1}-src.zip";

        public static readonly string CoderPackUrl = "http://mcp.ocean-labs.de/files/archive/mcp{0}.zip";

        public static readonly string MinecraftLoginUrl = "https://login.minecraft.net/?user={0}&password={1}&version=14";


    }
}
