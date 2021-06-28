
using System;
using System.IO;
using System.Text;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NLog.Targets;

namespace AutomaticMEPTrace
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Startup : IExternalCommand
    {
        public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
             try
             {
                 AppDomain.CurrentDomain.UnhandledException += (sender, args) => { 
                     MessageBox.Show((args.ExceptionObject as Exception).ToString());
                 };
                     // create the Application object

                     // merge in your application resources
                     Application.Current.Resources.MergedDictionaries.Add(
                         Application.LoadComponent(
                             new Uri("AutomaticMEPTrace;component/Themes/Generic.xaml",
                                 UriKind.Relative)) as ResourceDictionary);
                 SetupNlog();
                 App.ThisApp!.ShowFormSeparateThread(commandData.Application);
                 return Result.Succeeded;
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.ToString());
                 return Result.Failed;
             }
        }

        private void SetupNlog()
        {
            var config = new NLog.Config.LoggingConfiguration();

            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = Path.Combine(path,@$"PluginsR1\Logs\logstash-{App.scriptName}.txt"),
                Layout  = @"${date:universalTime=true:format=yyyy-MM-ddTHH\:mm\:ss.fffZ}: |${level}| ${message}",
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveAboveSize = 7340032,
                AutoFlush = true,
                Encoding = Encoding.UTF8,
            };
            config.AddTarget(logfile);
            config.AddRuleForAllLevels(logfile);
            NLog.LogManager.Configuration = config;
        }
    }
}