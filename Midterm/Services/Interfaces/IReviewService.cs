using Midterm.DTOs.Reviews;



namespace Midterm.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateReviewAsync(int guestId, CreateReviewRequestDto request);
    }
}