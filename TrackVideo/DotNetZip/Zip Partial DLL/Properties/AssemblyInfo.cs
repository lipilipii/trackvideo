using System.Reflection;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Ionic's Zip Library")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("DotNetZip Library")]
[assembly: AssemblyCopyright("Copyright © Dino Chiesa 2007, 2008, 2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]


#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyDescription("a library for handling zip archives. http://www.codeplex.com/DotNetZip (Flavor=Debug)")]
#else
[assembly: AssemblyConfiguration("Retail")]
[assembly: AssemblyDescription("a library for handling zip archives. http://www.codeplex.com/DotNetZip (Flavor=Retail)")]
#endif


// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("dfd2b1f6-e3be-43d1-9b43-11aae1e901d8")]

[assembly:System.CLSCompliant(true)]

// workitem 4698
[assembly: AllowPartiallyTrustedCallers] 

[assembly: AssemblyVersion("1.8.4.22")]
[assembly: AssemblyFileVersion("1.8.4.22")]
