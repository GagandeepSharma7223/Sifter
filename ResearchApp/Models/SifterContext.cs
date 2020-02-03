using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ResearchApp.Models
{
    public partial class SifterContext : DbContext
    {
        public SifterContext()
        {
        }

        public SifterContext(DbContextOptions<SifterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Action> Action { get; set; }
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<AuthorLanguage> AuthorLanguage { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Book2> Book2 { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Measurement> Measurement { get; set; }
        public virtual DbSet<Particular> Particular { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<RawBook> RawBook { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        //public virtual DbSet<TreeCategory> TreeCategory { get; set; }
        //public virtual DbSet<TreeColumn> TreeColumn { get; set; }
        //public virtual DbSet<TreeTable> TreeTable { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<VAuthor> VAuthor { get; set; }
        public virtual DbSet<VAuthorLanguage> VAuthorLanguage { get; set; }
        public virtual DbSet<VUnit> VUnit { get; set; }
        public virtual DbSet<VWork> VWork { get; set; }
        public virtual DbSet<VWorkAuthor> VWorkAuthor { get; set; }
        public virtual DbSet<Work> Work { get; set; }
        public virtual DbSet<WorkAuthor> WorkAuthor { get; set; }
        public virtual DbSet<MetaCategory> MetaCategory { get; set; }
        public virtual DbSet<MetaColumn> MetaColumn { get; set; }
        public virtual DbSet<MetaTable> MetaTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=18.224.44.180;Database=Sifter;User Id=sifter;Password=mintrum1357;Trusted_Connection=False;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action>(entity =>
            {
                entity.Property(e => e.ActionId)
                    .HasColumnName("ActionID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [ActionSeq])");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.AuthorID)
                    .HasColumnName("AuthorID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [AuthorSeq])");

                entity.Property(e => e.AlsoKnownAs).HasMaxLength(100);

                entity.Property(e => e.BirthCountryID).HasColumnName("BirthCountryID");

                entity.Property(e => e.Comments)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.FullName).HasMaxLength(400);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.Occupation)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.PenName).HasMaxLength(100);

                entity.Property(e => e.Sources)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AuthorLanguage>(entity =>
            {
                entity.Property(e => e.AuthorLanguageId)
                    .HasColumnName("AuthorLanguageID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [AuthorLanguageSeq])");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.BookId)
                    .HasColumnName("BookID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [BookSeq])");

                entity.Property(e => e.Authors).HasMaxLength(500);

                entity.Property(e => e.Extension)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.FileLocation)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PublicationDate)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Publisher).HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(1000);
            });

            modelBuilder.Entity<Book2>(entity =>
            {
                entity.Property(e => e.Book2Id)
                    .HasColumnName("Book2ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Authors).HasMaxLength(500);

                entity.Property(e => e.Extension)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.FileLocation)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PublicationDate)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Publisher).HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(1000);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryID)
                    .HasColumnName("CategoryID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [CategorySeq])");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityID)
                    .HasColumnName("CityID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [CitySeq])");

                entity.Property(e => e.AlternateNames).HasMaxLength(200);

                entity.Property(e => e.CountryID).HasColumnName("CountryID");

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.FeatureCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GeoNameID).HasColumnName("GeoNameID");

                entity.Property(e => e.Latitude).HasColumnType("decimal(8, 5)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(8, 5)");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NameAscii).HasMaxLength(100);

                entity.Property(e => e.RegionID).HasColumnName("RegionID");

                entity.Property(e => e.SearchNames).HasMaxLength(4000);

                entity.Property(e => e.Slug).HasMaxLength(100);

                entity.Property(e => e.TimeZone)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryID)
                    .HasColumnName("CountryID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [CountrySeq])");

                entity.Property(e => e.AlternateNames).HasMaxLength(200);

                entity.Property(e => e.Code2)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Code3)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Continent)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.GeoNameID).HasColumnName("GeoNameID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NameAscii).HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Slug).HasMaxLength(100);

                entity.Property(e => e.TlD)
                    .HasColumnName("TLD")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.CurrencyId)
                    .HasColumnName("CurrencyID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [CurrencySeq])");

                entity.Property(e => e.Name).HasMaxLength(40);
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.EquipmentId)
                    .HasColumnName("EquipmentID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [EquipmentSeq])");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.LanguageID)
                    .HasColumnName("LanguageID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [LanguageSeq])");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ValidFromUtc)
                    .HasColumnName("ValidFromUTC")
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.ValidToUtc)
                    .HasColumnName("ValidToUTC")
                    .HasDefaultValueSql("(CONVERT([datetime2],'9999-12-31 23:59:59.9999999'))");
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.Property(e => e.MeasurementId)
                    .HasColumnName("MeasurementID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [MeasurementSeq])");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Particular>(entity =>
            {
                entity.Property(e => e.ParticularId)
                    .HasColumnName("ParticularID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [ParticularSeq])");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.Property(e => e.PublisherID)
                    .HasColumnName("PublisherID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PublisherSeq])");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<RawBook>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Authors).IsUnicode(false);

                entity.Property(e => e.BookFrom)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Bwpdffile)
                    .HasColumnName("BWPDFFile")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Collection)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Contributor)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FileBaseName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FileCount)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Languages)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LenTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MetaXmlfile)
                    .HasColumnName("MetaXMLFile")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.Pdffile)
                    .HasColumnName("PDFFile")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PublicationDateStr)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Publisher)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Sponsor)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ThumbFile)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Title).IsUnicode(false);

                entity.Property(e => e.Topics).IsUnicode(false);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.RegionID)
                    .HasColumnName("RegionID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [RegionSeq])");

                entity.Property(e => e.AlternateNames).HasMaxLength(200);

                entity.Property(e => e.CountryID).HasColumnName("CountryID");

                entity.Property(e => e.DisplayName).HasMaxLength(100);

                entity.Property(e => e.GeoNameCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GeoNameID).HasColumnName("GeoNameID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NameAscii).HasMaxLength(100);

                entity.Property(e => e.Slug).HasMaxLength(100);
            });

            modelBuilder.Entity<TreeCategory>(entity =>
            {
                entity.HasKey(e => e.CategorySeq)
                    .HasName("PK__TreeCate__74C21A54F40A2A8F");

                entity.Property(e => e.CategorySeq).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TreeColumn>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ColumnName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FkdisplayCol)
                    .HasColumnName("FKDisplayCol")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FkjoinCol)
                    .HasColumnName("FKJoinCol")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fktable)
                    .HasColumnName("FKTable")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IDColumn).HasColumnName("IDColumn");

                entity.Property(e => e.TableName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TreeTable>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.Property(e => e.UnitID)
                    .HasColumnName("UnitID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [UnitSeq])");

                entity.Property(e => e.Attribution).HasMaxLength(200);

                entity.Property(e => e.CategoryID).HasColumnName("CategoryID");

                entity.Property(e => e.LiteralTitle).HasMaxLength(100);

                entity.Property(e => e.Text).HasMaxLength(4000);

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.TitleEnglish).HasMaxLength(100);

                entity.Property(e => e.WorkID).HasColumnName("WorkID");
            });

            modelBuilder.Entity<VAuthor>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vAuthor");

                entity.Property(e => e.AlsoKnownAs).HasMaxLength(100);

                entity.Property(e => e.Author).HasMaxLength(400);

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.BirthCountry).HasMaxLength(100);

                entity.Property(e => e.Comments)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.Occupation)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.PenName).HasMaxLength(100);

                entity.Property(e => e.Sources)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VAuthorLanguage>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vAuthorLanguage");

                entity.Property(e => e.Author).HasMaxLength(400);

                entity.Property(e => e.AuthorLanguageId).HasColumnName("AuthorLanguageID");

                entity.Property(e => e.Language).HasMaxLength(100);
            });

            modelBuilder.Entity<VUnit>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vUnit");

                entity.Property(e => e.Attribution).HasMaxLength(200);

                entity.Property(e => e.Author).HasMaxLength(400);

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Text).HasMaxLength(4000);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

            });

            modelBuilder.Entity<VWork>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vWork");

                entity.Property(e => e.Author).HasMaxLength(400);

                entity.Property(e => e.Editor).HasMaxLength(400);

                entity.Property(e => e.Language).HasMaxLength(100);

                entity.Property(e => e.PubCity).HasMaxLength(100);

                entity.Property(e => e.PubCountry).HasMaxLength(100);

                entity.Property(e => e.PubRegion).HasMaxLength(100);

                entity.Property(e => e.Publisher).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.TitleEnglish).HasMaxLength(200);

                entity.Property(e => e.Translator).HasMaxLength(400);

                entity.Property(e => e.WorkId).HasColumnName("WorkID");
            });

            modelBuilder.Entity<VWorkAuthor>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vWorkAuthor");

                entity.Property(e => e.Author).HasMaxLength(400);

                entity.Property(e => e.Role)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Work).HasMaxLength(200);

                entity.Property(e => e.WorkAuthorId).HasColumnName("WorkAuthorID");
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.Property(e => e.WorkID)
                    .HasColumnName("WorkID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [WorkSeq])");

                entity.Property(e => e.AuthorID).HasColumnName("AuthorID");

                entity.Property(e => e.CityID).HasColumnName("CityID");

                entity.Property(e => e.EditorID).HasColumnName("EditorID");

                entity.Property(e => e.LanguageID).HasColumnName("LanguageID");

                entity.Property(e => e.PublisherID).HasColumnName("PublisherID");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.TitleEnglish).HasMaxLength(200);

                entity.Property(e => e.TitleLiteral).HasMaxLength(200);

                entity.Property(e => e.TranslatorID).HasColumnName("TranslatorID");
            });

            modelBuilder.Entity<WorkAuthor>(entity =>
            {
                entity.Property(e => e.WorkAuthorID)
                    .HasColumnName("WorkAuthorID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [WorkAuthorSeq])");

                entity.Property(e => e.AuthorID).HasColumnName("AuthorID");

                entity.Property(e => e.Role)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.WorkID).HasColumnName("WorkID");
            });

            modelBuilder.Entity<MetaCategory>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.CategorySeq)
                    .HasName("CI")
                    .IsClustered();

                entity.Property(e => e.MetaCategoryId)
                    .HasColumnName("MetaCategoryID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [MetaCategorySeq])");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MetaColumn>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => new { e.TableName, e.ColSeq })
                    .HasName("CI")
                    .IsClustered();

                entity.Property(e => e.ColType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ColumnName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FkdisplayCol)
                    .HasColumnName("FKDisplayCol")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FkjoinCol)
                    .HasColumnName("FKJoinCol")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fktable)
                    .HasColumnName("FKTable")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Idcolumn).HasColumnName("IDColumn");

                entity.Property(e => e.MetaColumnId)
                    .HasColumnName("MetaColumnID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [MetaColumnSeq])");

                entity.Property(e => e.TableName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MetaTable>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.TableName)
                    .HasName("CI")
                    .IsClustered();

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MetaCategoryId).HasColumnName("MetaCategoryID");

                entity.Property(e => e.MetaTableId)
                    .HasColumnName("MetaTableID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [MetaTableSeq])");

                entity.Property(e => e.TableName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });


            modelBuilder.HasSequence<int>("ActionSeq").StartsAt(6059);

            modelBuilder.HasSequence<int>("AuthorLanguageSeq").StartsAt(5296);

            modelBuilder.HasSequence<int>("AuthorSeq").StartsAt(5422);

            modelBuilder.HasSequence<int>("BookSeq").StartsAt(10001);

            modelBuilder.HasSequence<int>("CategorySeq").StartsAt(130);

            modelBuilder.HasSequence<int>("CitySeq").StartsAt(16309);

            modelBuilder.HasSequence<int>("CountrySeq").StartsAt(253);

            modelBuilder.HasSequence<int>("CurrencySeq").StartsAt(246);

            modelBuilder.HasSequence<int>("EquipmentSeq").StartsAt(165);

            modelBuilder.HasSequence<int>("LanguageSeq").StartsAt(251);

            modelBuilder.HasSequence<int>("MeasurementSeq").StartsAt(163);

            modelBuilder.HasSequence<int>("ParticularSeq").StartsAt(424);

            modelBuilder.HasSequence<int>("PublisherSeq").StartsAt(2856);

            modelBuilder.HasSequence<int>("RegionSeq").StartsAt(3960);

            modelBuilder.HasSequence<int>("UnitSeq").StartsAt(124639);

            modelBuilder.HasSequence<int>("WorkAuthorSeq").StartsAt(5508);

            modelBuilder.HasSequence<int>("WorkSeq").StartsAt(5216);

            modelBuilder.HasSequence<int>("MetaCategorySeq").StartsAt(101);

            modelBuilder.HasSequence<int>("MetaColumnSeq").StartsAt(1001);

            modelBuilder.HasSequence<int>("MetaTableSeq").StartsAt(101);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
