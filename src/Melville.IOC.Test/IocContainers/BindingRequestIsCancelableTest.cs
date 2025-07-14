using FluentAssertions;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Moq;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public class BindingRequestIsCancelableTest
{
    private readonly Mock<IIocService> iocService = new();
    private readonly RootBindingRequest root;

    public BindingRequestIsCancelableTest()
    {
        root = new RootBindingRequest(typeof(BindingRequestIsCancelableTest),
            iocService.Object);
    }

    [Fact]
    public void RootRequestIsCancelable()
    {
        root.IsCancelled.Should().BeFalse();
        root.IsCancelled = true;
        root.IsCancelled.Should().BeTrue();
    }

    [Fact]
    public void ForwardedRequestIsCancelable()
    {
        var cloned = root.CreateSubRequest(typeof(BindingRequestIsCancelableTest));
        root.IsCancelled.Should().BeFalse();
        cloned.IsCancelled.Should().BeFalse();
        cloned.IsCancelled = true;
        root.IsCancelled.Should().BeTrue();
        cloned.IsCancelled.Should().BeTrue();
    }
}