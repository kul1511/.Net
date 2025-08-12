using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class TestingAuthDbContext : IdentityDbContext<ApplicationUser>
{
    public TestingAuthDbContext(DbContextOptions<TestingAuthDbContext> options) : base(options) { }
    public TestingAuthDbContext() { }
    public DbSet<UserDetails> UserDetails { get; set; }
}
