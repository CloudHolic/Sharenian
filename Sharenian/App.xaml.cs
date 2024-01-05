using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Sharenian.Configs;
using System.Windows.Threading;
using Sharenian.Utils;

namespace Sharenian;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public static FrozenSet<ApiInfo>? ApiInfos { get; set; }

    public App()
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith("Keys.yaml"));

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            return;

        using var reader = new StreamReader(stream);
        var config = reader.ReadToEnd();

        ApiInfos = deserializer.Deserialize<List<ApiInfo>>(config).ToFrozenSet();
    }

    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var logger = Logger.Instance;
        logger.Error(e.Exception);
        e.Handled = true;
    }
}
