using MvcPeliculasApiAWS.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MvcPeliculasApiAWS.Services
{
    public class PeliculasService
    {
        private string ApiUrl;
        private MediaTypeWithQualityHeaderValue header;

        public PeliculasService(IConfiguration config)
        {
            this.ApiUrl = config.GetValue<string>("ApiUrls:ApiPeliculas");
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                HttpResponseMessage response =
                    await client.GetAsync(this.ApiUrl + request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                    return default(T);
            }
        }

        public async Task<List<Pelicula>> GetPeliculasAsync()
        {
            string request = "api/peliculas";
            List<Pelicula> pelis = await this.CallApiAsync<List<Pelicula>>(request);
            return pelis;
        }

        public async Task<List<Pelicula>> GetPeliculasActorAsync(string actor)
        {
            string request = $"api/peliculas/actor/{actor}";
            List<Pelicula> pelis = await this.CallApiAsync<List<Pelicula>>(request);
            return pelis;
        }

        public async Task<Pelicula> FindPeliculaAsync(int id)
        {
            string request = $"api/peliculas/{id}";
            Pelicula peli = await this.CallApiAsync<Pelicula>(request);
            return peli;
        }

        public async Task<Pelicula> CreatePeliculaAsync
            (string genero, string titulo, string argumento,
            string foto, string actores, string youtube, int precio)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/peliculas";
                Pelicula peliNew = new Pelicula()
                {
                    IdPelicula = 0,
                    Actores = actores,
                    Argumento = argumento,
                    Foto = foto,
                    Genero = genero,
                    Precio = precio,
                    Titulo = titulo,
                    Youtube = youtube
                };
                string jsonData = JsonConvert.SerializeObject(peliNew);
                StringContent content = new StringContent(jsonData, this.header);
                HttpResponseMessage response = await client.PostAsync(this.ApiUrl + request, content);
                if (response.IsSuccessStatusCode)
                {
                    Pelicula peliCreated = await response.Content.ReadAsAsync<Pelicula>();
                    return peliCreated;
                }
                else
                    return null;
            }
        }

        public async Task UpdatePeliculaAsync
            (int idPelicula, string genero, string titulo, string argumento,
            string foto, string actores, string youtube, int precio)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/peliculas";
                Pelicula peliNew = new Pelicula()
                {
                    IdPelicula = idPelicula,
                    Actores = actores,
                    Argumento = argumento,
                    Foto = foto,
                    Genero = genero,
                    Precio = precio,
                    Titulo = titulo,
                    Youtube = youtube
                };
                string jsonData = JsonConvert.SerializeObject(peliNew);
                StringContent content = new StringContent(jsonData, this.header);
                HttpResponseMessage response = await client.PutAsync(this.ApiUrl + request, content);
            }
        }

        public async Task DeletePeliculaAsync
            (int idPelicula)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = $"api/peliculas/{idPelicula}";
                HttpResponseMessage response = await client.DeleteAsync(this.ApiUrl + request);
            }
        }

    }
}
