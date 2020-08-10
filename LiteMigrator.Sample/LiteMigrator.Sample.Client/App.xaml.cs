using System;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using LiteMigrator.Sample.Client.Services;
using LiteMigrator.Sample.Client.ViewModels;
using LiteMigrator.Sample.Client.Views;

namespace LiteMigrator.Sample.Client
{
  public partial class App
  {
    /*
     * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
     * This imposes a limitation in which the App class must have a default constructor.
     * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
     */

    public App() : this(null)
    {
    }

    public App(IPlatformInitializer initializer) : base(initializer)
    {
    }

    protected override async void OnInitialized()
    {
      InitializeComponent();

      var nav = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainView)}");
      if (!nav.Success)
      {
        // Log the message, break, or throw an exception
        Console.WriteLine(nav.Exception.Message);
        System.Diagnostics.Debugger.Break();

        throw new System.Exception(nav.Exception.Message);
      }
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      // Services
      containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
      containerRegistry.RegisterSingleton<ILogService, LogService>();

      // Navigation
      containerRegistry.RegisterForNavigation<NavigationPage>();
      containerRegistry.RegisterForNavigation<MainView, MainViewModel>();

      // OnIdiom Registration
      ////containerRegistry.RegisterForNavigationOnIdiom<MainView, MainViewModel>(
      ////  desktopView: typeof(MainDesktopView),
      ////  tabletView: typeof(MainTabletView));
    }

    protected override void OnResume()
    {
      base.OnResume();

      // TODO: Refresh network data, perform UI updates, and reacquire resources like cameras, I/O devices, etc.
    }

    protected override void OnSleep()
    {
      base.OnSleep();

      // TODO: This is the time to save app data in case the process is terminated.
      // This is the perfect timing to release exclusive resources (camera, I/O devices, etc...)
    }
  }
}
