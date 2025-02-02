using MvcProject.DTOs;
using MvcProject.Enums;
using MvcProject.Models;
using MvcProject.Utilities;

namespace MvcProject.Interfaces.IServices
{
    public interface ICallbackService
    {
        Task<ServiceResult> ProcessCallbackAsync(CallbackRequestModel callbackRequest, bool isWithdraw);
    }
}
