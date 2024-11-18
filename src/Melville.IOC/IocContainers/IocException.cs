using System;
using System.Collections.Generic;
using System.Linq;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers;

public class IocException : Exception
{
    public IocException(string? message) : base(message)
    {
    }

    public IocException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}