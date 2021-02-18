﻿using System;
using System.Windows;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.MVVM.Wpf.ViewFrames
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class OnDisplayedAttribute: Attribute
    {
        public string MethodName {get;}
        public OnDisplayedAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public void DoCall(object target, DependencyObject frame)
        {
            RunOnVisualTreeSearch.TryInvokeMethod(frame, Array.Empty<object>(), target, MethodName, out _);
        }
    }
}