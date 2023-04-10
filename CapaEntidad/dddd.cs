using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class dddd
    {

        public static void byToFile(byte[] fileInBytes, string nameFile) {

            byte[] fileBytes = fileInBytes; // bytes del archivo obtenidos de alguna fuente, como una base de datos o un archivo subido por el usuario
            string filePath = "wwwroot/inputFile/" + nameFile; // ruta donde se guardará el nuevo archivo

            // Escribir los bytes en el archivo especificado
            File.WriteAllBytes(filePath, fileBytes);


        }
    }
}
