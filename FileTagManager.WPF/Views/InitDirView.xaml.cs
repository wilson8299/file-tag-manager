using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileTagManager.WPF.Views
{
    /// <summary>
    /// InitDirView.xaml 的互動邏輯
    /// </summary>
    public partial class InitDirView : UserControl
    {
        public InitDirView()
        {
            InitializeComponent();
        }

        private void ItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Scroll.ScrollToEnd();
        }
    }
}
