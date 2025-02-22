using Microsoft.AspNetCore.Hosting.Server;

namespace MvcProyectoJerseys.Helpers
{
    public enum Folders
    {
        Images,Avatar,Jerseys,Temporal
    }
    public class HelperPathProvider
    {
        private IServer server;
        private IWebHostEnvironment hostEnvironment;
        private IHttpContextAccessor accessor;

        public HelperPathProvider(IServer server, IWebHostEnvironment hostEnvironment, IHttpContextAccessor accessor)
        {
            this.server=server;
            this.hostEnvironment=hostEnvironment;
            this.accessor=accessor;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder==Folders.Avatar)
            {
                carpeta="avatar";
            }else if (folder==Folders.Images)
            {
                carpeta="images";
            }else if (folder==Folders.Jerseys)
            {
                carpeta="jersey";
            }
            else if (folder==Folders.Temporal)
            {
                carpeta="temp";
            }
            string roothPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(roothPath, carpeta, fileName);
            return path;
        }
        public string MapUrlPath(string fileName,Folders folder)
        {
            string carpeta = "";
            if (folder==Folders.Avatar)
            {
                carpeta="avatar";
            }
            else if (folder==Folders.Images)
            {
                carpeta="images";
            }
            else if (folder==Folders.Jerseys)
            {
                carpeta="jersey";
            }else if (folder==Folders.Temporal)
            {
                carpeta="temp";
            }
            var request = accessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/{carpeta}/{fileName}";
        }
    }

}
