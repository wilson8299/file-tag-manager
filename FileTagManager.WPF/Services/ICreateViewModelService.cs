using FileTagManager.WPF.ViewModels;

namespace FileTagManager.WPF.Services
{
    public interface ICreateViewModelService
    {
        BaseViewModel CreateContainerViewModel(VCType vmType);
        BaseViewModel CreateDetailViewModel(VDType vmType);
    }
}
