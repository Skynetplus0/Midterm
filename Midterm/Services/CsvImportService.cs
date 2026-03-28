using CsvHelper;
using CsvHelper.Configuration;
using Midterm.DTOs.Listings;
using Midterm.Models;
using Midterm.Repositories.Interfaces;
using Midterm.Services.Interfaces;
using System.Globalization;



using Microsoft.AspNetCore.Http;





namespace Midterm.Services
{
    public class CsvImportService : ICsvImportService
    {
        private readonly IListingRepository _listingRepository;

        public CsvImportService(IListingRepository listingRepository)
        {
            _listingRepository = listingRepository;
        }

        public async Task<CsvImportResponseDto> ImportListingsAsync(IFormFile file, int hostId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("CSV file is required.");
            }

            var response = new CsvImportResponseDto();

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            var records = csv.GetRecords<CsvListingRowDto>().ToList();

            response.TotalRows = records.Count;

            var rowNumber = 1;

            foreach (var row in records)
            {
                try
                {
                    if (row.NoOfPeople <= 0)
                        throw new Exception("NoOfPeople must be greater than 0.");

                    if (string.IsNullOrWhiteSpace(row.Country))
                        throw new Exception("Country is required.");

                    if (string.IsNullOrWhiteSpace(row.City))
                        throw new Exception("City is required.");

                    if (row.Price <= 0)
                        throw new Exception("Price must be greater than 0.");

                    var listing = new Listing
                    {
                        HostId = hostId,
                        Title = string.IsNullOrWhiteSpace(row.Title)
                            ? $"Imported Listing {rowNumber}"
                            : row.Title.Trim(),
                        Description = row.Description,
                        NoOfPeople = row.NoOfPeople,
                        Country = row.Country.Trim(),
                        City = row.City.Trim(),
                        Price = row.Price,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _listingRepository.AddAsync(listing);
                    response.SuccessCount++;
                }
                catch (Exception ex)
                {
                    response.FailedCount++;
                    response.Errors.Add($"Row {rowNumber}: {ex.Message}");
                }

                rowNumber++;
            }

            if (response.SuccessCount > 0)
            {
                await _listingRepository.SaveChangesAsync();
            }

            response.Message = $"CSV processing completed. Successful: {response.SuccessCount}, Failed: {response.FailedCount}";
            return response;
        }
    }
}