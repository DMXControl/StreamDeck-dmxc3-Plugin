using LumosLIB;
using LumosLIB.Kernel;
using System.Reflection;
using System.Runtime.InteropServices;

// Allgemeine Informationen über eine Assembly werden über die folgenden
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die einer Assembly zugeordnet sind.
[assembly: AssemblyTitle("StreamDeck")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(AssemblyConstants.PROGRAM_COMPANY)]
[assembly: AssemblyProduct("StreamDeck-Plugin")]
[assembly: AssemblyCopyright("Copyright © " + AssemblyConstants.PROGRAM_COMPANY + " " + AssemblyConstants.PROGRAM_C_YEAR)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Durch Festlegen von ComVisible auf FALSE werden die Typen in dieser Assembly
// für COM-Komponenten unsichtbar.  Wenn Sie auf einen Typ in dieser Assembly von
// COM aus zugreifen müssen, sollten Sie das ComVisible-Attribut für diesen Typ auf "True" festlegen.
[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird
[assembly: Guid("1025465d-6fc5-494f-a41b-469ce2f46209")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder Standardwerte für die Build- und Revisionsnummern verwenden,
// indem Sie "*" wie unten gezeigt eingeben:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(LumosConstants.PROGRAM_VERSION_ASSEMBLY)]
[assembly: AssemblyFileVersion(LumosConstants.PROGRAM_VERSION_FILE)]
[assembly: AssemblyInformationalVersion(LumosConstants.PROGRAM_VERSION_INFO)]
