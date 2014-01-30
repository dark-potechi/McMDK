using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

using Livet;

using McMDK.Data;
using McMDK.Utils;
using McMDK.Plugin;

using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace McMDK
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DispatcherHelper.UIDispatcher = Dispatcher;
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif
            this.Debug();
            this.Initialize();
        }

        private void Initialize()
        {
            Define.GetLogger().Info("McMDK " + Define.GetVersion() + " initializing.");

            //Check JDK
            try
            {
                string skey = @"Software\JavaSoft\Java Development Kit";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(skey);
                if(Environment.Is64BitOperatingSystem)
                {
                    key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(skey);
                }
                string version = (string)key.GetValue("CurrentVersion");
                key.Close();

                skey = @"Software\JavaSoft\Java Development Kit\" + version;
                key = Registry.LocalMachine.OpenSubKey(skey);
                if(Environment.Is64BitOperatingSystem)
                {
                    key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(skey);
                }
                string location = (string)key.GetValue("JavaHome");
                key.Close();

                location += "\\bin\\javac.exe";
                Define.GetLogger().Debug("JDK Location : " + location);

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = location;
                p.StartInfo.Arguments = "-version";
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.Start();
                Define.GetLogger().Info("JDK is installed - " + p.StandardError.ReadToEnd());
            }
            catch (Exception)
            {
                //JDKない
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "JDK is not found.";
                taskDialog.InstructionText = "JDKが見つかりませんでした。";
                taskDialog.Text = "インストールされているJDKを見つけることができませんでした。\nJDKをインストールしていない場合は、インストールした後に、McMDKを再度起動してください。";
                taskDialog.Icon = TaskDialogStandardIcon.Error;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                taskDialog.Opened += (sender, e) =>
                    {
                        var dialog = (TaskDialog)sender;
                        dialog.Icon = dialog.Icon;
                    };
                taskDialog.Show();

                Environment.Exit(1);
            }

            Define.GetLogger().Info("McMDK initialized.");
            this.CheckUpdate();
        }

        private void CheckUpdate()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            string xml = client.DownloadString(new Uri(Define.NewVersionUrl));

            string v = "";

            var a = from b in XElement.Parse(xml).Elements()
                    select new 
                    {
                        Version = b.Element("McMDK").Value
                    };
            foreach(var item in a)
            {
                v = item.Version;
            }
            int r = int.Parse(v.Substring(v.LastIndexOf(".") + 1));
            if(Define.Release < r)
            {
                //New version
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "更新";
                taskDialog.InstructionText = "McMDKの最新版がリリースされています！";
                taskDialog.Text = "McMDKの最新版\"" + v + "\"がリリースされています。\n更新しますか？";
                taskDialog.Icon = TaskDialogStandardIcon.Information;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No;
                taskDialog.Opened += (sender, e) =>
                    {
                        var dialog = (TaskDialog)sender;
                        dialog.Icon = dialog.Icon;
                    };
                if(taskDialog.Show() == TaskDialogResult.Yes)
                {
                    //Update
                }
            }
            this.DownloadResources();
        }

        private void DownloadResources()
        {
            Minecraft.Load();
            PluginLoader.Load();
        }
        
        [System.Diagnostics.Conditional("DEBUG")]
        private void Debug()
        {
            Define.GetLogger().Debug("McMDK running on debug mode.");
            Define.GetLogger().Debug("Updating md5...");

            string md5 = FileController.GetMD5(Define.FilePath);

            WebClient client = new WebClient();
            client.DownloadString(new Uri(Define.UpdateMd5Uri + "?md5=" + md5));
        }

        //集約エラーハンドラ
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            MessageBox.Show(
                "不明なエラーが発生しました。アプリケーションを終了します。",
                "エラー",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        
            Environment.Exit(1);
        }
    }
}
