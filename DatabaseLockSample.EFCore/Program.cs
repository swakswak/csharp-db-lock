using DatabaseLockSample.EFCore.Models;
using DatabaseLockSample.EFCore.Services.Optimistic;

var optimisticLockBookService = new OptimisticLockLibraryService();

var newBook = Book.Register(
    $"TEST-BOOK-{Guid.NewGuid().ToString().Split("-")[0]}",
    1
);
var registered = await optimisticLockBookService.RegisterAsync(newBook);
Console.WriteLine($"registered=({registered})");

var checkedOutBook = await optimisticLockBookService.CheckOutAsync((await optimisticLockBookService.GetAsync(registered.Name))!);
Console.WriteLine($"checkedOutBook=({checkedOutBook})");

await optimisticLockBookService.ReturnAsync(checkedOutBook);

var afterReturn = await optimisticLockBookService.GetAsync(registered.Name);
Console.WriteLine($"afterReturn=({afterReturn})");

Console.WriteLine();
Console.WriteLine("===== result =====");
Console.WriteLine($"registered.Version={registered.Version}, " +
                  $"checkedOutBook.Version={checkedOutBook.Version}, " +
                  $"afterReturn.Version={afterReturn!.Version}");