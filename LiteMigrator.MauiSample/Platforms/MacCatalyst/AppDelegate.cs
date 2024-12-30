using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace LiteMigrator.MauiSample;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
  protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
