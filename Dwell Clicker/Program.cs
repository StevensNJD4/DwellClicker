using System.Threading;
using System.Diagnostics;

namespace Dwell_Clicker
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string mutexName = "DWELLNT";
            Mutex mutex;

            try
            {
                // Attempt to create a new named mutex.
                mutex = Mutex.OpenExisting(mutexName);
                // If we reach this point, the mutex already exists, and another instance is already running.
                return;
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                // If we reach this point, the mutex does not exist, and this is the first instance.
                mutex = new Mutex(true, mutexName);
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());

            // Release the mutex.
            mutex.ReleaseMutex();
        }
    }
}