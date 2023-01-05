using System;
using System.Diagnostics;

namespace FileReaderAPI.Helpers
{
    public class AzureHelper
    {

        public void ExecuteBatFile()
        {
            string filePath = "C:\\JoJo Maman Bébé\\NEXT\\azcopy.bat";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "cmd.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "/C " + filePath;

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                
            }
        }



        public void CopyDirectoryFromAzureStorage()
        {


            // Create the AzCopy command with the necessary parameters
            string command = "azcopy copy \"https://jmblandingdev.blob.core.windows.net/inbound/current\" \"C:\\danes\" --recursive\"";


            // Start the AzCopy process
            
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe", command);
            startInfo.ErrorDialog = true;
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            Process.Start(startInfo);
        }



    }
}