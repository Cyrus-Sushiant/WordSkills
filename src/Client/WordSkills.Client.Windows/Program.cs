﻿using WordSkills.Client.Windows.Configuration;
using Velopack;

namespace WordSkills.Client.Windows;

public partial class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        string? appCenterSecret = null;
        if (appCenterSecret is not null)
        {
            Microsoft.AppCenter.AppCenter.Start(appCenterSecret, typeof(Microsoft.AppCenter.Crashes.Crashes), typeof(Microsoft.AppCenter.Analytics.Analytics));
        }
        
        // https://github.com/velopack/velopack
        VelopackApp.Build().Run();
        var application = new App();
        application.InitializeComponent();
        Task.Run(async () =>
        {
            try
            {
                var services = await App.Current.Dispatcher.InvokeAsync(() => ((MainWindow)App.Current.MainWindow).BlazorWebView.Services);
                var windowsUpdateSettings = services.GetRequiredService<IConfiguration>().GetSection("WindowsUpdateSettings")?.Get<WindowsUpdateSettings>();
                if (windowsUpdateSettings?.FilesUrl is null)
                {
                    return;
                }
                var updateManager = new UpdateManager(windowsUpdateSettings.FilesUrl);
                var updateInfo = await updateManager.CheckForUpdatesAsync();
                if (updateInfo is not null)
                {
                    await updateManager.DownloadUpdatesAsync(updateInfo);
                    if (windowsUpdateSettings.AutoReload)
                    {
                        updateManager.ApplyUpdatesAndRestart(updateInfo, args);
                    }
                }
            }
            catch { }
        });
        application.Run();
    }
}
