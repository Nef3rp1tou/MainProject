using MvcProject.Enums;

namespace MvcProject.Utilities
{
    public static class ResponseMessages
    {
        public static string GetDefaultMessage(CustomStatusCode statusCode)
        {
            return statusCode switch
            {
                CustomStatusCode.Success => "Request processed successfully.",
                CustomStatusCode.AlreadyProcessedTransaction => "Transaction has already been processed.",
                CustomStatusCode.InactiveToken => "Token is inactive.",
                CustomStatusCode.InsufficientBalance => "Insufficient balance for the requested operation.",
                CustomStatusCode.InvalidHash => "The provided hash is invalid.",
                CustomStatusCode.InvalidToken => "The provided token is invalid or expired.",
                CustomStatusCode.TransferLimit => "Transfer limit exceeded.",
                CustomStatusCode.UserNotFound => "User not found in the system.",
                CustomStatusCode.InvalidAmount => "The specified amount is invalid.",
                CustomStatusCode.DuplicatedTransactionId => "Transaction ID already exists.",
                CustomStatusCode.SessionExpired => "User session has expired. Please re-authenticate.",
                CustomStatusCode.InvalidCurrency => "The specified currency is not supported.",
                CustomStatusCode.InvalidRequest => "The request is invalid.",
                CustomStatusCode.InvalidIp => "Request originated from an unauthorized IP address.",
                CustomStatusCode.PendingTransactionExists => "A pending transaction already exists for this user.",
                CustomStatusCode.BankRejectedTransaction => "The transaction was rejected by the bank.",
                CustomStatusCode.GeneralError => "An unexpected error occurred. Please try again later.",
                _ => "An unknown error occurred."
            };
        }
    }
}
