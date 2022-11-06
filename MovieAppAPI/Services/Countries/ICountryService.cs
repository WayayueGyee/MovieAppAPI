using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Countries;

namespace MovieAppAPI.Services.Countries;

public interface ICountryService {
    Task<IEnumerable<Country>?> GetAll();
    Task<Country?> GetById(Guid id);
    Task<Country?> GetByCountryName(string countryName);

    /// <exception cref="AlreadyExistsException"></exception>
    Task<Country> Create(CountryCreateModel countryCreateModel);

    /// <exception cref="AlreadyExistsException"></exception>
    /// <exception cref="RecordNotFoundException"></exception>
    Task Update(Guid id, CountryUpdateModel countryUpdateModel);

    /// <exception cref="RecordNotFoundException"></exception>
    Task Delete(Guid id);
}