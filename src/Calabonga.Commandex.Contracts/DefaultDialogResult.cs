﻿namespace Calabonga.Commandex.Contracts;

public partial class DefaultDialogResult<T> : ViewModelBase, IDialogResult
{
    public string DialogTitle => "Default Dialog Result";

    public bool Ok => true;

    public T? Result { get; set; }
}