using DatabaseLockSample.EFCore.Models;

namespace DatabaseLockSample.EFCore.Services;

public interface ILibraryService
{
    Task<Book> RegisterAsync(Book book);
    Task<Book?> GetAsync(string name);
    Task<Book> CheckOutAsync(Book book);
    Task ReturnAsync(Book book);
}