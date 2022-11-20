using DatabaseLockSample.EFCore;
using DatabaseLockSample.EFCore.Models;
using DatabaseLockSample.EFCore.Services;
using DatabaseLockSample.EFCore.Services.Optimistic;

var appDbContext = new AppDbContext();
IBookService optimisticLockBookService = new OptimisticLockBookService(appDbContext);
var registered = optimisticLockBookService.Register(Book.Register("DDD", 1));
Console.WriteLine($"registered.Version={registered.Version}");
var ddd = optimisticLockBookService.Get("DDD");
Console.WriteLine($"ddd.Version={ddd.Version}");
var checkedOutBook = optimisticLockBookService.CheckOut(ddd);
Console.WriteLine($"checkedOutBook.Version={checkedOutBook.Version}");