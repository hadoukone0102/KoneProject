﻿using KoneProject.DTOs;
using KoneProject.Helpers;
using KoneProject.Interfaces;
using KoneProject.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace KoneProject.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // Constructor
        private readonly IBookServices _bookServices;

        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        [HttpGet]
        public async Task<ActionResult<ApiRessponse>> GetAll()
        {
            var books = await _bookServices.GetAllAsync();
            return Ok(new ApiRessponse<IEnumerable<BooksDto>>(true, "Liste des livres recupérer avec succès", books));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiRessponse>> GetById(int id)
        {
            var book = await _bookServices.GetByIdAsync(id);
            return book == null ? NotFound() : Ok(new ApiRessponse<IEnumerator<BooksDto>>(true,"Livre à été recupérer avec succès",book));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBooksDto book)
        {
            var newBooks = await _bookServices.CreateAsync(book);
            var response = new ApiRessponse<BooksDto>(
                    true,
                    "Livre créé avec succès",
                    newBooks
                );

            return CreatedAtAction(nameof(GetById), new { id = newBooks.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiRessponse<string>>> Delete(int id)
        {
            var result = await _bookServices.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new ApiRessponse<string>(false, "Livre introuvable"));
            }

            return Ok(new ApiRessponse<string>(true, "Livre supprimé avec succès"));
        }

    }
}
