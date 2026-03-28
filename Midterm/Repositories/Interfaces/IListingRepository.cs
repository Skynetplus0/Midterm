using Midterm.Models;



namespace Midterm.Repositories.Interfaces
{
    public interface IListingRepository
    {
        Task AddAsync(Listing listing);
        Task<Listing?> GetByIdAsync(int id);
        Task<(List<Listing> Items, int TotalCount)> QueryAvailableListingsAsync(
            DateTime fromDate,
            DateTime toDate,
            int noOfPeople,
            string country,
            string city,
            int pageNumber,
            int pageSize);

        Task<bool> SaveChangesAsync();
    }
}