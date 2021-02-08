using System;
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
        public  Task WaitProgress(IWaitingService waiter) => CountToFive(waiter, _=>waiter.MakeProgress(), 5);
        public  Task WaitProgressWithLabel(IWaitingService waiter) => CountToFive(waiter, i=>waiter.MakeProgress($"Step #: {i}"), 5);
        public  Task WaitLabel(IWaitingService waiter) => CountToFive(waiter, i=>waiter.ProgressMessage = $"Step without bar #: {i}", double.MinValue);
        

        private static async Task CountToFive(IWaitingService waiter, Action<int> update, double maximum)
        {
            using (waiter.WaitBlock("Waiting ...", maximum))
            {
                for (int i = 0; i < 5; i++)
                {
                    update(i);
                    await Task.Delay(1000);
                }
            }
        }
    }
}