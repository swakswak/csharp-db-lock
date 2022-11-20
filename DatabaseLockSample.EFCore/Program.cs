using DatabaseLockSample.EFCore;
using DatabaseLockSample.EFCore.Models;
using DatabaseLockSample.EFCore.Services;
using DatabaseLockSample.EFCore.Services.Optimistic;

var optimisticLockBookService = new OptimisticLockLibraryService();
var registered = await optimisticLockBookService.RegisterAsync(Book.Register($"TEST-BOOK-{Guid.NewGuid().ToString().Split("-")[0]}", 1));
Console.WriteLine($"registered=({registered})");

var checkoutParam = await optimisticLockBookService.GetAsync(registered.Name);
Console.WriteLine($"ddd=({checkoutParam})");

var checkedOutBook = await optimisticLockBookService.CheckOutAsync(checkoutParam!);
Console.WriteLine($"checkedOutBook=({checkedOutBook})");

await optimisticLockBookService.ReturnAsync(checkedOutBook);

var afterReturn = await optimisticLockBookService.GetAsync(registered.Name);

Console.WriteLine($"registered=({registered}), afterRegister=({checkoutParam}), checkedOutBook=({checkedOutBook}), afterReturn={afterReturn}");