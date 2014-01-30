using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK.Data;
using McMDK.Models;
using McMDK.Views;
using McMDK.MCP;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace McMDK.ViewModels
{
    public class NewProjectWindowViewModel : ViewModel, IDataErrorInfo
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

        public NewProjectWindow View;

        private List<string> _MinecraftVersions = new List<string>();
        private List<string> _MinecraftForgeVers = new List<string>();

        public void Initialize()
        {
            this.ProjectName = "";
            this.ProjectName = null;
            this.MinecraftVersion = "";
            this.MinecraftVersion = null;
            this.MinecraftForgeVer = "";
            this.MinecraftForgeVer = null;
            this.MinecraftVersions = Minecraft.MinecraftVersions;
        }

        public void Clear()
        {
            this.ProjectName = "";
            this.ProjectName = null;
            this.MinecraftVersion = null;
            this.MinecraftForgeVer = null;
        }


        #region [Create]Button

        private ViewModelCommand _CreateButtonCommand;
        public ViewModelCommand CreateButtonCommand
        {
            get
            {
                if (_CreateButtonCommand == null)
                {
                    _CreateButtonCommand = new ViewModelCommand(CreateProject);
                }
                return _CreateButtonCommand;
            }
        }

        public void CreateProject()
        {
            //Create a new project;
            if(String.IsNullOrEmpty(this._MinecraftForgeVer) || String.IsNullOrEmpty(this._MinecraftVersion) || String.IsNullOrEmpty(this._ProjectName))
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "入力エラー";
                taskDialog.InstructionText = "値が入力されていません。";
                taskDialog.Text = "値が十分ではありません。\nすべての値を入力する必要があります。";
                taskDialog.Icon = TaskDialogStandardIcon.Error;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                taskDialog.Opened += (sender, e) =>
                {
                    var dialog = (TaskDialog)sender;
                    dialog.Icon = dialog.Icon;
                };
                taskDialog.Show();
                return;
            }
            Project project = new Project
            {
                Mod = new List<Mod>(),
                Name = this.ProjectName,
                MCVersion = this.MinecraftVersion,
                ForgeVersion = this.MinecraftForgeVer,
                MCPVersion = Minecraft.MCPVersions[this.MinecraftVersion]
            };


            Setup setup = new Setup(project);
            //setup.SetProgressWindow(this.View);
            if(this.MinecraftVersion.Contains("Gradle"))
            {
                setup.SetupForgeGradle();
            }
            else
            {
                setup.SetupForge();
            }
        }

        #endregion


        #region [Cancel]Button

        private ViewModelCommand _CancelButtonCommand;
        public ViewModelCommand CancelButtonCommand
        {
            get
            {
                if (_CancelButtonCommand == null)
                {
                    _CancelButtonCommand = new ViewModelCommand(CancelButton);
                }
                return _CancelButtonCommand;
            }
        }

        public void CancelButton()
        {
            View.Dismiss();
        }

        #endregion


        #region IDataError

        public Dictionary<string, string> _errors = new Dictionary<string, string>();

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string name]
        {
            get
            {
                if (_errors.ContainsKey(name))
                {
                    return _errors[name];
                }
                return null;
            }
        }

        #endregion


        #region GenMcModInfo
        private bool _GenMcModInfo;
        public bool GenMcModInfo
        {
            get
            {
                return _GenMcModInfo; 
            }
            set
            {
                _GenMcModInfo = value;
            }
        }
        #endregion


        #region MinecraftVersionsプロパティ

        public List<string> MinecraftVersions
        {
            get
            {
                return _MinecraftVersions;
            }
            set
            {
                _MinecraftVersions = value;
                RaisePropertyChanged("MinecraftVersions");
            }
        }

        #endregion


        #region MinecraftForgeVersプロパティ

        public List<string> MinecraftForgeVers
        {
            get
            {
                return _MinecraftForgeVers;
            }
            set
            {
                _MinecraftForgeVers = value;
                RaisePropertyChanged("MinecraftForgeVers");
            }
        }

        #endregion


        #region Messageプロパティ

        private string _Message;
        public string Message
        {
            get
            {
                return this._Message;
            }
            set
            {
                _Message = value;
                RaisePropertyChanged("Message");
            }
        }

        #endregion


        #region ProjectName変更通知プロパティ

        private string _ProjectName;
        public string ProjectName
        {
            get
            {
                return this._ProjectName;
            }
            set
            {
                if(EqualityComparer<string>.Default.Equals(this._ProjectName, value))
                {
                    return;
                }
                this._ProjectName = value;
                RaisePropertyChanged("ProjectName");

                if(string.IsNullOrEmpty(this._ProjectName))
                {
                    _errors["ProjectName"] = "プロジェクト名を入力してください。";
                }
                else
                {
                    _errors["ProjectName"] = null;
                }
            }
        }

        #endregion


        #region MinecraftVersion変更通知プロパティ

        private string _MinecraftVersion;
        public string MinecraftVersion
        {
            get
            {
                return this._MinecraftVersion;
            }
            set
            {
                if (EqualityComparer<string>.Default.Equals(this._MinecraftVersion, value))
                {
                    return;
                }
                this._MinecraftVersion = value;
                RaisePropertyChanged("MinecraftVersion");

                if (string.IsNullOrEmpty(this._MinecraftVersion))
                {
                    _errors["MinecraftVersion"] = "Minecraftのバージョンを選択してください。";
                }
                else
                {
                    _errors["MinecraftVersion"] = null;
                    //Forge切り替え
                    MinecraftForgeVers = Minecraft.ForgeVersions[value];
                    MinecraftForgeVer = "";

                    if (this._MinecraftVersion.Contains("Gradle"))
                    {
                        this.Message = "※GradleはMinecraft Forge #953以降で使用されているビルドツールです。";
                    }
                    else
                    {
                        this.Message = "";
                    }
                }
            }
        }

        #endregion


        #region MinecraftForgeVer変更通知プロパティ

        private string _MinecraftForgeVer;
        public string MinecraftForgeVer
        {
            get
            {
                return this._MinecraftForgeVer;
            }
            set
            {
                if (EqualityComparer<string>.Default.Equals(this._MinecraftForgeVer, value))
                {
                    return;
                }
                this._MinecraftForgeVer = value;
                RaisePropertyChanged("MinecraftForgeVer");

                if (string.IsNullOrEmpty(this._MinecraftForgeVer))
                {
                    _errors["MinecraftForgeVer"] = "Minecraft Forgeのバージョンを選択してください。";
                }
                else
                {
                    _errors["MinecraftForgeVer"] = null;
                }
            }
        }

        #endregion
    }
}
