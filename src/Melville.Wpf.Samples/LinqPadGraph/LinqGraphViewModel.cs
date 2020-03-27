using System.Linq;
using System.Windows.Media.Imaging;
using Melville.Linq.Statistics.Graphics;
using Melville.Linq.Statistics.HypothesisTesting;

namespace Melville.Wpf.Samples.LinqPadGraph
{
    public class LinqGraphViewModel
    {
        public CanCreateGraphWithData<int> Source { get; } = 
            Enumerable.Range(1, 9).SelectMany(i => Enumerable.Repeat(i, i))
                .Graph().Histogram(i => i);

        public BitmapSource OutputImage { get; }

        public LinqGraphViewModel()
        {
            OutputImage = Source.ToWpfBitmap();
        }
    }
}