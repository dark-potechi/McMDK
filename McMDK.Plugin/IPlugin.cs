using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Utils.Log;

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
        /// プラグインの作者を取得します。
        /// </summary>
        string Author { get; }

        /// <summary>
        /// プラグインの固有IDを取得します。
        /// </summary>
        string PluginID { get; }

        /// <summary>
        /// プラグインの依存関係を取得します。
        /// </summary>
        string Dependents { get; }

        /// <summary>
        /// 未使用
        /// </summary>
        string Support { get; }

        /// <summary>
        /// ログ
        /// </summary>
        Logger Logger { get; }

        /// <summary>
        /// プラグインロード時に呼び出されます。
        /// </summary>
        void Initialize();

    }
}
