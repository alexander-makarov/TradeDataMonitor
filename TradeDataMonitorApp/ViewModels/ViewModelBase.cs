using System.ComponentModel;
using System.Runtime.CompilerServices;
using TradeDataMonitorApp.MvvmHelpers;

namespace TradeDataMonitorApp.ViewModels
{
    /// <summary>
    /// Base viewModel class
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
