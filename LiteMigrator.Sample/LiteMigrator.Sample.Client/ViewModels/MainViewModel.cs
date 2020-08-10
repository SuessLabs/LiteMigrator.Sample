using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using LiteMigrator.Sample.Client.Services;
using Prism.Commands;
using Prism.Navigation;
using Xeno.LiteMigrator;
using Xeno.LiteMigrator.DataObjects;
using Xeno.LiteMigrator.Versioning;

namespace LiteMigrator.Sample.Client.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private string _errorMessage;
    private string _latestVersion;
    private LiteMigration _liteMigrator;
    private ILogService _log;

    public MainViewModel(INavigationService nav, ILogService logService)
        : base(nav)
    {
      Title = "Main Page";

      _log = logService;

      MigrationsAvailable = new ObservableCollection<IMigration>();
      MigrationsMissing = new ObservableCollection<IMigration>();
      MigrationsInstalled = new ObservableCollection<IVersionInfo>();

      StatusMessage = string.Empty;
      LatestVersion = string.Empty;
    }

    public DelegateCommand CmdApplyMigrations => new DelegateCommand(OnApplyMigrationsAsync);

    public DelegateCommand CmdGetAllMigrations => new DelegateCommand(OnGetAllMigrations);

    public DelegateCommand CmdGetInstalledMigrations => new DelegateCommand(OnGetInstalledMigrationsAsync);

    public DelegateCommand CmdGetMissingMigrations => new DelegateCommand(OnGetMissingMigrationsAsync);

    public DelegateCommand CmdRemoveDatabase => new DelegateCommand(OnRemoveDatabase);

    public string LatestVersion
    {
      get => _latestVersion;
      set => SetProperty(ref _latestVersion, value);
    }

    public ObservableCollection<IMigration> MigrationsAvailable { get; set; }

    public ObservableCollection<IVersionInfo> MigrationsInstalled { get; set; }

    public ObservableCollection<IMigration> MigrationsMissing { get; set; }

    public string StatusMessage
    {
      get => _errorMessage;
      set => SetProperty(ref _errorMessage, value);
    }

    private string DatabasePath
    {
      get
      {
        // var path = ":memory:";
        var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        path = Path.Combine(path, "LiteMigratorTest.db3");

        return path;
      }
    }

    public override void Initialize(INavigationParameters parameters)
    {
      base.Initialize(parameters);

      var resourceAssembly = Assembly.GetExecutingAssembly();
      var resourceNamespace = "LiteMigrator.Sample.Client.Scripts";

      _liteMigrator = new LiteMigration(DatabasePath, resourceAssembly, resourceNamespace);
    }

    private async void OnApplyMigrationsAsync()
    {
      var resourceAssembly = Assembly.GetExecutingAssembly();
      var resourceNamespace = "LiteMigrator.Sample.Client.Scripts";

      _liteMigrator = new LiteMigration(DatabasePath, resourceAssembly, resourceNamespace);
      bool success = await _liteMigrator.MigrateUpAsync();
      StatusMessage = success ? "Installed" : "Error: " + _liteMigrator.LastError;
    }

    /// <summary>
    /// Gets a list of all available migration scripts whether they're installed yet or not.
    /// </summary>
    private void OnGetAllMigrations()
    {
      _log.Debug("==================");
      _log.Debug("==[ All Migrations");

      MigrationsAvailable.Clear();

      try
      {
        var sortedMigrations = _liteMigrator.Migrations.GetSortedMigrations();

        foreach (var mResource in sortedMigrations)
        {
          IMigration mig = mResource.Value;

          MigrationsAvailable.Add(mig);

          _log.Debug("Script: " + mig.Script);
          _log.Debug("Version Number: " + mig.VersionNumber);
          _log.Debug("Applied Dttm: " + mig.AppliedDttm);
          _log.Debug("Description: " + mig.Description);
          _log.Debug("--------------");
        }
      }
      catch (Exception ex)
      {
        _log.Error($"Error: {ex.Message}");
      }

      StatusMessage = _liteMigrator.LastError;
    }

    /// <summary>
    /// Migration scripts installed and registered in the database
    /// </summary>
    private async void OnGetInstalledMigrationsAsync()
    {
      _log.Debug("========================");
      _log.Debug("==[ Migrations Installed");

      MigrationsInstalled.Clear();

      try
      {
        var migs = await _liteMigrator.GetInstalledMigrationsAsync();

        foreach (var verInfo in migs)
        {
          IVersionInfo mig = verInfo.Value;

          MigrationsInstalled.Add(mig);

          _log.Debug("Version Number: " + mig.VersionNumber);
          _log.Debug("Applied Dttm: " + mig.AppliedDttm);
          _log.Debug("Description: " + mig.Description);
          _log.Debug("--------------");
        }
      }
      catch (Exception ex)
      {
        _log.Error($"Error: {ex.Message}");
      }

      StatusMessage = _liteMigrator.LastError;
    }

    /// <summary>
    /// List of migrations scripts not installed yet
    /// </summary>
    private async void OnGetMissingMigrationsAsync()
    {
      try
      {
        _log.Debug("============================");
        _log.Debug("==[ Migrations Not Installed");

        MigrationsMissing.Clear();

        var notInstalled = await _liteMigrator.GetMissingMigrationsAsync();

        foreach (var mResource in notInstalled)
        {
          IMigration mig = mResource.Value;

          MigrationsMissing.Add(mig);

          _log.Debug("Script: " + mig.Script);
          _log.Debug("Version Number: " + mig.VersionNumber);
          _log.Debug("Applied Dttm: " + mig.AppliedDttm);
          _log.Debug("Description: " + mig.Description);
          _log.Debug("--------------");
        }
      }
      catch (Exception ex)
      {
        _log.Error($"Error: {ex.Message}");
      }

      StatusMessage = _liteMigrator.LastError;
    }

    private void OnRemoveDatabase()
    {
      MigrationsAvailable.Clear();
      MigrationsInstalled.Clear();
      MigrationsMissing.Clear();

      if (File.Exists(DatabasePath))
        File.Delete(DatabasePath);

      StatusMessage = File.Exists(DatabasePath) ? "Failed to remove DB" : "DB Removed";
      StatusMessage += $" from {DatabasePath}";

      _log.Info(StatusMessage);
    }

    private void SampleDialog()
    {
      // Log Sample
      ////_log.Debug("Debug message.");
      ////_log.Info("Info message.");
      ////_log.Warn("Warning message.");
      ////_log.Error("Error message.");
      ////_log.Fatal("Fatal-error message.");
    }
  }
}
