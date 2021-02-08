using System.Net.Sockets;
using System.Threading.Tasks;
using Melville.MVVM.WaitingServices;

namespace Melville.Wpf.Samples.WaitMessageTest
{
    public class WaitSampleViewModel
    {
        public async Task SimpleWait(IWaitingService waiter)
        {
            using (waiter.WaitBlock("Waiting ..."))
            {
                await Task.Delay(5000);
            }
        }
    }
}