using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;
using Melville.INPC;

namespace Melville.MVVM.Maui.Commands;

public abstract class CommandBase: ICommand
{
    public bool CanExecute(object? parameter) => IsEnabled;

    private bool isEnabled = true;
    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            if (value == isEnabled) return;
            isEnabled = value;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public event EventHandler? CanExecuteChanged;

    public abstract void Execute(object? parameter);
}

