﻿using System.Windows.Controls;
using Calabonga.Commandex.Contracts;

namespace Calabonga.Commandex.QuizCommand
{
    /// <summary>
    /// Interaction logic for QuizDialogView.xaml
    /// </summary>
    public partial class QuizDialogView : UserControl, IDialogView
    {
        public QuizDialogView()
        {
            InitializeComponent();
        }
    }
}