using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using McMDK.Utils;

namespace McMDK.Data
{
    public class Minecraft
    {
        public static List<string> MinecraftVersions = new List<string>();

        public static Dictionary<string, List<string>> ForgeVersions = new Dictionary<string, List<string>>();

        public static Dictionary<string, string> MCPVersions = new Dictionary<string, string>();

        public static Dictionary<int, Dictionary<string, string>> MinecraftSource = new Dictionary<int, Dictionary<string, string>>();

        public static void Load()
        {
            try
            {
                Uri uri = new Uri(Define.MinecraftVersionUrl);
                WebClient client = new WebClient();
                Minecraft.ParseMCVErsions(client.DownloadString(uri));
            }
            catch (Exception e) 
            {
                Define.GetLogger().Error("Download failed.", e);
            }
        }

        private static void ParseMCVErsions(string r)
        {
            if(r != null)
            {
                XElement element = XElement.Parse(r);
                var q = from p in element.Element("Minecraft").Elements("Version")
                        select new
                        {
                            Version = p.Value,
                            MCPVersion = (string)p.Attribute("mcp")
                        };
                foreach(var item in q)
                {
                    Minecraft.MinecraftVersions.Add(item.Version);
                    Minecraft.MCPVersions.Add(item.Version, item.MCPVersion);
                }

                foreach(string v in Minecraft.MinecraftVersions)
                {
                    try
                    {
                        Uri uri = new Uri(String.Format(Define.ForgeVersionsUrl, v).Replace(" ", "_"));
                        WebClient client = new WebClient();
                        Console.WriteLine(uri);
                        Minecraft.ParseForgeVersions(client.DownloadString(uri), v);
                    }
                    catch (Exception e)
                    {
                        Define.GetLogger().Error("Download failed.", e);
                    }
                }
            }
            else
            {
                Define.GetLogger().Error("Download failed.");
            }
        }

        private static void ParseForgeVersions(String r, String v)
        {
            if(r != null)
            {
                List<string> list = new List<string>();
                XElement element = XElement.Parse(r);
                var q = from p in element.Elements("MinecraftForge").Elements("Version")
                        select new
                        {
                            Version = p.Value
                        };
                foreach(var item in q)
                {
                    list.Add(item.Version);
                }
                Minecraft.ForgeVersions.Add(v, list);
            } 
            else
            {
                Define.GetLogger().Error("Download failed");
            }
        }
    }
}
