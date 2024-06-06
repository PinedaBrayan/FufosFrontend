
namespace FufosFrontend.Utils;

public static class Utilities
{
    public static string SearchFile(string FileName)
    {
        try
        {
            var Assembly = typeof(Utilities).Assembly;

            var Path = Assembly.GetManifestResourceNames()
                .Single(x => x.EndsWith(FileName));

            using var stream = Assembly.GetManifestResourceStream(Path)!;
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
        }

        return string.Empty;
    }

    public static Type SearchModule(string Name)
    {
        var Module = typeof(Utilities).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.Name == "IBaseModule"))
                .Where(x => !x.Name.Contains("BaseModule"))
                .Single(x => x.FullName == $"FufosFrontend.Modules.{Name}Module");

        return Module;
    }
}