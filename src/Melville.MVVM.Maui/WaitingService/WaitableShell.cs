using System;
using Melville.MVVM.Maui.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace Melville.MVVM.Maui.WaitingService;

public class WaitableShell: Shell
{
    public WaitableShell()
    {
        InheritedCommand.SetInheritedCommandParameter(
            this, new ShowProgressImplementation(Navigation));
    }
}