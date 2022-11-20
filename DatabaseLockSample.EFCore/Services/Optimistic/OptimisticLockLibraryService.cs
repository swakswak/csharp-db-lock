using System.Data;
using DatabaseLockSample.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLockSample.EFCore.Services.Optimistic;

public class OptimisticLockLibraryService : ILibraryService
{
    public async Task<Book> RegisterAsync(Book book)
    {
        await using var dbContext = new AppDbContext();
        var alreadyExistsBook = await dbContext.Books.FirstOrDefaultAsync(b => book.Name.Equals(b.Name));
        if (alreadyExistsBook is not null) throw new DuplicateNameException(book.Name);

        var added = await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        return added.Entity;
    }

    public async Task<Book?> GetAsync(string name)
    {
        await using var dbContext = new AppDbContext();
        return await dbContext.Books.FirstOrDefaultAsync(b => b.Name.Equals(name));
    }

    public async Task<Book> CheckOutAsync(Book book)
    {
        await using var dbContext = new AppDbContext();
        var expectedVersion = book.Version;
        var forCheckOut = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id && b.Version == expectedVersion);
        
        if (forCheckOut is null) throw new KeyNotFoundException($"{book.Id}");
        if (forCheckOut.Stock < 1) throw new OperationCanceledException("Out of stock.");

        forCheckOut.Stock -= 1;
        forCheckOut.Version = expectedVersion + 1;
        var checkBookBeforeCommit = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id && b.Version == expectedVersion);
        if (checkBookBeforeCommit is null) throw new KeyNotFoundException($"{expectedVersion}");

        await dbContext.SaveChangesAsync();

        return forCheckOut;
    }

    public async Task ReturnAsync(Book book)
    {
        await using var dbContext = new AppDbContext();
        var expectedVersion = book.Version;
        var forReturn = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id && b.Version == expectedVersion);
        if (forReturn is null) throw new KeyNotFoundException($"{book.Id}");

        forReturn.Stock += 1;
        forReturn.Version += 1;
        
        var checkBookBeforeCommit = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id && b.Version == expectedVersion);
        if (checkBookBeforeCommit is null) throw new KeyNotFoundException($"{expectedVersion}");

        await dbContext.SaveChangesAsync();
    }
}