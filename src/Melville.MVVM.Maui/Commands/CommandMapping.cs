using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Melville.MVVM.Maui.Commands;

public static class CommandMapping
{
    public static void MapEnabledTo<T>(
        this CommandBase command, T target, Expression<Func<T,bool>> enabledExpression)
        where T : INotifyPropertyChanged
    {
        var method = enabledExpression.Compile();
        var names = PropertyExpressionVisitor.ScanForNames(enabledExpression, typeof(T));
        target.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName is null || names.Contains(e.PropertyName)) 
                command.IsEnabled = method(target);
        }; 
        command.IsEnabled = method(target);
    }
}