using System.Diagnostics;
using Avalonia.Controls;
using LibVLCSharp.Shared;
using ReactiveUI;

namespace English2k23.ViewModels;

public class VideoPlayerModel : ViewModelBase
{
    private string _errorMsg;
    private readonly LibVLC? _libVLC;
    private readonly string? _videoPath;
    public MediaPlayer? MediaPlayer;

    public VideoPlayerModel(string? videoPath)
    {
        _videoPath = videoPath;

        if (!Design.IsDesignMode)
        {
            //var os = AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetRuntimeInfo().OperatingSystem;
            //if (os == OperatingSystemType.WinNT)
            //{
            //    var libVlcDirectoryPath = Path.Combine(Environment.CurrentDirectory, "libvlc", IsWin64() ? "win-x64" : "win-x86");
            //    Core.Initialize(libVlcDirectoryPath);
            //}
            //else
            {
                Core.Initialize();
            }

            _libVLC = new LibVLC(
                true
            );
            _libVLC.Log += VlcLogger_Event;

            MediaPlayer = new MediaPlayer(_libVLC);
        }
    }

    public string ErrorMsg
    {
        get => _errorMsg;
        set => this.RaiseAndSetIfChanged(ref _errorMsg, value);
    }

    public void MediaEndReached(object sender, EventArgs args)
    {
        ThreadPool.QueueUserWorkItem(_ => Stop());
    }

    private void VlcLogger_Event(object? sender, LogEventArgs l)
    {
        Debug.WriteLine(l.Message);
    }

    public void Play()
    {
        if (_libVLC != null && MediaPlayer != null)
        {
            //string[] Media_AdditionalOptions = {
            //    $":avcodec-hw=any"
            //};
            string[] Media_AdditionalOptions = { };

            // Try to get path from Videos repertory. If unsuccessful, take absolute path

            Media media;
            if (string.IsNullOrWhiteSpace(_videoPath))
            {
                ErrorMsg = "An error occured, no path was given";
            }
            else
            {
                ErrorMsg = "";
                media = new Media(
                    _libVLC,
                    new Uri($"{AppDomain.CurrentDomain.BaseDirectory}Videos{Path.DirectorySeparatorChar}{_videoPath}"),
                    Media_AdditionalOptions
                );
                Console.WriteLine(media.Mrl);
                MediaPlayer.Play(media);

                media.Dispose();
            }
        }
    }


    public void Stop()
    {
        if (MediaPlayer != null)
        {
            MediaPlayer.Stop();
            Console.WriteLine("Stopping...");
        }
    }

    public void Dispose()
    {
        MediaPlayer?.Dispose();
        _libVLC?.Dispose();
    }

    public static bool IsWin64()
    {
        if (nint.Size == 4) return false;

        return true;
    }
}