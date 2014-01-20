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

namespace McMDK.ViewModels.Config
{
    public class CommonConfigViewModel : ViewModel
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

        public void Initialize()
        {
        }

        #region IsShowToolTip変更通知プロパティ

        private bool _IsShowToolTip;
        public bool IsShowToolTip
        {
            get
            {
                return this._IsShowToolTip;
            }
            set
            {
                if (EqualityComparer<bool>.Default.Equals(this._IsShowToolTip, value))
                {
                    return;
                }
                this._IsShowToolTip = value;
                RaisePropertyChanged("IsShowToolTip");
            }
        }

        #endregion


        #region IsSendReport

        private bool _IsSendReport;
        public bool IsSendReport
        {
            get
            {
                return this._IsSendReport;
            }
            set
            {
                if(EqualityComparer<bool>.Default.Equals(this._IsSendReport, value))
                {
                    return;
                }
                this._IsSendReport = value;
                RaisePropertyChanged("IsSendReport");
            }
        }

        #endregion


        #region IsAutoUpdate変更通知プロパティ

        private bool _IsAutoUpdate;
        public bool IsAutoUpdate
        {
            get
            {
                return this._IsAutoUpdate;
            }
            set
            {
                if(EqualityComparer<bool>.Default.Equals(this._IsAutoUpdate, value))
                {
                    return;
                }
                this._IsAutoUpdate = value;
                RaisePropertyChanged("IsAutoUpdate");
            }
        }

        #endregion


        #region IsNoAssets

        private bool _IsNoAssets;
        public bool IsNoAssets
        {
            get
            {
                return this._IsNoAssets;
            }
            set
            {
                if (EqualityComparer<bool>.Default.Equals(this._IsNoAssets, value))
                {
                    return;
                }
                this._IsNoAssets = value;
                RaisePropertyChanged("IsNoAssets");
            }
        }

        #endregion


        #region IsBackgroundWork

        private bool _IsBackgroundWork;
        public bool IsBackgroundWork
        {
            get
            {
                return this._IsBackgroundWork;
            }
            set
            {
                if (EqualityComparer<bool>.Default.Equals(this._IsBackgroundWork, value))
                {
                    return;
                }
                this._IsBackgroundWork = value;
                RaisePropertyChanged("IsBackgroundWork");
            }
        }

        #endregion


        #region IsOfflineWork

        private bool _IsOfflineWork;
        public bool IsOfflineWork
        {
            get
            {
                return this._IsOfflineWork;
            }
            set
            {
                if (EqualityComparer<bool>.Default.Equals(this._IsOfflineWork, value))
                {
                    return;
                }
                this._IsOfflineWork = value;
                RaisePropertyChanged("IsOfflineWork");
                if(this._IsOfflineWork)
                {
                    this.IsNoAssets = true;
                    this.IsSendReport = false;
                    this.IsAutoUpdate = false;
                }
            }
        }

        #endregion
    }
}
