using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Countries;

namespace MovieAppAPI.Services.Countries;

public class CountryService : ICountryService {
    private readonly MovieDataContext _context;
    private readonly IMapper _mapper;

    public CountryService(MovieDataContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Country>?> GetAll() {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country?> GetById(Guid id) {
        var country = await _context.Countries.FindAsync(id);
        return country;
    }

    private async Task<bool> IsCountryExists(string countryName) {
        return await _context.Countries.AnyAsync(country => country.CountryName == countryName);
    }

    private async Task<bool> IsCountryExists(Guid id) {
        return await _context.Countries.AnyAsync(country => country.Id == id);
    }

    /// <exception cref="AlreadyExistsException"></exception>
    public async Task<Country> Create(CountryCreateModel countryCreateModel) {
        var isExists = await IsCountryExists(countryCreateModel.CountryName);
        if (isExists) {
            throw ExceptionHelper.CountryAlreadyExistsException(countryCreateModel.CountryName);
        }

        var newCountry = _mapper.Map<Country>(countryCreateModel);
        _context.Countries.Add(newCountry);
        await _context.SaveChangesAsync();

        return newCountry;
    }
    
    /// <exception cref="AlreadyExistsException"></exception>
    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Update(Guid id, CountryUpdateModel countryUpdateModel) {
        var isNewValueExists = await IsCountryExists(countryUpdateModel.CountryName);
        if (isNewValueExists) {
            throw ExceptionHelper.CountryAlreadyExistsException(countryUpdateModel.CountryName);
        }

        var dbCountry = await _context.Countries.FindAsync(id);

        if (dbCountry is null) {
            throw ExceptionHelper.CountryNotFoundException(id.ToString());
        }

        var updatedCountry = _mapper.Map(countryUpdateModel, dbCountry);
        _context.Countries.Update(updatedCountry);
        await _context.SaveChangesAsync();
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Delete(Guid id) {
        var isExists = await IsCountryExists(id);
        if (!isExists) {
            throw ExceptionHelper.CountryNotFoundException(id.ToString());
        }

        var country = new Country(id);
        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();
    }
}