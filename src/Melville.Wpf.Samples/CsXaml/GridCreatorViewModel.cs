using System.Windows.Controls;
using Melville.Mvvm.CsXaml;

namespace Melville.Wpf.Samples.CsXaml
{
    public class GridCreatorViewModel
    {
        
    }

    public class GridCreatorView : UserControl
    {
        public GridCreatorView()
        {
            AddChild(BuildXaml.Create<Grid, GridCreatorViewModel>(i =>
            {

            }));
        }
    }
}