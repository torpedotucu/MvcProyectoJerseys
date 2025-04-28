using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcProyectoJerseys.Data;
using MvcProyectoJerseys.Helpers;
using MvcProyectoJerseys.Models;
using MvcProyectoJerseys.Services;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MvcProyectoJerseys.Repositories
{

    public class RepositoryCamisetas
    {
        private ServiceStorageBlobs service;

        public RepositoryCamisetas(ServiceStorageBlobs service) 
        {

            this.service=service;
        }
        
        public async Task SubirFichero(IFormFile file,Folders folders, string nombreArchivo)
        {
            await this.service.UploadBlobAsync("camisetas", nombreArchivo, file.OpenReadStream());
        }

        

        public string GenerateUniqueFileName(int idUser, IFormFile archivo)
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
            string extension = Path.GetExtension(archivo.FileName);
            string nombreUnico = $"{idUser}_{timeStamp}{extension}";
            return nombreUnico;
        }

        
    }
}
