using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK.Plugin
{
    public interface IPlugin
    {
        /// <summary>
        /// プラグインの名前を取得します。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// プラグインのバージョンを取得します。
        /// </summary>
        string Version { get; }

        /// <summary>
        /// プラグインロード時に呼び出されます。
        /// </summary>
        void Initialize();

    }
}
