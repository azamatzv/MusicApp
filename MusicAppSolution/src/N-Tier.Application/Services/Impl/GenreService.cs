using N_Tier.Core.DTOs;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<GenreDto> AddGenreAsync(GenreDto genreDto)
        {
            var genre = new Genre
            {
                Name = genreDto.Name
            };

            // Genre entity'sini qo'shish
            var createdGenre = await _genreRepository.AddAsync(genre);

            return new GenreDto
            {
                Name = createdGenre.Name
            };
        }

        public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
        {
            var genres = await _genreRepository.GetAllAsync(g => true);

            return genres.Select(genre => new GenreDto
            {
                Name = genre.Name
            });
        }

        public async Task<GenreDto> GetGenreAsync(Guid id)
        {
            var genre = await _genreRepository.GetFirstAsync(g => g.Id == id);

            return new GenreDto
            {
                Name = genre.Name
            };
        }
    }
}
