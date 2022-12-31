using Autofac;
using System.Reflection;
using try_reverse.service;

namespace try_reverse;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<PatchService>().As<IPatchService>().SingleInstance();
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var pluginTypes = types.Where(t => t.GetInterfaces().Contains(typeof(IPlugin))).ToList();
        foreach (var pluginType in pluginTypes)
        {
            builder.RegisterType(pluginType).As<IPlugin>();
        }
        var container = builder.Build();

        Console.WriteLine("Path to the folder containing NostaleClientX.exe :\n");
        string folder = Console.ReadLine()!;
        string path = Path.Combine(folder, "NostaleClientX.exe");
        if (!File.Exists(path))
        {
            Console.WriteLine("(error : main) unable to read the NosTaleClientX content");
            return;
        }
        string hexString;
        using (var stream = File.OpenRead(path))
        {
            using var reader = new BinaryReader(stream);
            var data = reader.ReadBytes((int)stream.Length);
            hexString = BitConverter.ToString(data).Replace("-", string.Empty);
            reader.Dispose();
        }

        using var scope = container.BeginLifetimeScope();
        var plugins = scope.Resolve<IEnumerable<IPlugin>>();
        foreach (var plugin in plugins)
        {
            plugin.Execute(folder, hexString);
        }
        scope.Dispose();
    }
}