using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;

namespace MusicApp.Controllers
{
    public class MusicController : ApiControllerBase
    {
        private readonly IMusicService _musicService;

        public MusicController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadMusic(IFormFile file, string name, Guid authorId, Guid genreId)
        {
            try
            {
                var music = await _musicService.AddMusicAsync(file, name, authorId, genreId);
                return Ok(new
                {
                    Message = "Music uploaded successfully.",
                    Music = music
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("play/{musicId}")]
        public async Task<IActionResult> PlayMusic(Guid musicId)
        {
            if (musicId == Guid.Empty)
                return BadRequest(new { Error = "Invalid music ID." });

            try
            {
                var fileBytes = await _musicService.PlayMusicAsync(musicId);
                return File(fileBytes, "audio/mpeg");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("download/{musicId}")]
        public async Task<IActionResult> DownloadMusic(Guid musicId, [FromQuery] Guid accountId)
        {
            if (musicId == Guid.Empty || accountId == Guid.Empty)
                return BadRequest(new { Error = "Music ID and user ID are required." });

            try
            {
                string createdBy = User.Identity?.Name ?? "Anonymous";
                var fileBytes = await _musicService.DownloadMusicAsync(musicId, accountId, createdBy);
                return File(fileBytes, "application/octet-stream", $"{musicId}.mp3");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

    }
}
