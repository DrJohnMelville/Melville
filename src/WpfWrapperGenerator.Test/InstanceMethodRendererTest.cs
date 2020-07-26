using System.Windows.Documents;
using Moq;
using Xunit;

namespace WpfWrapperGenerator.Test
{
    public class InstanceMethodRendererTest
    {
        private readonly Mock<IRenderMethods> render = new Mock<IRenderMethods>();
        

        [Fact]
        public void DoesNotRenderFloater_HorizontalAlignment()
        {
            var type = typeof(Floater);
            var field = type.GetField(nameof(Floater.HorizontalAlignmentProperty));
            var dp = Floater.HorizontalAlignmentProperty;
            InstanceMethodRenderer.TryRender(field, dp, render.Object);
            render.VerifyNoOtherCalls();
        }

    }
}