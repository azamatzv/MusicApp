using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs.GenreDtos;
using N_Tier.Core.Entities;

namespace MusicApp.Controllers
{
    public class GenreController : ApiControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAllGenresAsync();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreDto genreDto)
        {
            var createdGenre = await _genreService.AddGenreAsync(genreDto);
            return CreatedAtAction(nameof(GetAllGenres), new { name = createdGenre.Name }, createdGenre);
        }
    }
}
