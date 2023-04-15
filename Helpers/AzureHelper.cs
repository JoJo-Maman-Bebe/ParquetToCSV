
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Specialized;
using System.IO;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Azure.Identity;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage;

namespace FileReaderAPI.Helpers
{

    public class AzureHelper
    {


        public static void DownloadParquetFiles(string sasToken, string containerUrl)
        {
            // Create a blob endpoint for the container using the SAS token
            Uri containerUri = new Uri(containerUrl + "?" + sasToken);

            // Create a blob client for interacting with the blob service
            CloudBlobClient blobClient = new CloudBlobClient(containerUri);

            // Get the container reference
            CloudBlobContainer container = new CloudBlobContainer(containerUri);
            // Get the blobs in the container
            foreach (IListBlobItem item in container.ListBlobs())
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    if (blob.Name.EndsWith(".parquet"))
                    {
                        // Download the blob to a local file
                        string localFilePath = Path.Combine("Downloads", blob.Name);
                        blob.DownloadToFile(localFilePath, FileMode.Create);
                        Console.WriteLine($"Downloaded {blob.Name} to {localFilePath}");
                    }
                }
                if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory blobDirectory = (CloudBlobDirectory)item;
                    foreach (IListBlobItem blob in blobDirectory.ListBlobs())
                    {
                        if (blob.GetType() == typeof(CloudBlockBlob))
                        {
                            CloudBlockBlob blob2 = (CloudBlockBlob)blob;
                            blob2.DownloadToFile(blob2.Name.Replace("current/", new ConfigurationHelper().GetAppSettingsValue("parquetFileBaseFolder2")), FileMode.Create);
                        }

                    }

                }
            }
        }





        public static void DownloadParquetFiles2(string accountName, string containerName, string clientId, string clientSecret, string tenantId)
        {

            //account name = "BIS-JmbLanding-PROD"
            //containername = "inbound"
           // SPN Name -BIS - JmbLanding - PROD
            //SPN ID -1bb3537c - 6906 - 443f - ae7f - a2d53f7e07d5
//          Secret ID -516598bb - 9f8a - 422c - a6c5 - 58449d72f905

            var clientCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            // Create a blob service client using the account name and token credential
            var blobServiceClient = new BlobServiceClient(new Uri($"https://{accountName}.blob.core.windows.net/"), clientCredential);

            // Get a reference to the container and directory
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            // Download the parquet files

            foreach (var blob in containerClient.GetBlobs())
            {
                if (blob.Name.EndsWith(".parquet"))
                {
                    // Download the blob to a local file
                    string localFilePath = Path.Combine("Downloads", blob.Name);
                    var blobClient = containerClient.GetBlobClient(blob.Name);
                    blobClient.DownloadTo(localFilePath);
                    Console.WriteLine($"Downloaded {blob.Name} to {localFilePath}");
                }
            }
        }
    




public void GetFirstParquetFile(string containerUrl, string sasToken)
        {
            //string containerUrl = "https://jmblandingdev.blob.core.windows.net/inbound";
            //string sasToken = "sv=2021-06-08&ss=bfqt&srt=sco&sp=rlx&se=2023-10-24T18:07:54Z&st=2022-10-24T10:07:54Z&sip=188.227.240.140&spr=https&sig=pnIEu9dBNEveH6IMV98YDcpKnWKTEb%2BW73amLyOebXo%3D";



            DownloadParquetFiles(sasToken, containerUrl);

            FileHelper fileHelper = new FileHelper();

            fileHelper.OrganiseParquetFiles(new ConfigurationHelper().GetAppSettingsValue("parquetFileBaseFolder"));
        }

        public void GetNewParquetFiles()
        {
            //string containerUrl = "https://jmblandingdev.blob.core.windows.net/inbound";
            //string sasToken = "sv=2021-06-08&ss=bfqt&srt=sco&sp=rlx&se=2023-10-24T18:07:54Z&st=2022-10-24T10:07:54Z&sip=188.227.240.140&spr=https&sig=pnIEu9dBNEveH6IMV98YDcpKnWKTEb%2BW73amLyOebXo%3D";

            //string accountString = @"42M8Q~JmFL6GOI.~9cODIgBTyVguzhyOE4Sd7cT8";

            //accountName, containerName, clientId,clientSecret,tenantId
            // await DownloadParquetFiles2("BIS-JmbLanding-PROD", "inbound","42M8Q~JmFL6GOI.~9cODIgBTyVguzhyOE4Sd7cT8", ");

            DownloadParquetFiles2("jmblandingdev", "inbound", "f849cbb6-c572-40af-a896-377286ffb794", "rQh8Q~MPAGFed_mTDIwCKvv_yggk93Is~PC2baQT", "f009f285-5242-433a-9365-daa1edf145c3");
        }


    }
}
