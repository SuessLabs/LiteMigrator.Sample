using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using LiteMigrator.DataObjects;
using LiteMigrator.MauiSample.Services;
using LiteMigrator.Versioning;
using Microsoft.Maui.Storage;
using Prism.Commands;
using Prism.Navigation;

namespace LiteMigrator.MauiSample.ViewModels;

public class MainPageViewModel : ViewModelBase
{
  private string _errorMessage = string.Empty;
  private string _latestVersion = string.Empty;
  private LogService _log;
  private bool _useExternalScripts;

  public MainPageViewModel(INavigationService navigationService, LogService log)
    : base(navigationService)
  {
    _log = log;

    StatusMessage = string.Empty;
    LatestVersion = string.Empty;

    Title = "LiteMigrator Sample";
  }

  public DelegateCommand CmdApplyMigrations => new(OnApplyMigrationsAsync);

  public DelegateCommand CmdGetAllMigrations => new(OnGetAllMigrations);

  public DelegateCommand CmdGetInstalledMigrations => new(OnGetInstalledMigrationsAsync);

  public DelegateCommand CmdGetMissingMigrations => new(OnGetMissingMigrationsAsync);

  public DelegateCommand CmdRemoveDatabase => new(OnRemoveDatabase);

  public string LatestVersion { get => _latestVersion; set => SetProperty(ref _latestVersion, value); }

  public ObservableCollection<IMigration> MigrationsAvailable { get; set; } = [];

  public ObservableCollection<IVersionInfo> MigrationsInstalled { get; set; } = [];

  public ObservableCollection<IMigration> MigrationsMissing { get; set; } = [];

  public string StatusMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }

  /// <summary>Load migration scripts from external DLL (LiteMigrator.Sample.Common).</summary>
  public bool UseExternalScripts { get => _useExternalScripts; set => SetProperty(ref _useExternalScripts, value); }

  public override void Initialize(INavigationParameters parameters)
  {
    base.Initialize(parameters);
  }

  private string GetDatabasePath()
  {
    //// var path = ":memory:";
    //// var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
    var path = FileSystem.AppDataDirectory;

    path = Path.Combine(path, "LiteMigratorTest.db3");

    return path;
  }

  private LiteMigration InitLiteMigrator()
  {
    // Scripts from main project
    if (!UseExternalScripts)
    {
      var assm = Assembly.GetExecutingAssembly();
      var resourceNamespace = "LiteMigrator.MauiSample.Scripts";
      return new LiteMigration(GetDatabasePath(), resourceNamespace, assm);
    }
    else
    {
      // Perform migrations from external project
      return LiteMigrator.Sample.Common.MigrationRunner.InitLiteMigrator(GetDatabasePath());

      // Otherwise, it gets messy and unstable doing the following
      // because .NET's AOT will remove the "unused" assembly file.
      ////var allAssms = AppDomain.CurrentDomain.GetAssemblies();
      ////assm = allAssms.Where(x => x.FullName.Contains("LiteMigrator.Sample.Common")).FirstOrDefault(assm);
    }
  }

  private async void OnApplyMigrationsAsync()
  {
    using var migrator = InitLiteMigrator();

    try
    {
      bool success = await migrator.MigrateUpAsync();
      StatusMessage = success ? "Installed" : "Error: " + migrator.LastError;
    }
    catch (Exception ex)
    {
      _log.Error(ex.Message);

      StatusMessage = $"Error! {migrator.LastError}";
    }
  }

  /// <summary>
  /// Gets a list of all available migration scripts whether they're installed yet or not.
  /// </summary>
  private void OnGetAllMigrations()
  {
    _log.Debug("==================");
    _log.Debug("==[ All Migrations");

    MigrationsAvailable.Clear();

    using var migrator = InitLiteMigrator();

    try
    {
      var sortedMigrations = migrator.Migrations.GetSortedMigrations();

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

    StatusMessage = migrator.LastError;
  }

  /// <summary>
  /// Migration scripts installed and registered in the database
  /// </summary>
  private async void OnGetInstalledMigrationsAsync()
  {
    _log.Debug("========================");
    _log.Debug("==[ Migrations Installed");

    MigrationsInstalled.Clear();

    using var migrator = InitLiteMigrator();

    try
    {
      var migs = await migrator.GetInstalledMigrationsAsync();

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

    StatusMessage = migrator.LastError;
  }

  /// <summary>
  /// List of migrations scripts not installed yet
  /// </summary>
  private async void OnGetMissingMigrationsAsync()
  {
    using var migrator = InitLiteMigrator();

    try
    {
      _log.Debug("============================");
      _log.Debug("==[ Migrations Not Installed");

      MigrationsMissing.Clear();

      var notInstalled = await migrator.GetMissingMigrationsAsync();

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

    StatusMessage = migrator.LastError;
  }

  private void OnRemoveDatabase()
  {
    MigrationsAvailable.Clear();
    MigrationsInstalled.Clear();
    MigrationsMissing.Clear();

    try
    {
      if (File.Exists(GetDatabasePath()))
        File.Delete(GetDatabasePath());
    }
    catch (Exception ex)
    {
      _log.Error(ex.Message);
    }

    StatusMessage = File.Exists(GetDatabasePath()) ? "Failed to remove DB" : "DB Removed";
    StatusMessage += $" from {GetDatabasePath()}";

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
