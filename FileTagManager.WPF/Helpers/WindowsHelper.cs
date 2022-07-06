using FileTagManager.WPF.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FileTagManager.WPF.Helpers
{
    public static class WindowsHelper
    {
        static readonly List<Window> Windows = new List<Window>();

        public static T CreateWindow<T>(T window) where T : Window
        {
            Windows.Add(window);
            window.Closed += WindowClosed;
            window.IsVisibleChanged += WindowIsVisibleChanged;
            return window;
        }

        static void WindowIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var initDirView = Application.Current.Windows.OfType<InitDirView>().Single();
            initDirView.Visibility = Equals(e.NewValue, true) ? Visibility.Hidden : Visibility.Visible;
        }

        static void WindowClosed(object sender, System.EventArgs e)
        {
            var window = (Window)sender;
            window.Closed -= WindowClosed;
            window.IsVisibleChanged -= WindowIsVisibleChanged;
            Windows.Remove(window);
        }
    }
}
