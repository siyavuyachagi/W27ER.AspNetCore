namespace Application.Shared;
/// <summary>
/// Represents the result of an operation, including success status, message, errors, and optionally data.
/// </summary>
/// <typeparam name="T">The type of data returned on a successful operation.</typeparam>
public class GenericResult<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// A human-readable message describing the result.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// A list of error messages if the operation failed.
    /// </summary>
    public List<string> Errors { get; set; } = new List<string>();

    /// <summary>
    /// The data returned by the operation if successful.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Private constructor to enforce the use of the <see cref="Success"/> and <see cref="Failure"/> methods.
    /// </summary>
    /// <param name="succeeded">Whether the operation succeeded.</param>
    /// <param name="message">A descriptive message for the operation.</param>
    /// <param name="data">The data returned if successful.</param>
    /// <param name="errors">List of errors if the operation failed.</param>
    private GenericResult(bool succeeded, string message, T? data = default, List<string>? errors = null)
    {
        Succeeded = succeeded;
        Message = message;
        Data = data;
        Errors = errors ?? new List<string>();
    }

    /// <summary>
    /// Creates a successful result with optional message and data.
    /// </summary>
    /// <param name="message">Optional success message. Defaults to "Operation completed successfully."</param>
    /// <param name="data">Optional data to include with the success result.</param>
    /// <param name="errors">Optional errors to include with the success result.</param>
    /// <returns>A <see cref="Result{T}"/> representing a successful operation.</returns>
    public static GenericResult<T> Success(T? data = default, string message = "Operation completed successfully.", List<string>? errors = null)
        => new GenericResult<T>(true, message, data, errors);

    /// <summary>
    /// Creates a failed result with optional message and errors.
    /// </summary>
    /// <param name="message">Optional failure message. Defaults to "Operation failed."</param>
    /// <param name="errors">Optional list of errors associated with the failure.</param>
    /// <returns>A <see cref="Result{T}"/> representing a failed operation.</returns>
    public static GenericResult<T> Failure(string message = "Operation failed.", List<string>? errors = null)
        => new GenericResult<T>(false, message, default, errors);
}
