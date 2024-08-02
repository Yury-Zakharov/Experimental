using Autofac.Extensions.DependencyInjection;
using IntelliFlo.AppStartup.DefaultAppSettings;
using IntelliFlo.AppStartup.Utils;
using IntelliFlo.Platform.Config.AwsSecrets;
using IntelliFlo;
using VaultConfigProvider.Core;
using System.Text.RegularExpressions;
using Serilog;

namespace Microservice.Experimental.Platform;

/// <summary>
/// 
/// </summary>
public static class MinimalMicroserviceApp
{
    private static readonly Regex ParameterRegex = new(@"\-(?<parameter>.*?)\:(?<value>.*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    private const string DefaultAppSettingsFile = "AppStartup.DefaultAppSettings.appsettings.default.json";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <param name="switchMappings"></param>
    /// <typeparam name="TStartup"></typeparam>
    /// <returns></returns>
    public static WebApplicationBuilder Build<TStartup>(string[] args, Dictionary<string, string>? switchMappings = null) where TStartup : class
    {
        var filteredArgs = FilterParameters(args);
        var enrichedArgs = HostParameterEnricher.Enrich(filteredArgs);

        var builder = WebApplication.CreateBuilder(enrichedArgs);

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())

            .ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.AddEmbeddedJsonFile(DefaultAppSettingsFile);

                var config = configuration.Build();
                // add secrets from AWS Secrets Manager
                configuration.AddAwsSecretsConfiguration(config);

                // add secrets from Vault
                configuration.AddVaultConfiguration(config);

                if (switchMappings?.Count > 0)
                    configuration.AddCommandLine(args, switchMappings);

            })
            .ConfigureLogging((_, logging) =>
            {
                // Lines below are required to exclude default .NET Core logging providers
                // These providers are excluded to make sure that logging format remains
                // the same as in .NET Framework microservices.
                logging.ClearProviders();
            })
            .UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<TStartup>();
            });


        return builder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string[] FilterParameters(IEnumerable<string> args) =>
        args
            .Select(argument =>
            {
                var match = ParameterRegex.Match(argument);

                if (!match.Success)
                    return argument;

                var paramName = match.Groups["parameter"].Value;
                var paramValue = match.Groups["value"].Value;

                return $"{paramName}={paramValue}";
            })
            .Where(argument => !string.IsNullOrWhiteSpace(argument))
            .ToArray();
}