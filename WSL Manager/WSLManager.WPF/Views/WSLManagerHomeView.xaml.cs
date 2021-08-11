using System.Windows.Controls;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;

namespace WSLManager.WPF.Views
{
    [MvxContentPresentation]
    public partial class WSLManagerHomeView : MvxWpfView
    {
        public WSLManagerHomeView()
        {
            InitializeComponent();
        }

    }
}