using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Plugin;

namespace McMDK.Data
{
    /// <summary>
    /// Mod内のアイテム的な
    /// </summary>
    [Serializable]
    public class Mod
    {
        public Dictionary<string, string> Properties { set; get; }

        public Plugin.Plugin Plugin { set; get; }

        public string UniqueID { set; get; }

        public override string ToString()
        {
            return this.Plugin.Name + ":" + this.UniqueID;
        }
    }
}
