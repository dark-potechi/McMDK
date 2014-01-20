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

using McMDK.Models;
using McMDK.Utils;
using McMDK.Views;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace McMDK.ViewModels
{
    public class MainWindowViewModel : ViewModel
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

        private Model Model = new Model();

        public MainWindow View;

        //Views
        private NewProjectWindow npw = new NewProjectWindow();

        private ConfigWindow cw = new ConfigWindow();

        public void Initialize()
        {
            this.View.MainGrid.Children.Add(npw);
            this.View.MainGrid.Children.Add(cw);
        }


        #region OpenSettingsCommand

        private ViewModelCommand _OpenSettingsCommand;
        public ViewModelCommand OpenSettingsCommand
        {
            get
            {
                if(_OpenSettingsCommand == null)
                {
                    _OpenSettingsCommand = new ViewModelCommand(ShowSettings);
                }
                return _OpenSettingsCommand;
            }
        }

        public void ShowSettings()
        {
            this.cw.Show();
        }

        #endregion


        #region CreateProjectCommand

        private ViewModelCommand _CreateProjectCommand;
        public ViewModelCommand CreateProjectCommand
        {
            get
            {
                if(_CreateProjectCommand == null)
                {
                    _CreateProjectCommand = new ViewModelCommand(CreateNewProject);
                }
                return _CreateProjectCommand;
            }
        }

        public void CreateNewProject()
        {
            this.npw.Show();
        }

        #endregion


        #region OpenProjectCommand

        private ViewModelCommand _OpenProjectCommand;

        public ViewModelCommand OpenProjectCommand
        {
            get
            {
                if(_OpenProjectCommand == null)
                {
                    _OpenProjectCommand = new ViewModelCommand(OpenProject);
                }
                return _OpenProjectCommand;
            }
        }

        public void OpenProject()
        {
            if(this.Model.CurrentProject != null)
            {
                var dialog = new TaskDialog();
                dialog.Caption = "警告";
                dialog.InstructionText = "プロジェクトはすでに開かれています。";
                dialog.Text = "既に開かれているプロジェクトを閉じて、別のプロジェクトを開きますか？";
                dialog.Icon = TaskDialogStandardIcon.Warning;
                dialog.StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No;
                dialog.Opened += (s, e) =>
                {
                    var d = (TaskDialog)s;
                    d.Icon = d.Icon;
                };
                dialog.Show();
            }
        }

        #endregion


        public string Title
        {
            get
            {
                return "Minecraft Mod Development Kit " + Define.GetVersion();
            }
        }
    }
}
