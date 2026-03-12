using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Models;

namespace RoyalVilla_API.Context
{
    public class RoyalVilleDbContext(DbContextOptions<RoyalVilleDbContext> options)
        : DbContext(options)
    {
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Villa>()
                .HasData(
                    new Villa
                    {
                        Id = 1,
                        Name = "Royal Villa",
                        Details =
                            "Luxurious villa with stunning ocean views and a private beach access.",
                        Rate = 500.0,
                        Sqft = 2500,
                        Occupancy = 6,
                        ImageUrl =
                            "https://media.istockphoto.com/id/506903162/photo/luxurious-villa-with-pool.jpg?s=612x612&w=0&k=20&c=Ek2P0DQ9nHQero4m9mdDyCVMVq3TLnXigxNPcZbgX2E=",
                        CreatedDate = new DateTime(2025, 1, 1),
                        UpdatedDate = new DateTime(2025, 1, 1),
                    },
                    new Villa
                    {
                        Id = 2,
                        Name = "Royal Palace",
                        Details =
                            "Luxurious palace with stunning ocean views and a private beach access.",
                        Rate = 800.0,
                        Sqft = 1500,
                        Occupancy = 14,
                        ImageUrl =
                            "https://media.istockphoto.com/id/165824154/photo/emirates-palace-abu-dhabi-uae.jpg?s=612x612&w=0&k=20&c=vmOX2eZZwJp1hy1pPsWqs5V6Mc9ZN-LKfgoHvgn1w-8=",
                        CreatedDate = new DateTime(2024, 1, 1),
                        UpdatedDate = new DateTime(2025, 1, 1),
                    },
                    new Villa
                    {
                        Id = 3,
                        Name = "Mansion",
                        Details = "Luxurious mansion with stunning ocean views.",
                        Rate = 300.0,
                        Sqft = 1500,
                        Occupancy = 5,
                        ImageUrl =
                            "https://img.freepik.com/free-photo/colonial-style-house-night-scene_1150-17925.jpg?semt=ais_rp_progressive&w=740&q=80",
                        CreatedDate = new DateTime(2025, 6, 1),
                    },
                    new Villa
                    {
                        Id = 4,
                        Name = "shark",
                        Details =
                            "shark house under reconstruction with stunning ocean views and a private beach access.",
                        Rate = 100.0,
                        Sqft = 500,
                        Occupancy = 2,
                        ImageUrl =
                            "https://cdn2.tuoitre.vn/thumb_w/750/471584752817336320/2025/7/15/shark1-1752548161011780290376-44-0-800-1210-crop-17525481761621830934074.png",
                        CreatedDate = new DateTime(2025, 1, 1),
                        UpdatedDate = new DateTime(2025, 1, 1),
                    }
                );
        }
    }
}
