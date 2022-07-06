using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics.CodeAnalysis;

namespace FileTagManager.WPF.Services
{
    public interface IOpenFileDialogService
    {
        string SelectedPath { get; }
        string SelectedSavePath { get; }
        CommonFileDialogResult SelectFolderDialog();
        CommonFileDialogResult SelectFileDialog(string type, string extension);
        CommonFileDialogResult SaveFileDialog();
    }
}
