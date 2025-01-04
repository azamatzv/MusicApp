using Microsoft.AspNetCore.Http;
using N_Tier.Core.Entities;

namespace N_Tier.Application.Services;

public interface IMusicService
{
    Task<Music> AddMusicAsync(IFormFile file, string name, Guid authorId, Guid genreId);
    Task<byte[]> PlayMusicAsync(string fileName);
    Task<byte[]> DownloadMusicAsync(string fileName);
}
