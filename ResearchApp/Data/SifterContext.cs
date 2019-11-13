//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;
//namespace ResearchApp.Models
//{
//    public class SifterContext : DbContext
//    {
//        public SifterContext(DbContextOptions<SifterContext> options)
//            : base(options)
//        {
//        }

//        public DbSet<Publisher> Publisher { get; set; }
//        public DbSet<Author> Author { get; set; }
//        public DbSet<Work> Works { get; set; }
//        public DbSet<Category> Categories { get; set; }
//        public DbSet<Language> Languages { get; set; }
//        public DbSet<City> Cities { get; set; }
//        public DbSet<Region> Regions { get; set; }
//        public DbSet<Country> Countries { get; set; }
//        public DbSet<WorkAuthor> WorkAuthors { get; set; }
//        public DbSet<Unit> Units { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Category>(entity =>
//            {
//                entity.Property(e => e.CategoryId)
//                    .HasColumnName("CategoryID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [CategorySeq])");

//                entity.Property(e => e.Name)
//                    .HasMaxLength(100)
//                    .IsUnicode(false);
//            });

//            modelBuilder.Entity<WorkAuthor>(entity =>
//            {
//                entity.Property(e => e.WorkAuthorId)
//                    .HasColumnName("WorkAuthorID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [WorkAuthorSeq])");
//                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
//                entity.Property(e => e.WorkId).HasColumnName("WorkID");
//                entity.Property(e => e.Role)
//                   .HasMaxLength(40);
//            });

//            modelBuilder.Entity<Publisher>(entity =>
//            {
//                entity.Property(e => e.PublisherId)
//                    .HasColumnName("PublisherID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [PublisherSeq])");

//                entity.Property(e => e.Name).HasMaxLength(100);
//            });

//            modelBuilder.Entity<Author>(entity =>
//            {
//                entity.Property(e => e.AuthorId)
//                    .HasColumnName("AuthorID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [AuthorSeq])");
//                entity.Property(e => e.AlsoKnownAs).HasMaxLength(100);
//                entity.Property(e => e.BirthCountryId).HasColumnName("BirthCountryID");
//                entity.Property(e => e.Comments)
//                    .HasMaxLength(400)
//                    .IsUnicode(false);
//                entity.Property(e => e.Title).HasMaxLength(200);
//                entity.Property(e => e.FirstName).HasMaxLength(200);
//                entity.Property(e => e.FullName).HasMaxLength(300);
//                entity.Property(e => e.Gender)
//                    .HasMaxLength(10)
//                    .IsUnicode(false);
//                entity.Property(e => e.IsOrganization)
//                    .HasMaxLength(3)
//                    .IsUnicode(false);
//                entity.Property(e => e.PenName).HasMaxLength(100);
//                entity.Property(e => e.Sources)
//                    .HasMaxLength(400)
//                    .IsUnicode(false);
//                entity.Property(e => e.Title).HasMaxLength(60);
//                entity.Property(e => e.LastName).HasMaxLength(400);
//            });

//            modelBuilder.Entity<Work>(entity =>
//            {
//                entity.Property(e => e.WorkId)
//                    .HasColumnName("WorkID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [WorkSeq])");

//                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

//                entity.Property(e => e.CityId).HasColumnName("CityID");

//                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

//                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

//                entity.Property(e => e.Title).HasMaxLength(200);

//                entity.Property(e => e.TitleEnglish).HasMaxLength(200);

//                entity.Property(e => e.TitleLiteral).HasMaxLength(200);
//            });

//            modelBuilder.Entity<Language>(entity =>
//            {
//                entity.Property(e => e.LanguageId)
//                    .HasColumnName("LanguageID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [LanguageSeq])");

//                entity.Property(e => e.Name).HasMaxLength(100);
//            });

//            modelBuilder.Entity<City>(entity =>
//            {
//                entity.Property(e => e.CityId)
//                    .HasColumnName("CityID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [CitySeq])");

