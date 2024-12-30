using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs;

namespace MusicApp.Controllers;

[Route("api/genre")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;
    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var genres = await _genreService.GetAllAsync();
            return Ok(genres);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the genres.", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var genre = await _genreService.GetByIdAsync(id);
            return Ok(genre);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the genre.", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddGenre([FromBody] GenreDto genreDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var addedGenre = await _genreService.AddGenreAsync(genreDto);
            return Ok(new
            {
                message = "Genre successfully added.",
                genre = addedGenre
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while adding the genre.", details = ex.Message });
        }
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] GenreDto genreDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var updatedGenre = await _genreService.UpdateGenreAsync(id, genreDto);
            return Ok(new
            {
                message = "Genre successfully updated.",
                genre = updatedGenre
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the genre.", details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        try
        {
            var isDeleted = await _genreService.DeleteGenreAsync(id);
            return Ok(new
            {
                message = isDeleted ? "Genre successfully deleted." : "Genre not found."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the genre.", details = ex.Message });
        }
    }
}