
using Spire.Doc;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace archivos.Utilidades
{
    public class FileUtil
    {

        public static string biteToFile(byte[] fileInBytes, string nameFile)
        {
            

            string fileExtension = Path.GetExtension(nameFile);     

            byte[] fileBytes = fileInBytes; 
            string filePath = "wwwroot/outputFile/" + nameFile;

            File.WriteAllBytes(filePath, fileBytes);

            if (fileExtension == ".docx")
            {

                nameFile = nameFile.Replace("docx", "pdf");

                string wordFilePath = filePath;
                string pdfFilePath = "wwwroot/outputFile/" + nameFile;

                Document document = new Document();
                document.LoadFromFile(wordFilePath);
                document.SaveToFile(pdfFilePath, FileFormat.PDF);

            }

          


            return nameFile;


        }


    }
}
