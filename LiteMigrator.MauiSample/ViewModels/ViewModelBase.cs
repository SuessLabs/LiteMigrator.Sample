using Prism.Mvvm;
using Prism.Navigation;

namespace LiteMigrator.MauiSample.ViewModels;

public class ViewModelBase : BindableBase, IInitialize, INavigatedAware
{
  private string _title = string.Empty;

  public ViewModelBase(INavigationService navigationService) => NavigationService = navigationService;

  public string Title { get => _title; set => SetProperty(ref _title, value); }

  protected INavigationService NavigationService { get; private set; }

  public virtual void Initialize(INavigationParameters parameters)
  {
  }

  public virtual void OnNavigatedFrom(INavigationParameters parameters)
  {
  }

  public virtual void OnNavigatedTo(INavigationParameters parameters)
  {
  }
}
