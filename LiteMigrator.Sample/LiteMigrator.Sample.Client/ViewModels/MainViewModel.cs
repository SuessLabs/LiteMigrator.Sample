using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.Forms;
using LiteMigrator.Sample.Client.Services;

namespace LiteMigrator.Sample.Client.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private ILogService _log;
    private IPageDialogService _dialogService;

    public MainViewModel(INavigationService nav, ILogService logService, IPageDialogService dialogService)
        : base(nav)
    {
      Title = "Main Page";

      _log = logService;
      _dialogService = dialogService;
    }

    public DelegateCommand CmdSampleDialog => new DelegateCommand(OnSampleDialogAsync);

    public DelegateCommand CmdSampleLogging => new DelegateCommand(OnSampleLogging);

    private async void OnSampleDialogAsync()
    {
      // Sample Usage:
      //  https://prismlibrary.com/docs/xamarin-forms/dialogs/page-dialog-service.html
      var result = await _dialogService.DisplayAlertAsync("Alert", "Display a sample pop-up ActionSheet?", "Accept", "Cancel");
      _log.Debug("Response: " + result);

      if (result)
      {
        var action = await _dialogService.DisplayActionSheetAsync("Sample Action Sheet", "Cancel", null, "Email", "In-App message", "IG");
        _log.Debug("ActionSheet: " + action);
      }
    }

    private void OnSampleLogging()
    {
      _log.Debug("Debug message.");
      _log.Info("Info message.");
      _log.Warn("Warning message.");
      _log.Error("Error message.");
      _log.Fatal("Fatal-error message.");
    }
  }
}
