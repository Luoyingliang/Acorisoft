using System.Windows;
using Acorisoft;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]
[assembly: XmlnsDefinition("https://github.com/Luoyingliang/Acorisoft", "Acorisoft.Platform.Windows.Controls")]
[assembly: XmlnsDefinition("https://github.com/Luoyingliang/Acorisoft", "Acorisoft.Platform.Windows.Panels")]
[assembly: XmlnsDefinition("https://github.com/Luoyingliang/Acorisoft", "Acorisoft.Platform.Windows")]
[assembly: XmlnsDefinition("https://github.com/Luoyingliang/Acorisoft", "Acorisoft.Platform.Windows.Primitives")]
[assembly: XmlnsDefinition("https://github.com/Luoyingliang/Acorisoft", "Acorisoft.Platform.Windows.Services")]
[assembly: XmlnsDefinition("https://github.com/Luoyingliang/Acorisoft", "Acorisoft.Platform.Windows.Threadings")]
[assembly: XmlnsDefinition("https://github.com/Luoyingliang/Acorisoft", "Acorisoft.Platform.Windows.ViewModels")]
[assembly: XmlnsDefinition("https://github.com/Luoyingliang/Acorisoft", "Acorisoft.Platform.Windows.Views")]
[assembly: XmlnsDefinition("https://github.com/Luoyingliang/Acorisoft", "Acorisoft.Platform")]
//[assembly: GenerateDefinition("https://github.com/Luoyingliang/Acorisoft")]
[assembly: SkipGenerateDefinition("Acorisoft.Platform.Windows.Primitives")]
