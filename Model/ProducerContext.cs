using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
public class ProducerContext: IdentityDbContext
{
    public ProducerContext(DbContextOptions<ProducerContext> options):base(options)
    {
        
    }
    public DbSet<GiftItem> GiftItems{get;set;}
    public DbSet<GiftItemDetail> GiftItemDetails{get;set;}

    public DbSet<CartDetail> CartDetails{get;set;}
    public DbSet<Order> Orders{get;set;}
    public DbSet<OrderDetail> OrderDetails{get;set;}
}