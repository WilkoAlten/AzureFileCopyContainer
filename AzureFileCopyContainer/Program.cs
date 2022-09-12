using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureFileCopyContainer
{
    internal class Program
    {

        /// <summary>
        /// Copy all files between two containers
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Start moving");

            Program.startCopy();

            Console.WriteLine("End moving");
        }

        private static async void startCopy()
        {
            string connection = "<StorageConnectionString>"; //Replace the tag <StorageConnectionString> with your own connectionstring to Azure Storage Account
            BlobServiceClient client = new BlobServiceClient(connection);


            BlobContainerClient sourceBlobContainer = client.GetBlobContainerClient("telemetry");
            BlobContainerClient destBlobContainer = client.GetBlobContainerClient("telemetry-archive");

            // Iterate through the blobs in a container
            List<BlobItem> segment = sourceBlobContainer.GetBlobs().ToList();
            int i = 0; 

            foreach (BlobItem blobItem in segment)
            {
                i++;
                Console.WriteLine($"{i} - Blob: {blobItem.Name}");

                BlobClient sourceBlob = sourceBlobContainer.GetBlobClient(blobItem.Name);
                BlobClient destBlob = destBlobContainer.GetBlobClient(blobItem.Name);

                //skip the blob if it already exists in the destination
                if (destBlob.Exists())
                {
                    Console.WriteLine($"{i} - Blob skipped: {blobItem.Name}");
                }
                else { 
                    CopyFromUriOperation ops = destBlob.StartCopyFromUri(sourceBlob.Uri);

                    
                    Console.WriteLine($"{i} - Blob: {destBlob.Name} Complete");
                }

            }
        }

       
    }
}
