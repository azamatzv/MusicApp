using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs;

namespace MusicApp.Controllers;

[Route("api/author")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var author = await _authorService.GetByIdAsync(id);
            return Ok(author);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the author.", details = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var authors = await _authorService.GetAllAsync();
            return Ok(authors);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the authors.", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAuthor([FromBody] AuthorDto authorDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var addedAuthor = await _authorService.AddAuthorAsync(authorDto);
            return Ok(new
            {
                message = "Author successfully added.",
                author = addedAuthor
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while adding the author.", details = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] AuthorDto authorDto)
    {
        try
        {
            var updatedAuthor = await _authorService.UpdateAuthorAsync(id, authorDto);
            return Ok(new
            {
                message = "Author successfully updated.",
                author = updatedAuthor
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the author.", details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(Guid id)
    {
        try
        {
            var result = await _authorService.DeleteAuthorAsync(id);
            return Ok(new { message = "Author successfully deleted." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the author.", details = ex.Message });
        }
    }
}