using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using McMDK.Utils;
using McMDK.Utils.Log;

namespace McMDK.Plugin
{
    public class PluginLoader
    {
        private static List<Plugin> Plugins = new List<Plugin>();

        public static void Load()
        {
            //Load plugins
            string[] plugins = FileController.LoadDirectory(Define.PluginDirectory, true);
            foreach(string plugin in plugins)
            {
                Define.GetLogger().Debug("load？ -> " + plugin);
                if(plugin.EndsWith("template"))
                {
                    continue;
                }
                if(!FileController.Exists(plugin + "\\plugin.xml"))
                {
                    //Not exist
                    continue;
                }
                //Load root
                var a = from b in XElement.Load(plugin + "\\plugin.xml").Elements()
                        select new McMDK.Plugin.Plugin
                        {
                            Name = b.Element("Name").Value,
                            PluginID = b.Element("PluginID").Value,
                            Author = b.Element("Author").Value,
                            Version = b.Element("Version").Value,
                            Dependents = b.Element("Dependents").Value,
                            Support = b.Element("Support").Value
                        };
                Plugin p = null;

                foreach(var item in a)
                {
                    p = item;
                }

                p.Logger = new Logger(p.Name);
                p.Logger.Fine("Loaded success.");

                Define.GetLogger().Fine(p.Name + " is loaded.");

                Plugins.Add(p);
            }
        }

        public static List<Plugin> GetPlugins()
        {
            return Plugins;
        }
    }
}
