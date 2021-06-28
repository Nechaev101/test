

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Autodesk.Revit.UI;
using AutomaticMEPTrace.Views;

namespace AutomaticMEPTrace
{
class App
    {
        /// <summary>
        /// Plugin name
        /// </summary>
        public static readonly string scriptName = "АвтоматическаяТрассировка";

        /// <summary>
        /// Plugin version
        /// </summary>
        public static readonly string
            version = "[" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "]";

        // class instance
        private static App? _thisApp;

        public static App? ThisApp
        {
            get
            {
                if ( _thisApp == null )
                {
                    _thisApp = new App();
                }

                return _thisApp;
            }
        }

        // ModelessForm instance
        private MainWindow? _mMyForm;

        // Separate thread to run Ui on
        private Thread? _uiThread;

        /// <summary>
        /// This is the method which launches the WPF window in a separate thread, and injects any methods that are
        /// wrapped by ExternalEventHandlers. This can be done in a number of different ways, and
        /// implementation will differ based on how the WPF is set up.
        /// </summary>
        /// <param name="uiapp">The Revit UIApplication within the add-in will operate.</param>
        public void ShowFormSeparateThread(UIApplication uiapp)
        {
            try
            {
                // If we do not have a thread started or has been terminated start a new one
                if ( !(_uiThread is null) && _uiThread.IsAlive )
                    return;
                //EXTERNAL EVENTS WITH ARGUMENTS

                _uiThread = new Thread(() =>
                {
                    try
                    {
                        SynchronizationContext.SetSynchronizationContext(
                            new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
                        // The dialog becomes the owner responsible for disposing the objects given to it.
                        _mMyForm = new MainWindow();
                        _mMyForm.Closed += (s, e) => Dispatcher.CurrentDispatcher.InvokeShutdown();
                        _mMyForm.Show();
                        Dispatcher.Run();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        // @TODO LOG "ex"
                    }
                });

                _uiThread.SetApartmentState(ApartmentState.STA);
                _uiThread.IsBackground = true;
                _uiThread.Start();
            }
            catch (Exception ex)
            {
                // @TODO Log "ex"
                MessageBox.Show(ex.ToString());
            }
        }

    }
}