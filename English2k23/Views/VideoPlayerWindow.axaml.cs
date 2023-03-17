using Avalonia.Controls;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using LibVLCSharp.Avalonia.Unofficial;

namespace English2k23.Views;

public partial class VideoPlayerWindow : ReactiveWindow<VideoPlayerModel>
{
    private static VideoPlayerWindow? _this;
    private readonly VideoView _videoViewer;


    public VideoPlayerModel? viewModel;


    public VideoPlayerWindow()
    {
        InitializeComponent();

        DataContext = ViewModel;

        _videoViewer = this.Get<VideoView>("VideoViewer");
        _this = this;

        Opened += MainWindow_Opened;
    }

    public static VideoPlayerWindow GetInstance()
    {
        return _this;
    }

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        if (_videoViewer != null && ViewModel?.MediaPlayer != null)
        {
            _videoViewer.MediaPlayer = ViewModel.MediaPlayer;
            _videoViewer.MediaPlayer.SetHandle(_videoViewer.hndl);

            // or
            //_videoViewer.MediaPlayer.Hwnd = _videoViewer.hndl.Handle;

            // Set VideoView Content property by code
            //var tmp = new PlayerControls();
            //_videoViewer.SetContent(tmp._playerControl);
        }
    }
}