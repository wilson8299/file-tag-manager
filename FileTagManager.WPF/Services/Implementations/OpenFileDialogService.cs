using Microsoft.WindowsAPICodePack.Dialogs;
using System;

namespace FileTagManager.WPF.Services
{
    public class OpenFileDialogService : IOpenFileDialogService
    {
        private CommonOpenFileDialog _dialog;
        private CommonSaveFileDialog _saveDialog;

        public string SelectedPath => _dialog.FileName;
        public string SelectedSavePath => _saveDialog.FileName;

        public CommonFileDialogResult SelectFolderDialog()
        {
            _dialog = new CommonOpenFileDialog();
            _dialog.Multiselect = false;
            _dialog.IsFolderPicker = true;
            _dialog.Filters.Clear();
            return _dialog.ShowDialog();
        }

        public CommonFileDialogResult SelectFileDialog(string type, string extension)
        {
            _dialog = new CommonOpenFileDialog();
            _dialog.Multiselect = false;
            _dialog.IsFolderPicker = false;
            _dialog.Filters.Clear();
            _dialog.Filters.Add(new CommonFileDialogFilter(type, extension));
            return _dialog.ShowDialog();
        }

        public CommonFileDialogResult SaveFileDialog()
        {
            _saveDialog = new CommonSaveFileDialog();
            _saveDialog.DefaultFileName = $"file_tag_{DateTime.Now:yyyy.M.dd_HH-mm-ss}";
            _saveDialog.DefaultExtension = "json";
            _saveDialog.Filters.Add(new CommonFileDialogFilter("Json", "json"));
            return _saveDialog.ShowDialog();
        }
    }
}
