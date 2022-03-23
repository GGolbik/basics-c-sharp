
using System.Reflection;

// Specifies the version of the assembly being attributed.
// 
// Major - increased when the feature set/API of the software changes significantly
// Minor - increased when notable changes are made, minor API changes or addition of new functionality
// Patch - increased when minor changes are made, typically bug fixes and improvements (though no API changes)
// Build - a unique ID/number that represents the build instance
// You can specify all the values or you can default the Patch and Build Numbers by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.*")]
// Specifies the version number given to file as in file system. It's displayed by Windows Explorer, and never used by .NET framework or runtime for referencing.
[assembly: AssemblyFileVersion("1.0.0")]
// Specifies the version you would use when talking to customers or for display on your website. This version can be a string, like '1.0 Release Candidate'.
[assembly: AssemblyInformationalVersion("1.0.0 Alpha")]
[assembly: AssemblyTitle(".Net Console Application")]
[assembly: AssemblyDescription("A simple dotnet console application.")]
[assembly: AssemblyCompany("GGolbik")]
[assembly: AssemblyCopyright("Copyright © GGolbik 2022")] 