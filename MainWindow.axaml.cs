using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;
using Avalonia.Markup.Xaml;
using FluentAvalonia.UI.Controls;
using StarLight_Core.Utilities;
using System.Reflection;
using FSLAvalonia.Pages;

// 感谢 @呆鱼 提供的YMCL启动器源码！

namespace FSLAvalonia
{
    public partial class MainWindow : Window
    {
        Pages.Mainpage Mainpage = new();
        Pages.Accounts Accounts = new();

        public class info
        {
            public static string java { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void NavigationView_SelectionChanged(object? s, FluentAvalonia.UI.Controls.NavigationViewSelectionChangedEventArgs e)
        {
            switch (((NavigationViewItem)((NavigationView)s!).SelectedItem!).Tag)
            {
                case "主页":
                    Nav.Content = Mainpage;
                    break;
                case "账户":
                    Nav.Content = Accounts;
                    break;
            }
        }
    }
}