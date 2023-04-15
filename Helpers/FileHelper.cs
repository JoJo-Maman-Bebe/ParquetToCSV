using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Helpers
{
    public class FileHelper
    {

        public void OrganiseParquetFiles(string folderPath)
        {
            string errorFolder = new ConfigurationHelper().GetAppSettingsValue("parquetFileErrorFolder");
            string baseFolder = new ConfigurationHelper().GetAppSettingsValue("parquetFileBaseFolder");

            string[] files = Directory.GetFiles(folderPath);

            foreach(string file in files)
            {
                if (file.EndsWith(".parquet"))
                    {

                    string fileName = Path.GetFileName(file);
                    string[] parts = fileName.Split('_');
                    string folderName = "";
                    for(int i = 0; i < parts.Length-1; i++ )
                    {
                        folderName += parts[i].ToUpper();

                    }
                    string destinationFolder = Path.Combine(baseFolder, folderName) + "\\";
                    try
                    {
                        File.Move(folderPath + fileName, destinationFolder + fileName);
                    }
                    catch
                    {
                        File.Move(folderPath + fileName, errorFolder + fileName);
                    }

                }
                
            }

        }


    }
}