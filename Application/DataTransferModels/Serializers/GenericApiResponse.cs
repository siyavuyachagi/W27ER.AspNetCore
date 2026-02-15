using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.DataTransferModels.Serializers;

#region Generic ApiResponse
/// <summary>
/// Generic Standardized API response wrapper used for all endpoints.
/// Encapsulates the success status, message, payload data,
/// validation errors, and timestamp. Designed to provide consistent responses
/// across the application for both success and failure scenarios.
/// </summary>
public class GenericApiResponse<T>
{
    /// <summary>
    /// Indicates whether the request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// A human-readable message describing the result.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Optional payload or data for the response.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// General list of error messages.
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Field-specific validation errors.
    /// </summary>
    public Dictionary<string, List<string>>? FieldErrors { get; set; }

    /// <summary>
    /// Timestamp of when the response was generated (UTC).
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;



    #region Common Responses
    /// <summary>
    /// Creates a successful response with status code 200 (OK).
    /// </summary>
    /// <param name="data">Optional payload.</param>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing success.</returns>
    public static GenericApiResponse<T> Ok(T? data = default, string message = "Operation successful")
    {
        return new()
        {
            //Status = 200,
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Creates a response with status code 201 (Created).
    /// </summary>
    /// <param name="data">Optional created entity.</param>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing creation success.</returns>
    public static GenericApiResponse<T> Created(T? data = default, string message = "Resource created successfully")
    {
        return new()
        {
            //Status = 201,
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Creates a response with status code 204 (No Content).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing no content.</returns>
    public static GenericApiResponse<T> NoContent(string message = "No content")
    {
        return new()
        {
            //Status = 204,
            Success = true,
            Message = message,
            Data = default
        };
    }

    /// <summary>
    /// Creates a response with status code 302 (Redirect) pointing to a new location.
    /// </summary>
    /// <param name="url">The URL to redirect to.</param>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing a redirect.</returns>
    public static GenericApiResponse<T> Redirect(string url, string message = "Resource has moved")
    {
        return new()
        {
            //Status = 302,
            Success = true,
            Message = message,
            Data = (T?)(object?)url
        };
    }

    /// <summary>
    /// Creates a response with status code 400 (Bad Request).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <param name="errors">Optional list of errors.</param>
    /// <returns>GenericApiResponse representing a bad request.</returns>
    public static GenericApiResponse<T> BadRequest(string message = "Bad request", List<string>? errors = null)
    {
        return new()
        {
            //Status = 400,
            Success = false,
            Message = message,
            Errors = errors
        };
    }

    /// <summary>
    /// Creates a response with status code 401 (Unauthorized).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing unauthorized access.</returns>
    public static GenericApiResponse<T> Unauthorized(string message = "Unauthorized")
    {
        return new()
        {
            //Status = 401,
            Success = false,
            Message = message
        };
    }

    /// <summary>
    /// Creates a response with status code 403 (Forbidden).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing forbidden access.</returns>
    public static GenericApiResponse<T> Forbidden(string message = "Forbidden")
    {
        return new()
        {
            //Status = 403,
            Success = false,
            Message = message
        };
    }

    /// <summary>
    /// Creates a response with status code 404 (Not Found).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing not found.</returns>
    public static GenericApiResponse<T> NotFound(string message = "Resource not found")
    {
        return new()
        {
            //Status = 404,
            Success = false,
            Message = message
        };
    }


    /// <summary>
    /// Creates a response with status code 409 (Conflict).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing a conflict.</returns>
    public static GenericApiResponse<T> Conflict(string message = "Conflict occurred")
    {
        return new()
        {
            //Status = 409,
            Success = false,
            Message = message
        };
    }

    /// <summary>
    /// Creates a response with status code 422 (Unprocessable Entity) using ModelState.
    /// Extracts validation errors automatically.
    /// </summary>
    /// <param name="modelState">The ModelStateDictionary from the controller.</param>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing validation failure.</returns>
    public static GenericApiResponse<T> UnprocessableEntity(ModelStateDictionary modelState, string message = "Validation failed")
    {
        var fieldErrors = modelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
            );

        var errors = fieldErrors.SelectMany(f => f.Value).ToList();

        return new GenericApiResponse<T>
        {
            //Status = 422,
            Success = false,
            Message = message,
            Errors = errors.Any() ? errors : null,
            FieldErrors = fieldErrors.Any() ? fieldErrors : null
        };
    }

    /// <summary>
    /// Creates a response with status code 500 (Internal Server Error).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing a server error.</returns>
    public static GenericApiResponse<T> InternalServerError(string message = "Internal server error")
    {
        return new()
        {
            //Status = 500,
            Success = false,
            Message = message
        };
    }
    #endregion
}
#endregion
