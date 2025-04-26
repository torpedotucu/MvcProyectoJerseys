using MvcProyectoJerseys.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
//using MvcProyectoJerseys.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.Extensions.Azure;
using NugetJerseyHubRGO.Models;

namespace MvcProyectoJerseys.Services
{
    public class ServiceCamisetas
    {
        
        private string UrlApi;
        private string uCam = "api/camisetas/";
        private string uUsu = "api/usuarios/";
        private string uCom = "api/comentarios/";
        private MediaTypeWithQualityHeaderValue Header;
        private IHttpContextAccessor contextAccessor;

        public ServiceCamisetas(IConfiguration configuration,IHttpContextAccessor contextAccessor)
        {
            this.UrlApi=configuration.GetValue<string>("ApiUrls:ApiCamisetas");
            this.Header=new MediaTypeWithQualityHeaderValue("application/json");
            this.contextAccessor=contextAccessor;
        }

        public async Task<string>GetTokenAsync(string userName,string password)
        {
            using( HttpClient client=new HttpClient())
            {
                string request = "api/auth/login";
                client.BaseAddress=new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    UserName = userName,
                    Password=password
                };
                string json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content
                        .ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //VAMOS A REALIZAR UNA SOBRECARGA DEL METODO
        //RECIBIENDO EL TOKEN
        private async Task<T> CallApiAsync<T>
            (string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }


        private string GetToken()
        {
            return this.contextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN")?.Value;
        }

        public async Task<List<Camiseta>> GetPublicacionesInicio()
        {
            string token = this.contextAccessor.HttpContext.User.FindFirst(x => x.Type=="TOKEN").Value;
            string request = uUsu+"publicacionesinicio";
            List<Camiseta> camisetas = await this.CallApiAsync<List<Camiseta>>(request, token);
            return camisetas;
        }
        public async Task<Usuario> GetUsuario()
        {
            string token = this.GetToken();
            string request = uUsu+"PerfilUsuario";
            Usuario usuario = await this.CallApiAsync<Usuario>(request, token);
            return usuario;
        }
        
        public async Task<Usuario>GetUsuarioCorreo(string correo)
        {
            string token = this.GetToken();
            string request = uUsu+"usuarioCorreo/"+correo.ToLower();
            Usuario usuario = await this.CallApiAsync<Usuario>(request, token);
            return usuario;
        }

        public async Task<List<Usuario>> GetAmigosUsuario()
        {
            string token = this.GetToken();
            string request = uUsu+"amigos";
            List<Usuario> amigos = await this.CallApiAsync<List<Usuario>>(request, token);
            return amigos;
        }

        public async Task<Camiseta> GetCamiseta(int idCamiseta)
        {
            string token = this.GetToken();
            string request = uCam+"camiseta/"+idCamiseta;
            Camiseta camiseta = await this.CallApiAsync<Camiseta>(request, token);
            return camiseta;
        }

        public async Task<int> SubirCamiseta(CamisetaCreateDTO camiseta)
        {
            string token = this.GetToken();
            string request = "";
            //OTRO METODO PARA LAS PETICIONES PUT,POST...
        }

    }
}
