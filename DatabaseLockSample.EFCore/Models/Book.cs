using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseLockSample.EFCore.Models;

[Table("book")]
public class Book
{
    protected Book() { }

    private Book(long? id, string name, int stock, int version)
    {
        Id = id;
        Name = name;
        Stock = stock;
        Version = version;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public int Stock { get; set; }
    
    [ConcurrencyCheck]
    public int Version { get; set; }

    public static Book Register(string name, int stock) => new(null, name, stock, 0);

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Stock)}: {Stock}, {nameof(Version)}: {Version}";
    }
}