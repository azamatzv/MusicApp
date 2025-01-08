using Microsoft.AspNetCore.Http;
using N_Tier.Core.Entities;

namespace N_Tier.Application.Services;

public interface IMusicService
{
    Task<Music> AddMusicAsync(IFormFile file, string name, Guid authorId, Guid genreId);
    Task<byte[]> PlayMusicAsync(Guid musicId);
    Task<byte[]> DownloadMusicAsync(Guid musicId, Guid accountId, string createdBy);
}
