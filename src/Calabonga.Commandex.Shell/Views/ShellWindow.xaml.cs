﻿using System.Windows;

namespace Calabonga.Commandex.Shell.Views;

/// <summary>
/// Interaction logic for ShellWindow.xaml
/// </summary>
public partial class ShellWindow : Window
{
    public ShellWindow()
    {
        InitializeComponent();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        Application.Current.Shutdown();
    }
}
