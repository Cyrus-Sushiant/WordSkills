﻿using System.Net.Http;
using WordSkills.Client.Windows.Services;
using Microsoft.Extensions.Logging;

namespace WordSkills.Client.Windows;

public static partial class Program
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        // Services being registered here can get injected in windows project only.

        ConfigurationBuilder configurationBuilder = new();
        configurationBuilder.AddClientConfigurations();
        var configuration = configurationBuilder.Build();
        services.TryAddTransient<IConfiguration>(sp => configuration);

        Uri.TryCreate(configuration.GetApiServerAddress(), UriKind.Absolute, out var apiServerAddress);
        services.TryAddSingleton(sp =>
        {
            var handler = sp.GetRequiredKeyedService<DelegatingHandler>("DefaultMessageHandler");
            HttpClient httpClient = new(handler)
            {
                BaseAddress = apiServerAddress
            };
            return httpClient;
        });

        services.AddWpfBlazorWebView();
        if (BuildConfiguration.IsDebug())
        {
            services.AddBlazorWebViewDeveloperTools();
        }

        services.TryAddTransient<IStorageService, WindowsStorageService>();
        services.TryAddTransient<IBitDeviceCoordinator, WindowsDeviceCoordinator>();
        services.TryAddTransient<IExceptionHandler, WindowsExceptionHandler>();
        services.AddSingleton<ILocalHttpServer>(sp => new WindowsLocalHttpServer(services));

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
            loggingBuilder.AddEventLog();
            loggingBuilder.AddEventSourceLogger();
            if (BuildConfiguration.IsDebug())
            {
                loggingBuilder.AddDebug();
            }
            loggingBuilder.AddConsole();
            if (Microsoft.AppCenter.AppCenter.Configured)
            {
                loggingBuilder.AddAppCenter(options => { });
            }
            loggingBuilder.AddApplicationInsights(config =>
            {
                config.TelemetryInitializers.Add(new WindowsTelemetryInitializer());
                var connectionString = configuration["ApplicationInsights:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString) is false)
                {
                    config.ConnectionString = connectionString;
                }
            }, options =>
            {
                options.IncludeScopes = true;
            });
        });

        services.AddClientCoreProjectServices();
    }
}
