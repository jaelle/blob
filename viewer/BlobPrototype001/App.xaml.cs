using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Reflection;

namespace BlobPrototype001
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            String applicationPath, tempPath;

            applicationPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (applicationPath.Substring(0, 6) == "file:\\")
            {
                applicationPath = applicationPath.Remove(0, 6);
            }
            tempPath = applicationPath + "\\tmp";

            if (System.IO.Directory.Exists(tempPath))
            {
                //remove everything in this directory
                string[] files;

                files = System.IO.Directory.GetFiles(tempPath);
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                }


                string[] directories = System.IO.Directory.GetDirectories(tempPath);

                foreach (string directory in directories)
                {
                    files = System.IO.Directory.GetFiles(directory);

                    foreach (string file in files)
                    {
                        System.IO.File.Delete(file);
                    }

                    System.IO.Directory.Delete(directory);
                }
            }
        }

    }
}
