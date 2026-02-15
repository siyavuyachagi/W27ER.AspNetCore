namespace Application.Shared
{
    /// <summary>
    /// Non-generic helper to allow calling Result.Success() or Result.Failure() without specifying T.
    /// Uses <see cref="object"/> as the data type.
    /// </summary>
    public class Result
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
        /// Private constructor to enforce the use of the <see cref="Success"/> and <see cref="Failure"/> methods.
        /// </summary>
        /// <param name="succeeded">Whether the operation succeeded.</param>
        /// <param name="message">A descriptive message for the operation.</param>
        /// <param name="errors">List of errors if the operation failed.</param>
        private Result(bool succeeded, string message, List<string>? errors = null)
        {
            Succeeded = succeeded;
            Message = message;
            Errors = errors ?? new List<string>();
        }

        /// <summary>
        /// Creates a successful result with default data type <see cref="object"/>.
        /// </summary>
        /// <param name="message">Optional success message. Defaults to "Operation completed successfully."</param>
        /// <param name="errors">Optional list of errors associated with the failure.</param>
        /// <returns>A <see cref="Result{object}"/> representing a successful operation.</returns>
        public static Result Success(string message = "Operation completed successfully.", List<string>? errors = null)
            => new Result(true, message, errors);

        /// <summary>
        /// Creates a failed result with default data type <see cref="object"/>.
        /// </summary>
        /// <param name="message">Optional failure message. Defaults to "Operation failed."</param>
        /// <param name="errors">Optional list of errors associated with the failure.</param>
        /// <returns>A <see cref="Result{object}"/> representing a failed operation.</returns>
        public static Result Failure(string message = "Operation failed.", List<string>? errors = null)
            => new Result(false, message, errors);
    }
}
