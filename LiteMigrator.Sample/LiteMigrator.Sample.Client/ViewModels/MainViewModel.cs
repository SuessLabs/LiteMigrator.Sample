using System.Reflection;
using DryIoc;
using LiteMigrator.Sample.Client.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xeno.LiteMigrator;

namespace LiteMigrator.Sample.Client.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private IPageDialogService _dialogService;
    private string _errorMessage;
    private string _latestVersion;
    private LiteMigration _liteMigrator;
    private ILogService _log;

    public MainViewModel(INavigationService nav, ILogService logService, IPageDialogService dialogService)
        : base(nav)
    {
      Title = "Main Page";

      _log = logService;
      _dialogService = dialogService;
    }

    public DelegateCommand CmdGetCurrentVersion => new DelegateCommand(OnGetCurrentVersionAsync);

    public DelegateCommand CmdMigrateUp => new DelegateCommand(OnMigrateUpAsync);

    public string ErrorMessage
    {
      get => _errorMessage;
      set => SetProperty(ref _errorMessage, value);
    }

    public string LatestVersion
    {
      get => _latestVersion;
      set => SetProperty(ref _latestVersion, value);
    }

    public override void Initialize(INavigationParameters parameters)
    {
      base.Initialize(parameters);

      ErrorMessage = string.Empty;
      LatestVersion = string.Empty;

      var dbPath = ":memory:";
      var resourceAssembly = Assembly.GetExecutingAssembly();
      var resourceNamespace = "LiteMigrator.Sample.Client.Scripts";

      _liteMigrator = new LiteMigration(dbPath, resourceAssembly, resourceNamespace);
    }

    private async void OnGetCurrentVersionAsync()
    {
      var versions = _liteMigrator.Versions;

      var sortedMigrations = _liteMigrator.Migrations.GetSortedMigrations();

      _log.Debug("=======================");
      _log.Debug("==[ All Migrations");
      foreach (var mResource in sortedMigrations)
      {
        var mig = mResource.Value;

        _log.Debug("Script: " + mig.Script);
        _log.Debug("Version Number: " + mig.VersionNumber);
        _log.Debug("Applied Dttm: " + mig.AppliedDttm);
        _log.Debug("Description: " + mig.Description);
        _log.Debug("--------------");
      }

      _log.Debug("=======================");
      _log.Debug("==[ Migrations Not Installed");
      var notInstalled = await _liteMigrator.GetMissingMigrationsAsync();
      foreach (var mResource in notInstalled)
      {
        var mig = mResource.Value;
        _log.Debug("Script: " + mig.Script);
        _log.Debug("Version Number: " + mig.VersionNumber);
        _log.Debug("Applied Dttm: " + mig.AppliedDttm);
        _log.Debug("Description: " + mig.Description);
        _log.Debug("--------------");
      }
    }

    private async void OnMigrateUpAsync()
    {
      bool success = await _liteMigrator.MigrateUpAsync();
      ErrorMessage = success ? "Installed" : "Error: " + _liteMigrator.LastError;
    }

    private void SampleDialog()
    {
      // Log Sample
      ////_log.Debug("Debug message.");
      ////_log.Info("Info message.");
      ////_log.Warn("Warning message.");
      ////_log.Error("Error message.");
      ////_log.Fatal("Fatal-error message.");

      // Dialog Sample:
      //////  https://prismlibrary.com/docs/xamarin-forms/dialogs/page-dialog-service.html
      ////var result = await _dialogService.DisplayAlertAsync("Alert", "Display a sample pop-up ActionSheet?", "Accept", "Cancel");
      ////_log.Debug("Response: " + result);
      ////
      ////if (result)
      ////{
      ////  var action = await _dialogService.DisplayActionSheetAsync("Sample Action Sheet", "Cancel", null, "Email", "In-App message", "IG");
      ////  _log.Debug("ActionSheet: " + action);
      ////}
    }
  }
}
