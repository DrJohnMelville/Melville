using Melville.IOC.AspNet.RegisterFromServiceCollection;
using Melville.IOC.IocContainers;
using Microsoft.Extensions.Logging;

namespace Maui.Scratch;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.ConfigureContainer(new MelvilleServiceProviderFactory(true, Initialize) );
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void Initialize(IBindableIocService obj)
    {
    }
}
