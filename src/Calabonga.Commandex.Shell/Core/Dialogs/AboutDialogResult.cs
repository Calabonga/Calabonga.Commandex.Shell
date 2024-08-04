using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Shell.Core.Engine;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Calabonga.Commandex.Shell.Core.Dialogs;

public partial class AboutDialogResult : DefaultDialogResult
{
    private readonly FileService _fileService;

    public AboutDialogResult(FileService fileService)
    {
        _fileService = fileService;
        Title = "About Commandex";
    }

    public override WindowStyle WindowStyle => WindowStyle.None;

    [ObservableProperty]
    private string _artifactsSize = "0.0 KB";


    [RelayCommand]
    private void CloseDialog() => ((Window)Owner!).Close();

    [RelayCommand]
    private void LoadData()
    {
        var total = ((float)_fileService.GetArtifactsSize() / 1024).ToString("F");
        ArtifactsSize = $"{total} KB";
    }
}