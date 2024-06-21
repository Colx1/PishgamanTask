using PishgamanTask.Maui.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using PishgamanTask.Maui.Authentication;
using PishgamanTask.Maui.Services;
using Blazored.LocalStorage;

namespace PishgamanTask.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

			builder.Services.AddSingleton<IPlatformHttpMessageHandler>(sp =>
			{
#if ANDROID
                return new AndroidHttpMessageHandler();
#else
				return null!;
#endif

			});

            builder.Services.AddHttpClient();
			builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddAuthorizationCore();


            //         var baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7067" : "https://localhost:7067";
            //builder.Services.AddScoped(sp => new HttpClient {
            //             BaseAddress = new Uri(baseAddress) });


            builder.Services.AddHttpClient("custom-httpclient", httpClient =>
            {
                var baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7067" : "https://localhost:7067";
                httpClient.BaseAddress = new Uri(baseAddress);
            }).ConfigureHttpMessageHandlerBuilder(configBuilder =>
            {
                var platformMessageHandler = configBuilder.Services.GetRequiredService<IPlatformHttpMessageHandler>();
                configBuilder.PrimaryHandler = platformMessageHandler.GetHttpMessageHandler();
            });

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IPersonService, PersonService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
