using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

// This annotation specifies the table name in the database for this entity.
// Uncomment it if the table name in the database is different from the class name
[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    // Foreign key to associate the photo with a specific user (AppUser)
    public int AppUserId { get; set; }
    // Navigation property representing the relationship between Photo and AppUser.
    // The `null!` operator indicates that this property must not be null at runtime.
    // It's initialized to `null!` here to satisfy the compiler, but it is expected
    // that this property will be populated (e.g., via Entity Framework during data retrieval).
    public AppUser AppUser { get; set; } = null!;
}