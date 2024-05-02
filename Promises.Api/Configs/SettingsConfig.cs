using System.Globalization;
using FluentValidation;
using Promises.Application.Common.Managers;
using Promises.Application.Common.Models;

namespace Promises.Api.Configs;

public static class SettingsConfig
{
    public static IServiceCollection AddSettingsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var cultureInfo = new CultureInfo("tr-TR");
        System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        ValidatorOptions.Global.LanguageManager.Culture = cultureInfo;
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        services.Configure<TokenSetting>(configuration.GetSection("TokenSetting"));
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.Configure<MigrationCodes>(configuration.GetSection("MigrationCodes"));
        services.Configure<FireFileConfigs>(configuration.GetSection("FireFileConfigs"));
        services.AddTransient<TokenManager>();
        services.AddTransient<FileManager>();
        return services;
    }
}