using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Models;
using MvcProject.Utilities;

namespace MvcProject.Interfaces.IServices
{
    public interface ICallbackService
    {
        Task<CustomResponse> ProcessCallbackAsync(CallbackRequestModel callbackRequest, bool isWithdraw);
    }
}
