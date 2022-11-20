using DatabaseLockSample.EFCore.Models;

namespace DatabaseLockSample.EFCore.Services;

public interface IBookService
{
    Book Register(Book book);
    Book? Get(string name);
    Book CheckOut(Book book);
    void Return(Book book);
}