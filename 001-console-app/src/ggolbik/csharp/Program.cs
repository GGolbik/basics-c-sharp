using System;
using System.Reflection;

namespace ggolbik.csharp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");

      Program.printAssembly();
    }

    public static void printAssembly()
    {
      Program.printAssembly(Assembly.GetEntryAssembly());
    }

    public static void printAssembly(Assembly? assembly)
    {
      if (assembly == null)
      {
        return;
      }
      // Assembly
      Console.WriteLine($"Assembly: {assembly.ToString()}");
      Console.WriteLine($"FullName: {assembly.GetName().FullName}");
      Console.WriteLine($"Name: {assembly.GetName().Name}");
      Console.WriteLine($"ProcessorArchitecture: {assembly.GetName().ProcessorArchitecture}");

      // AssemblyVersion
      Console.WriteLine($"Version: {assembly.GetName().Version}");
      // AssemblyFileVersion
      Console.WriteLine($"FileVersion: {assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}");
      // AssemblyInformationalVersion
      Console.WriteLine($"InformationalVersion: {assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}");

      Console.WriteLine($"Title: {assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title}");
      Console.WriteLine($"Company: {assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company}");
      Console.WriteLine($"Description: {assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description}");
      Console.WriteLine($"Copyright: {assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright}");
    }
  }
}
