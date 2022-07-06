using FileTagManager.WPF.ViewModels;

namespace FileTagManager.WPF.Services
{
    public interface INavigatorService
    {
        BaseViewModel ViewContainerNavigator { get; set; }
        BaseViewModel ViewDetailNavigator { get; set; }
    }

    public enum VCType
    {
        InitDirView = 0,
        HomeView = 1
    }

    public enum VDType
    {
        FileListView = 0,
        SearchView = 1
    }
}
