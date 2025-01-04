using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Application.Services;

public interface IGenreService

{
    Task<GenreDto> AddGenreAsync(GenreDto genreDto);
    Task<IEnumerable<GenreDto>> GetAllGenresAsync();
    Task<GenreDto> GetGenreAsync(Guid id);
}
