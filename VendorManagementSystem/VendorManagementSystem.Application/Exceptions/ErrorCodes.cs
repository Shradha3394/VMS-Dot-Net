namespace VendorManagementSystem.Application.Exceptions
{
    public enum ErrorCodes
    {
        Unknown = 0,
        NullArgument = 5001,
        InvalidCredintials,
        EmptyCredentials,
        NotFound,
        DatabaseError,
        AuthenthicationError,
        EmailServiceError,
        DuplicateEntryError,
        InternalError,
        InvalidInputFields,
        InvalidToken,
    }
}