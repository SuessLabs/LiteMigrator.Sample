using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using LiteMigrator.MauiSample.Views;
using LiteMigrator.MauiSample.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using LiteMigrator.MauiSample.Services;

namespace LiteMigrator.MauiSample;

public static class MauiProgram
{
  public static MauiApp CreateMauiApp()
  {
    var builder = MauiApp.CreateBuilder();
    builder
      .UseMauiApp<App>()
      .UsePrism(prism =>
      {
        prism.ConfigureModuleCatalog(catalog =>
        {
          // Prism Modules here
        })
        .RegisterTypes(container =>
        {
          // Services
          container.RegisterSingleton<LogService>();

          // View registration
          container.RegisterForNavigation<MainPage>();
        })
        // Prism.Maui.Rx:
        ////.AddGlobalNavigationObserver(context => context.Subscribe(x =>
        ////{
        ////  if (x.Type == NavigationRequestType.Navigate)
        ////    Console.WriteLine($"Navigation (URL): {x.Uri}");
        ////  else
        ////    Console.WriteLine($"Navigation (Type): {x.Type}");
        ////}))
        .CreateWindow(nav => nav.CreateBuilder()
          .AddSegment<MainPageViewModel>()
          .NavigateAsync(OnNavigationError)
        );
      })
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

  private static void OnNavigationError(Exception exception)
  {
    Console.WriteLine(exception);
    System.Diagnostics.Debugger.Break();
  }
}
