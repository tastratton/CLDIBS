using helper.Time;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using Microsoft.AspNetCore.DataProtection;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.CommandLine.NamingConventionBinder;
//using Microsoft.Extensions.Logging;
//using static System.CommandLine.Help.HelpBuilder;
//using System.Collections.Generic;
using System.Collections.Immutable;
#if DEBUG
using System.Diagnostics;
#endif


namespace helper;

internal class Program
{
    private static void DebugLog(Object? message) 
    {
#if DEBUG
        if(message != null) Debug.WriteLine(message);
#endif
    }
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("My Helper App");
        rootCommand.AddCommand(new TimeCommand());
        var cmdlineBuilder = new CommandLineBuilder(rootCommand);

        /*
        Action<IConfigurationBuilder> getConfig = configBuilder =>
        {
            configBuilder.AddJsonFile("appsettings.json", true, false);
        };
        */

        //IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
        //IHost host = hostBuilder.Build();
        Func<string[], IHostBuilder> hostBuilderFactory = funcArgArr =>
        {
            var h = Host.CreateDefaultBuilder(funcArgArr);
            // could do some things with h here
            return h;
            /*
             * https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.host.createdefaultbuilder?view=net-8.0#microsoft-extensions-hosting-host-createdefaultbuilder(system-string())
            The following defaults are applied to the returned HostBuilder:

            Set the ContentRootPath to the result of GetCurrentDirectory().
            Load host IConfiguration from "DOTNET_" prefixed environment variables.
            Load host IConfiguration from supplied command line arguments.
            Load app IConfiguration from 'appsettings.json' and 'appsettings.[EnvironmentName].json'.
            Load app IConfiguration from User Secrets when EnvironmentName is 'Development' using the entry assembly.
            Load app IConfiguration from environment variables.
            Load app IConfiguration from supplied command line arguments.
            Configure the ILoggerFactory to log to the console, debug, and event source output.
            Enable scope validation on the dependency injection container when EnvironmentName is 'Development'.

            */
        };

        /*
        Action<IHostBuilder> configureHost = hostBuilder =>
                    {
                        ;//todo
                    };
        */


        var parser = cmdlineBuilder
            .UseHost(hostBuilderFactory, builder =>
                {
                    //IConfiguration configFromCreateDefault = Host.CreateDefaultBuilder(args).
                    builder
                        .ConfigureHostConfiguration((IConfigurationBuilder configBuilder) =>
                        {
                            // only working if manually added
                            // CONCLUDED - PROBABLY SHOULDN'T DO THIS.
                            // INSTEAD THIS SHOULD USUALLY BE CONFIGURED VIA DOTNET PREFIXED VARIABLES.
                            // AKA WHY WE CODE DIS?
                            /*var appconfig = configBuilder
                                .AddJsonFile("appsettings.json", true, true)
                                .Build();
                            if (appconfig["HostConfig:somesetting"] == "true")
                            {
                                DebugLog("ConfigureHost somesetting is true");
                            };
                            */

                            //https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.host.createdefaultbuilder?view=net-8.0

                            var config = configBuilder.Build();
#if DEBUG
                            DebugLog("************** config in ConfigureHostConfiguration ****************");
                            foreach (var kvp in config.AsEnumerable().ToImmutableSortedDictionary()) { DebugLog($"{kvp.Key}={kvp.Value}"); };
#endif


                        })
                        ;
                    builder
                        .ConfigureAppConfiguration((ctx, configBuilder) =>
                        {
                            //IHostEnvironment env = ctx.HostingEnvironment;
                            //IConfiguration passedconfig = ctx.Configuration;  //THIS IS FROM LAST STAGE
                            var config = configBuilder.Build();
#if DEBUG
                            DebugLog("************** config in ConfigureAppConfiguration *****************");
                            foreach (var kvp in config.AsEnumerable().ToImmutableSortedDictionary()) { DebugLog($"{kvp.Key}={kvp.Value}"); };
                            if (config["HostConfig:somesetting"] == "true")
                            {
                                DebugLog("ConfigureApp somesetting is true");
                            };
#endif

                        })
                        ;
                    builder.ConfigureServices((ctx, services) =>
                    {
                        //IHostEnvironment env = ctx.HostingEnvironment;
                        IConfiguration config = ctx.Configuration; // <= last stages config

                        //var config = configBuilder.Build(); //we don't do this here because not passing config... config didn't change
                        //DebugLog("************** config in ConfigureServices *****************");
                        /*
                        foreach (var thing in config.AsEnumerable().ToImmutableSortedDictionary()) { DebugLog(thing); };
                        if (config["HostConfig:somesetting"] == "true")
                        {
                            DebugLog("ConfigureApp somesetting is true");
                        };
                        */
#if DEBUG
                        if (config["HostConfig:somesetting"] == "true")
                        {
                            DebugLog("ConfigureServices somesetting is true");
                        };
#endif

                        Program.ConfigureServices(services);

                    })
                        .UseCommandHandler<TimeCommand, TimeCommandHandler>();
                    ;
                })
            .UseDefaults()
            .Build();


        return await parser.InvokeAsync(args);

    }
    private static void ConfigureServices(IServiceCollection services)
    {
        //services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder); ;

        //services.AddOptions<TimeCommandOptions>("SiteTitle")
        /*
        services
            .AddOptions<TimeOptions>() 
            // todo 1 - do options pattern for DI somewhere else later, remove this
            //.Configure(options => options.)
            //.Bind(Configuration.GetSection(SettingsOptions.ConfigurationSectionName))
            //.ValidateDataAnnotations()
            ;
        */
        services.TryAddSingleton<TimeProvider>(TimeProvider.System);
    }

}
