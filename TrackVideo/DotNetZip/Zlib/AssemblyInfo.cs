using System.Reflection;
using System.Security;
using System.Runtime.CompilerServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("Ionic's Managed Zlib")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("DotNetZlib Library")]
[assembly: AssemblyCopyright("Copyright © Dino Chiesa 2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]



#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyDescription("library for Deflate and ZLIB compression. http://www.codeplex.com/DotNetZip (Flavor=Debug)")]
#else
[assembly: AssemblyConfiguration("Retail")]
[assembly: AssemblyDescription("library for Deflate and ZLIB compression. http://www.codeplex.com/DotNetZip (Flavor=Retail)")]
#endif


[assembly: System.CLSCompliant(true)]

[assembly: AllowPartiallyTrustedCallers] 

[assembly: AssemblyVersion("1.8.4.22")]
[assembly: AssemblyFileVersion("1.8.4.22")]


