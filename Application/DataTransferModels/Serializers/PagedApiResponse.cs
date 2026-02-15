namespace Application.DataTransferModels.Serializers;

public class PagedApiResponse : ApiResponse
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }



    public static PagedApiResponse Ok(
    int pageNumber = default,
    int pageSize = default,
    int totalRecords = default,
    string message = "Operation successful")
    {

        return new PagedApiResponse
        {
            Success = true,
            Message = message,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < ((int)Math.Ceiling(totalRecords / (double)pageSize))
        };
    }
}