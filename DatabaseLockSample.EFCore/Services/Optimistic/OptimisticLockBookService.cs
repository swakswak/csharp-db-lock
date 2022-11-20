using System.Data;
using DatabaseLockSample.EFCore.Models;

namespace DatabaseLockSample.EFCore.Services.Optimistic;

public class OptimisticLockBookService : IBookService
{
    public OptimisticLockBookService(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }

    public Book Register(Book book)
    {
        var alreadyExistsBook = DbContext.Books.FirstOrDefault(b => book.Name.Equals(b.Name));
        if (alreadyExistsBook is not null) throw new DuplicateNameException(book.Name);

        var added = DbContext.Books.Add(book);
        DbContext.SaveChanges();

        return added.Entity;
    }

    public Book? Get(string name)
    {
        return DbContext.Books.FirstOrDefault(b => b.Name.Equals(name));
    }

    public Book CheckOut(Book book)
    {
        var expectedVersion = book.Version;
        var forCheckOut = DbContext.Books.FirstOrDefault(b => b.Id == book.Id && b.Version == expectedVersion);
        
        if (forCheckOut is null) throw new KeyNotFoundException();
        if (forCheckOut.Stock < 1) throw new OperationCanceledException("Out of stock.");

        forCheckOut.Stock -= 1;
        forCheckOut.Version = expectedVersion + 1;
        var checkBookBeforeCommit = DbContext.Books.FirstOrDefault(b => b.Id == book.Id && b.Version == expectedVersion);
        if (checkBookBeforeCommit is null) throw new KeyNotFoundException();

        DbContext.SaveChanges();

        return forCheckOut;
    }

    public void Return(Book book)
    {
        throw new NotImplementedException();
    }
}