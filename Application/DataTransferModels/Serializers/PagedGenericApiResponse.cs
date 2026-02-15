using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.DataTransferModels.Serializers;

public class PagedGenericApiResponse<T> : GenericApiResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }


    /// <summary>
    /// Creates a successful response with status code 200 (OK).
    /// </summary>
    /// <param name="data">Optional payload.</param>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing success.</returns>
    public static PagedGenericApiResponse<T> Ok(
    T? data = default,
    int pageNumber = default,
    int pageSize = default,
    int totalRecords = default,
    string message = "Operation successful")
    {
        return new PagedGenericApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < ((int)Math.Ceiling(totalRecords / (double)pageSize))
        };
    }

    /// <summary>
    /// Creates a response with status code 201 (Created).
    /// </summary>
    /// <param name="data">Optional created entity.</param>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing creation success.</returns>
    public static new PagedGenericApiResponse<T> Created(T? data = default, string message = "Resource created successfully")
    {
        return new()
        {
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
    public static new PagedGenericApiResponse<T> NoContent(string message = "No content")
    {
        return new()
        {
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
    public static new PagedGenericApiResponse<T> Redirect(string url, string message = "Resource has moved")
    {
        return new()
        {
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
    public static new PagedGenericApiResponse<T> BadRequest(string message = "Bad request", List<string>? errors = null)
    {
        return new()
        {
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
    public static new PagedGenericApiResponse<T> Unauthorized(string message = "Unauthorized")
    {
        return new()
        {
            Success = false,
            Message = message
        };
    }

    /// <summary>
    /// Creates a response with status code 403 (Forbidden).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing forbidden access.</returns>
    public static new PagedGenericApiResponse<T> Forbidden(string message = "Forbidden")
    {
        return new()
        {
            Success = false,
            Message = message
        };
    }

    /// <summary>
    /// Creates a response with status code 404 (Not Found).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing not found.</returns>
    public static new PagedGenericApiResponse<T> NotFound(string message = "Resource not found")
    {
        return new()
        {
            Success = false,
            Message = message
        };
    }


    /// <summary>
    /// Creates a response with status code 409 (Conflict).
    /// </summary>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing a conflict.</returns>
    public static new PagedGenericApiResponse<T> Conflict(string message = "Conflict occurred")
    {
        return new()
        {
            Success = true,
            Message = message,
        };
    }

    /// <summary>
    /// Creates a response with status code 422 (Unprocessable Entity) using ModelState.
    /// Extracts validation errors automatically.
    /// </summary>
    /// <param name="modelState">The ModelStateDictionary from the controller.</param>
    /// <param name="message">Optional message.</param>
    /// <returns>GenericApiResponse representing validation failure.</returns>
    public static new PagedGenericApiResponse<T> UnprocessableEntity(ModelStateDictionary modelState, string message = "Validation failed")
    {
        var fieldErrors = modelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
            );

        var errors = fieldErrors.SelectMany(f => f.Value).ToList();

        return new PagedGenericApiResponse<T>
        {
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
    public static new PagedGenericApiResponse<T> InternalServerError(string message = "Internal server error")
    {
        return new()
        {
            Success = false,
            Message = message
        };
    }
}