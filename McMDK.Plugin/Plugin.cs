using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Utils.Log;

using McMDK.Plugin.Gui;
using McMDK.Plugin.Gui.Controls;

namespace McMDK.Plugin
{
    public class Plugin : IPlugin
    {
        public string Name { set; get; }

        public string Version { set; get; }

        public string Author { set; get; }

        public string PluginID { set; get; }

        public string Dependents { set; get; }

        public string Support { set; get; }

        public Logger Logger { set; get; }

        public List<UIControl> Controls { set; get; }

        public void Initialize()
        {
            this.Controls = new List<UIControl>();
        }
    }
}
