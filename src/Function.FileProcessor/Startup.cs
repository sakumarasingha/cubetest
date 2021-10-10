using Application.Interfaces;
using Azure.Storage;
using Azure.Storage.Files.Shares;
using Infrastructure.File;
using Infrastructure.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Function.FileProcessor.Startup))]
namespace Function.FileProcessor
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
           .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
           .AddEnvironmentVariables().Build();

            string accountName = Environment.GetEnvironmentVariable("StorageAccountName");
            string containerName = Environment.GetEnvironmentVariable("FileShareName");
            string accountKey = configurationBuilder.GetValue<string>("AccountKey");
            string containerEndpoint = string.Format("https://{0}.file.core.windows.net/{1}",
                                                        accountName,
                                                        containerName);

            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(accountName, accountKey);
            builder.Services.AddScoped(x => new ShareClient(new Uri(containerEndpoint), credential));
            builder.Services.AddTransient<IFileShareService, FileShareService>();
            builder.Services.AddTransient<IFileProcessorService, FileProcessorService>();
        }
    }
}
