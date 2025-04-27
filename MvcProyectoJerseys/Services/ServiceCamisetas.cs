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
        //METODO GPT
        private async Task<T> CallApiAsync<T>(string request, HttpMethod method, object body = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                HttpRequestMessage message = new HttpRequestMessage(method, request);

                if (body != null)
                {
                    string json = JsonConvert.SerializeObject(body);
                    message.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await client.SendAsync(message);
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
        private async Task<T> CallApiAsync<T>(string request, string token, HttpMethod method, object body = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpRequestMessage message = new HttpRequestMessage(method, request);

                if (body != null)
                {
                    string json = JsonConvert.SerializeObject(body);
                    message.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await client.SendAsync(message);
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
            //return this.contextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN")?.Value;
            string token = this.contextAccessor.HttpContext.Session.GetString("TOKEN");
            return token;
        }

        public async Task<List<Camiseta>> GetPublicacionesInicio()
        {
            string token = this.GetToken();
            string request = uUsu+"publicacionesinicio";
            List<Camiseta> camisetas = await this.CallApiAsync<List<Camiseta>>(request, token);
            return camisetas;
        }
        public async Task<Usuario> GetUsuario()
        {
            string token = this.GetToken();
            string request = uUsu+"PerfilUsuario";
            Usuario usuario = await this.CallApiAsync<Usuario>(request, token,HttpMethod.Get);
            return usuario;
        }
        public async Task<Usuario> GetUsuario(int idUsuario)
        {
            string token = this.GetToken();
            string request = uUsu+"GetUsuario/"+idUsuario;
            Usuario usuario = await this.CallApiAsync<Usuario>(request, token, HttpMethod.Get);
            return usuario;
        }

        public async Task<Usuario>GetUsuarioCorreo(string correo)
        {
            string token = this.GetToken();
            string request = uUsu+"usuarioCorreo/"+correo.ToLower();
            Usuario usuario = await this.CallApiAsync<Usuario>(request, token);
            return usuario;
        }

        public async Task<List<Camiseta>> GetCamisetasUsuario()
        {
            string token = this.GetToken();
            string request = uCam+"camisetasUsuario";
            List<Camiseta> camisetas = await this.CallApiAsync<List<Camiseta>>(request, token, HttpMethod.Get);
            return camisetas;
        }

        public async Task<List<Camiseta>> GetCamisetasUsuario(int idUsuario)
        {
            string token = this.GetToken();
            string request = uCam+"CamisetasUsuarioExterno/"+idUsuario;
            List<Camiseta> camisetas = await this.CallApiAsync<List<Camiseta>>(request, token, HttpMethod.Get);
            return camisetas;
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
            string request = uCam+"InsertarCamiseta";
            int idCamiseta = await this.CallApiAsync<int>(request, token, HttpMethod.Post, camiseta);
            return idCamiseta;
        }

        public async Task ModificarCamiseta(CamisetaUpdateDTO camiseta)
        {
            string token = this.GetToken();
            string request = uCam+"actualizarcamiseta";
            await this.CallApiAsync<object>(request, token, HttpMethod.Put, camiseta);

        }
        public async Task EditarPerfil(UsuarioUpdateDTO usuario)
        {
            string token = this.GetToken();
            string request = uUsu+"EditarPerfil";
            await this.CallApiAsync<object>(request, token, HttpMethod.Put, usuario);

        }

        public async Task<List<Comentario>> GetComentariosAsync(int idCamiseta)
        {
            string token = this.GetToken();
            string request = uCom+"ComentariosCamiseta/"+idCamiseta;
            List<Comentario>comentarios=await this.CallApiAsync<List<Comentario>>(request, token, HttpMethod.Get);
            return comentarios;
        }

        public async Task<CamisetaComentarios>DetalleCamiseta(int idCamiseta)
        {
            string token = this.GetToken();
            string request = uCam+"detallescamiseta/"+idCamiseta;
            CamisetaComentarios camiseta = await this.CallApiAsync<CamisetaComentarios>(request, token, HttpMethod.Get);
            return camiseta;
        }


        public async Task Comentar(ComentarioDTO comentario)
        {
            string token = this.GetToken();
            string request = uCom+"comentar";
            await this.CallApiAsync<object>(request, token, HttpMethod.Post, comentario);
        }

        public async Task<List<Pais>> GetPaisesAsync()
        {
            string request = "api/paises/paises";
            List<Pais> paises = await this.CallApiAsync<List<Pais>>(request, HttpMethod.Get);
            return paises;
        }
        

        public async Task<int>CreateUsuario(UsuarioCreateDTO usuario)
        {
            string request = uUsu+"CreateUsuario";
            int idUsuario = await this.CallApiAsync<int>(request, HttpMethod.Post, usuario);
            return idUsuario;
        }

        public async Task<Usuario>FindUsuarioAmistadCode(string friendCode)
        {
            string token = this.GetToken();
            string request = uUsu+"amigo/"+friendCode;
            Usuario usuario = await this.CallApiAsync<Usuario>(request, token, HttpMethod.Get);
            return usuario;
        }
        public async Task<bool> AreAlreadyFriends(int idUsuario)
        {
            string token = this.GetToken();
            string request = uUsu+"arefriends/"+idUsuario;
            bool loson = await this.CallApiAsync<bool>(request, token, HttpMethod.Get);
            return loson;
        }

        public async Task SetAmistad(int userB)
        {
            string token = this.GetToken();
            string request = uUsu+"SetAmistad"+userB;
            await this.CallApiAsync<object>(request, token, HttpMethod.Post);
        }

        //VER AMIGOS DEL USUARIO PROPIETARIO DEL TOKEN
        public async Task<List<Usuario>>GetListaAmigosAsync()
        {
            string token = this.GetToken();
            string request = uUsu+"Amigos";
            List<Usuario> usuarios = await this.CallApiAsync<List<Usuario>>(request, token, HttpMethod.Get);
            return usuarios;
        }
        //VER AMIGOS DE OTRO USUARIO
        public async Task<List<Usuario>> GetListaAmigosAsync(int idUsuario)
        {
            string token = this.GetToken();
            string request = uUsu+"AmigosUsuario/"+idUsuario;
            List<Usuario> usuarios = await this.CallApiAsync<List<Usuario>>(request, token, HttpMethod.Get);
            return usuarios;
        }


        public async Task InsertEtiquetas(List<string>etiquetas,int idCamiseta)
        {
            string token = this.GetToken();
            string request = uCam+"InsertEtiquetas/"+idCamiseta;
            await this.CallApiAsync<object>(request, token, HttpMethod.Post, etiquetas);

        }

        public async Task<List<Etiqueta>>GetEtiquetas(int idCamiseta)
        {
            string token = this.GetToken();
            string request = uCam+"etiquetascamiseta/"+idCamiseta;
            List<Etiqueta> etiquetas = await this.CallApiAsync<List<Etiqueta>>(request, token, HttpMethod.Get);
            return etiquetas;
        }
        public async Task DeleteCamiseta(int idCamiseta)
        {
            string token = this.GetToken();
            string request = uCam+"EliminarCamiseta/"+idCamiseta;
            await this.CallApiAsync<object>(request, token, HttpMethod.Delete);
        }
    }
}