//                entity.Property(e => e.AlternateNames).HasMaxLength(200);

//                entity.Property(e => e.CountryId).HasColumnName("CountryID");

//                entity.Property(e => e.DisplayName).HasMaxLength(200);

//                entity.Property(e => e.FeatureCode)
//                    .HasMaxLength(10)
//                    .IsUnicode(false);

//                entity.Property(e => e.GeoNameId).HasColumnName("GeoNameID");

//                entity.Property(e => e.Latitude).HasColumnType("decimal(8, 5)");

//                entity.Property(e => e.Longitude).HasColumnType("decimal(8, 5)");

//                entity.Property(e => e.Name).HasMaxLength(100);

//                entity.Property(e => e.NameAscii).HasMaxLength(100);

//                entity.Property(e => e.RegionId).HasColumnName("RegionID");

//                entity.Property(e => e.SearchNames).HasMaxLength(4000);

//                entity.Property(e => e.Slug).HasMaxLength(100);

//                entity.Property(e => e.TimeZone)
//                    .HasMaxLength(40)
//                    .IsUnicode(false);
//            });

//            modelBuilder.Entity<Region>(entity =>
//            {
//                entity.Property(e => e.RegionId)
//                    .HasColumnName("RegionID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [RegionSeq])");

//                entity.Property(e => e.AlternateNames).HasMaxLength(200);

//                entity.Property(e => e.CountryId).HasColumnName("CountryID");

//                entity.Property(e => e.DisplayName).HasMaxLength(100);

//                entity.Property(e => e.GeoNameCode)
//                    .HasMaxLength(10)
//                    .IsUnicode(false);

//                entity.Property(e => e.GeoNameId).HasColumnName("GeoNameID");

//                entity.Property(e => e.Name).HasMaxLength(100);

//                entity.Property(e => e.NameAscii).HasMaxLength(100);

//                entity.Property(e => e.Slug).HasMaxLength(100);
//            });

//            modelBuilder.Entity<Country>(entity =>
//            {
//                entity.Property(e => e.CountryId)
//                    .HasColumnName("CountryID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [CountrySeq])");

//                entity.Property(e => e.AlternateNames).HasMaxLength(200);

//                entity.Property(e => e.Code2)
//                    .HasMaxLength(2)
//                    .IsUnicode(false);

//                entity.Property(e => e.Code3)
//                    .HasMaxLength(3)
//                    .IsUnicode(false);

//                entity.Property(e => e.Continent)
//                    .HasMaxLength(2)
//                    .IsUnicode(false);

//                entity.Property(e => e.GeoNameId).HasColumnName("GeoNameID");

//                entity.Property(e => e.Name).HasMaxLength(100);

//                entity.Property(e => e.NameAscii).HasMaxLength(100);

//                entity.Property(e => e.Phone)
//                    .HasMaxLength(20)
//                    .IsUnicode(false);

//                entity.Property(e => e.Slug).HasMaxLength(100);

//                entity.Property(e => e.Tld)
//                    .HasColumnName("TLD")
//                    .HasMaxLength(10)
//                    .IsUnicode(false);
//            });

//            modelBuilder.Entity<Unit>(entity =>
//            {
//                entity.Property(e => e.UnitId)
//                    .HasColumnName("UnitID")
//                    .HasDefaultValueSql("(NEXT VALUE FOR [UnitSeq])");

//                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

//                entity.Property(e => e.Text).HasMaxLength(4000);

//                entity.Property(e => e.Title).HasMaxLength(100);

//                entity.Property(e => e.LiteralTitle).HasMaxLength(100);

//                entity.Property(e => e.TitleEnglish).HasMaxLength(100);

//                entity.Property(e => e.Attribution).HasMaxLength(400);

//                entity.Property(e => e.WorkId).HasColumnName("WorkID");
//            });
//        }
//    }
//}
