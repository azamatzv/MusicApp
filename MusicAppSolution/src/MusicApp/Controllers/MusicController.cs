using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Services;

namespace MusicApp.Controllers
{
    public class MusicController : ApiController
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

        [HttpGet("play/{fileName}")]
        public async Task<IActionResult> PlayMusic(string fileName)
        {
            try
            {
                var fileBytes = await _musicService.PlayMusicAsync(fileName);
                return File(fileBytes, "audio/mpeg", fileName);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadMusic(string fileName)
        {
            try
            {
                var fileBytes = await _musicService.DownloadMusicAsync(fileName);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
    }
}
