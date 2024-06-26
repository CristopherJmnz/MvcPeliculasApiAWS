﻿using Microsoft.AspNetCore.Mvc;
using MvcPeliculasApiAWS.Models;
using MvcPeliculasApiAWS.Services;

namespace MvcPeliculasApiAWS.Controllers
{
    public class PeliculasController:Controller
    {
        private readonly PeliculasService service;
        private ServiceStorageS3 serviceS3;
        public PeliculasController(PeliculasService service, ServiceStorageS3 serviceS3)
        {
            this.service = service;
            this.serviceS3 = serviceS3;
        }

        public async Task<IActionResult> Index()
        {
            List<Pelicula>pelis=await this.service.GetPeliculasAsync();
            return View(pelis);
        }

        public IActionResult PeliculasActor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PeliculasActor(string actor)
        {
            List<Pelicula> pelis = await this.service.GetPeliculasActorAsync(actor);
            return View(pelis);
        }

        public async Task<IActionResult> Details(int id)
        {
            Pelicula peli = await this.service.FindPeliculaAsync(id);
            return View(peli);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Pelicula peli,IFormFile foto)
        {
            Pelicula peliNew = await this.service.CreatePeliculaAsync
                (peli.Genero, peli.Titulo, peli.Argumento,
                foto.FileName, peli.Actores, peli.Youtube, peli.Precio);
            using (Stream stream = foto.OpenReadStream())
            {
                await this.serviceS3.UploadFileAsync
                    (foto.FileName, stream);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Pelicula peli = await this.service.FindPeliculaAsync(id);
            return View(peli);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Pelicula peli)
        {
            await this.service.UpdatePeliculaAsync
                (peli.IdPelicula,peli.Genero, peli.Titulo, peli.Argumento,
                peli.Foto, peli.Actores, peli.Youtube, peli.Precio);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.service.DeletePeliculaAsync(id);
            return RedirectToAction("Index");
        }
    }
}
