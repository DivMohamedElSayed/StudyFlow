namespace StudyFlow.API.Abstractions;

public class PaginatedList<T>(List<T> item, int pageNumber, int count, int pageSize) // list of paginated of value 
{
    public List<T> Item { get; private set; } = item;
    public int PageNumber { get; private set; } = pageNumber;
    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public async Task<PaginatedList<T>> CreateAsync(IQueryable<T> sourse,int pageNumber,int pageSize,CancellationToken cancellationToken = default)
    {
        var count = await sourse.CountAsync(cancellationToken);
        var items = await sourse.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PaginatedList<T>(items,pageNumber,count,pageSize); 
    }

}
