using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;

namespace MusicApp.Controllers
{
    public class AuthorController : ApiController
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDto authorDto)
        {
            var createdAuthor = await _authorService.AddAuthorAsync(authorDto);
            return CreatedAtAction(nameof(GetAllAuthors), new { name = createdAuthor.Name }, createdAuthor);
        }
    }
}
