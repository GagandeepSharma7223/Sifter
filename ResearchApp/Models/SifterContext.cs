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

        public virtual DbSet<A> A { get; set; }
        public virtual DbSet<Action> Action { get; set; }
        public virtual DbSet<AdminHoneypotLoginattempt> AdminHoneypotLoginattempt { get; set; }
        public virtual DbSet<AuthGroup> AuthGroup { get; set; }
        public virtual DbSet<AuthGroupPermissions> AuthGroupPermissions { get; set; }
        public virtual DbSet<AuthPermission> AuthPermission { get; set; }
        public virtual DbSet<AuthUser> AuthUser { get; set; }
        public virtual DbSet<AuthUserGroups> AuthUserGroups { get; set; }
        public virtual DbSet<AuthUserUserPermissions> AuthUserUserPermissions { get; set; }
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<AuthorAuthor> AuthorAuthor { get; set; }
        public virtual DbSet<AuthorAuthorLanguagesKnown> AuthorAuthorLanguagesKnown { get; set; }
        public virtual DbSet<AuthorAuthorLegacyInfo> AuthorAuthorLegacyInfo { get; set; }
        public virtual DbSet<AuthorContributor> AuthorContributor { get; set; }
        public virtual DbSet<AuthorGender> AuthorGender { get; set; }
        public virtual DbSet<AuthorLanguage> AuthorLanguage { get; set; }
        public virtual DbSet<AuthorLegacy> AuthorLegacy { get; set; }
        public virtual DbSet<AuthorOccupation> AuthorOccupation { get; set; }
        public virtual DbSet<AuthorRole> AuthorRole { get; set; }
        public virtual DbSet<AuthorTitle> AuthorTitle { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CommonCurrency> CommonCurrency { get; set; }
        public virtual DbSet<CommonLanguage> CommonLanguage { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<DjangoAdminLog> DjangoAdminLog { get; set; }
        public virtual DbSet<DjangoContentType> DjangoContentType { get; set; }
        public virtual DbSet<DjangoMigrations> DjangoMigrations { get; set; }
        public virtual DbSet<DjangoSession> DjangoSession { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<L> L { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<LexiconDefinition> LexiconDefinition { get; set; }
        public virtual DbSet<LexiconVariety> LexiconVariety { get; set; }
        public virtual DbSet<LocationCity> LocationCity { get; set; }
        public virtual DbSet<LocationCountry> LocationCountry { get; set; }
        public virtual DbSet<LocationRegion> LocationRegion { get; set; }
        public virtual DbSet<Measurement> Measurement { get; set; }
        public virtual DbSet<Particular> Particular { get; set; }
        public virtual DbSet<ParticularAction> ParticularAction { get; set; }
        public virtual DbSet<ParticularCategory> ParticularCategory { get; set; }
        public virtual DbSet<ParticularClassification> ParticularClassification { get; set; }
        public virtual DbSet<ParticularDescription> ParticularDescription { get; set; }
        public virtual DbSet<ParticularEquipment> ParticularEquipment { get; set; }
        public virtual DbSet<ParticularGroup> ParticularGroup { get; set; }
        public virtual DbSet<ParticularIngredient> ParticularIngredient { get; set; }
        public virtual DbSet<ParticularMaterial> ParticularMaterial { get; set; }
        public virtual DbSet<ParticularMeasurement> ParticularMeasurement { get; set; }
        public virtual DbSet<ParticularParticular> ParticularParticular { get; set; }
        public virtual DbSet<ParticularType> ParticularType { get; set; }
        public virtual DbSet<PublicationDigitalsurrogate> PublicationDigitalsurrogate { get; set; }
        public virtual DbSet<PublicationLegacywork> PublicationLegacywork { get; set; }
        public virtual DbSet<PublicationPublisher> PublicationPublisher { get; set; }
        public virtual DbSet<PublicationRepository> PublicationRepository { get; set; }
        public virtual DbSet<PublicationSeries> PublicationSeries { get; set; }
        public virtual DbSet<PublicationWork> PublicationWork { get; set; }
        public virtual DbSet<PublicationWorkAuthor> PublicationWorkAuthor { get; set; }
        public virtual DbSet<PublicationWorkElectronicCopies> PublicationWorkElectronicCopies { get; set; }
        public virtual DbSet<PublicationWorkKnownCopies> PublicationWorkKnownCopies { get; set; }
        public virtual DbSet<PublicationWorkLanguage> PublicationWorkLanguage { get; set; }
        public virtual DbSet<PublicationWorkLegacyData> PublicationWorkLegacyData { get; set; }
        public virtual DbSet<PublicationWorktype> PublicationWorktype { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<RawAuthor> RawAuthor { get; set; }
        public virtual DbSet<RawCountry> RawCountry { get; set; }
        public virtual DbSet<RawInfoType> RawInfoType { get; set; }
        public virtual DbSet<RawUnit> RawUnit { get; set; }
        public virtual DbSet<RawWhatType> RawWhatType { get; set; }
        public virtual DbSet<RawWork> RawWork { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<RegistrationRegistrationprofile> RegistrationRegistrationprofile { get; set; }
        public virtual DbSet<RegistrationSupervisedregistrationprofile> RegistrationSupervisedregistrationprofile { get; set; }
        public virtual DbSet<TreeCategory> TreeCategory { get; set; }
        public virtual DbSet<TreeColumn> TreeColumn { get; set; }
        public virtual DbSet<TreeTable> TreeTable { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<UnitCategory> UnitCategory { get; set; }
        public virtual DbSet<UnitLegacyunit> UnitLegacyunit { get; set; }
        public virtual DbSet<UnitPeople> UnitPeople { get; set; }
        public virtual DbSet<UnitUnit> UnitUnit { get; set; }
        public virtual DbSet<UnitUnitAction> UnitUnitAction { get; set; }
        public virtual DbSet<UnitUnitDescription> UnitUnitDescription { get; set; }
        public virtual DbSet<UnitUnitEquipment> UnitUnitEquipment { get; set; }
        public virtual DbSet<UnitUnitIngredient> UnitUnitIngredient { get; set; }
        public virtual DbSet<UnitUnitLegacyData> UnitUnitLegacyData { get; set; }
        public virtual DbSet<UnitUnitMaterial> UnitUnitMaterial { get; set; }
        public virtual DbSet<UnitUnitMeasurement> UnitUnitMeasurement { get; set; }
        public virtual DbSet<V> V { get; set; }
        public virtual DbSet<VAuthor> VAuthor { get; set; }
        public virtual DbSet<VAuthorLanguage> VAuthorLanguage { get; set; }
        public virtual DbSet<VUnit> VUnit { get; set; }
        public virtual DbSet<VWork> VWork { get; set; }
        public virtual DbSet<VWorkAuthor> VWorkAuthor { get; set; }
        public virtual DbSet<Work> Work { get; set; }
        public virtual DbSet<WorkAuthor> WorkAuthor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=66.165.137.57;Database=Sifter;User Id=sifter;Password=saffronsky;Trusted_Connection=False;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<A>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("a");

                entity.Property(e => e.I).HasColumnName("i");

                entity.Property(e => e.J).HasColumnName("j");
            });

            modelBuilder.Entity<Action>(entity =>
            {
                entity.Property(e => e.ActionId)
                    .HasColumnName("ActionID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [ActionSeq])");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AdminHoneypotLoginattempt>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("admin_honeypot_loginattempt", "old");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ip_address")
                    .HasMaxLength(39);

                entity.Property(e => e.Path)
                    .HasColumnName("path")
                    .HasColumnType("ntext");

                entity.Property(e => e.SessionKey)
                    .HasColumnName("session_key")
                    .HasMaxLength(50);

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasMaxLength(36);

                entity.Property(e => e.UserAgent)
                    .HasColumnName("user_agent")
                    .HasColumnType("ntext");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<AuthGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("auth_group", "old");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<AuthGroupPermissions>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("auth_group_permissions", "old");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            });

            modelBuilder.Entity<AuthPermission>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("auth_permission", "old");

                entity.Property(e => e.Codename)
                    .HasColumnName("codename")
                    .HasMaxLength(100);

                entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<AuthUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("auth_user", "old");

                entity.Property(e => e.DateJoined)
                    .HasColumnName("date_joined")
                    .HasMaxLength(36);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(254);

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(30);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IsStaff)
                    .HasColumnName("is_staff")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IsSuperuser)
                    .HasColumnName("is_superuser")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LastLogin)
                    .HasColumnName("last_login")
                    .HasMaxLength(36);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(30);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(128);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<AuthUserGroups>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("auth_user_groups", "old");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<AuthUserUserPermissions>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("auth_user_user_permissions", "old");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.AuthorId)
                    .HasColumnName("AuthorID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [AuthorSeq])");

                entity.Property(e => e.AlsoKnownAs).HasMaxLength(100);

                entity.Property(e => e.BirthCountryId).HasColumnName("BirthCountryID");

                entity.Property(e => e.Comments)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.FullName).HasMaxLength(400);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsOrganization)
                    .HasMaxLength(3)
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

            modelBuilder.Entity<AuthorAuthor>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("author_author");

                entity.Property(e => e.Accuracy)
                    .HasColumnName("accuracy")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ActivityEnds).HasColumnName("activity_ends");

                entity.Property(e => e.ActivityEndsEra)
                    .HasColumnName("activity_ends_era")
                    .HasMaxLength(1);

                entity.Property(e => e.ActivityStarts).HasColumnName("activity_starts");

                entity.Property(e => e.ActivityStartsEra)
                    .HasColumnName("activity_starts_era")
                    .HasMaxLength(1);

                entity.Property(e => e.AlsoKnownAs)
                    .HasColumnName("also_known_as")
                    .HasMaxLength(255);

                entity.Property(e => e.BirthCityId).HasColumnName("birth_city_id");

                entity.Property(e => e.BirthCountryId).HasColumnName("birth_country_id");

                entity.Property(e => e.BirthDay).HasColumnName("birth_day");

                entity.Property(e => e.BirthEra)
                    .HasColumnName("birth_era")
                    .HasMaxLength(1);

                entity.Property(e => e.BirthMonth).HasColumnName("birth_month");

                entity.Property(e => e.BirthRegionId).HasColumnName("birth_region_id");

                entity.Property(e => e.BirthYear).HasColumnName("birth_year");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("ntext");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.DeathCityId).HasColumnName("death_city_id");

                entity.Property(e => e.DeathCountryId).HasColumnName("death_country_id");

                entity.Property(e => e.DeathDay).HasColumnName("death_day");

                entity.Property(e => e.DeathEra)
                    .HasColumnName("death_era")
                    .HasMaxLength(1);

                entity.Property(e => e.DeathMonth).HasColumnName("death_month");

                entity.Property(e => e.DeathRegionId).HasColumnName("death_region_id");

                entity.Property(e => e.DeathYear).HasColumnName("death_year");

                entity.Property(e => e.FamilyName)
                    .HasColumnName("family_name")
                    .HasMaxLength(255);

                entity.Property(e => e.GenderId).HasColumnName("gender_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsOrganization)
                    .HasColumnName("is_organization")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.OccupationId).HasColumnName("occupation_id");

                entity.Property(e => e.PenName)
                    .HasColumnName("pen_name")
                    .HasMaxLength(255);

                entity.Property(e => e.Sources)
                    .HasColumnName("sources")
                    .HasColumnType("ntext");

                entity.Property(e => e.Spouse)
                    .HasColumnName("spouse")
                    .HasMaxLength(255);

                entity.Property(e => e.TitleId).HasColumnName("title_id");

                entity.Property(e => e.ValidName)
                    .HasColumnName("valid_name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<AuthorAuthorLanguagesKnown>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("author_author_languages_known");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LanguageId).HasColumnName("language_id");
            });

            modelBuilder.Entity<AuthorAuthorLegacyInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("author_author_legacy_info");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LegacyId).HasColumnName("legacy_id");
            });

            modelBuilder.Entity<AuthorContributor>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("author_contributor");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.NameId).HasColumnName("name_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");
            });

            modelBuilder.Entity<AuthorGender>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("author_gender", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<AuthorLanguage>(entity =>
            {
                entity.Property(e => e.AuthorLanguageId)
                    .HasColumnName("AuthorLanguageID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [AuthorLanguageSeq])");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");
            });

            modelBuilder.Entity<AuthorLegacy>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("author_legacy");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LegacyData)
                    .HasColumnName("legacy_data")
                    .HasColumnType("ntext");

                entity.Property(e => e.LegacyId).HasColumnName("legacy_id");
            });

            modelBuilder.Entity<AuthorOccupation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("author_occupation");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<AuthorRole>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("author_role", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<AuthorTitle>(entity =>
            {
                entity.ToTable("author_title");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [CategorySeq])");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityId)
                    .HasColumnName("CityID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [CitySeq])");

                entity.Property(e => e.AlternateNames).HasMaxLength(200);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.FeatureCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GeoNameId).HasColumnName("GeoNameID");

                entity.Property(e => e.Latitude).HasColumnType("decimal(8, 5)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(8, 5)");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NameAscii).HasMaxLength(100);

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.SearchNames).HasMaxLength(4000);

                entity.Property(e => e.Slug).HasMaxLength(100);

                entity.Property(e => e.TimeZone)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CommonCurrency>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("common_currency");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<CommonLanguage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("common_language");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryId)
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

                entity.Property(e => e.GeoNameId).HasColumnName("GeoNameID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NameAscii).HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Slug).HasMaxLength(100);

                entity.Property(e => e.Tld)
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

            modelBuilder.Entity<DjangoAdminLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("django_admin_log", "old");

                entity.Property(e => e.ActionFlag).HasColumnName("action_flag");

                entity.Property(e => e.ActionTime)
                    .HasColumnName("action_time")
                    .HasMaxLength(36);

                entity.Property(e => e.ChangeMessage)
                    .HasColumnName("change_message")
                    .HasColumnType("ntext");

                entity.Property(e => e.ContentTypeId).HasColumnName("content_type_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_id")
                    .HasColumnType("ntext");

                entity.Property(e => e.ObjectRepr)
                    .HasColumnName("object_repr")
                    .HasMaxLength(200);

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<DjangoContentType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("django_content_type", "old");

                entity.Property(e => e.AppLabel)
                    .HasColumnName("app_label")
                    .HasMaxLength(100);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<DjangoMigrations>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("django_migrations", "old");

                entity.Property(e => e.App)
                    .HasColumnName("app")
                    .HasMaxLength(255);

                entity.Property(e => e.Applied)
                    .HasColumnName("applied")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<DjangoSession>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("django_session", "old");

                entity.Property(e => e.ExpireDate)
                    .HasColumnName("expire_date")
                    .HasMaxLength(36);

                entity.Property(e => e.SessionData)
                    .HasColumnName("session_data")
                    .HasColumnType("ntext");

                entity.Property(e => e.SessionKey)
                    .HasColumnName("session_key")
                    .HasMaxLength(40);
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

            modelBuilder.Entity<L>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("l");

                entity.Property(e => e.J).HasColumnName("j");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.LanguageId)
                    .HasColumnName("LanguageID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [LanguageSeq])");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<LexiconDefinition>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("lexicon_definition", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<LexiconVariety>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("lexicon_variety", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<LocationCity>(entity =>
            {
                entity.ToTable("location_city");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AlternateNames)
                    .HasColumnName("alternate_names")
                    .HasColumnType("ntext");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.DisplayName)
                    .HasColumnName("display_name")
                    .HasMaxLength(200);

                entity.Property(e => e.FeatureCode)
                    .HasColumnName("feature_code")
                    .HasMaxLength(10);

                entity.Property(e => e.GeonameId).HasColumnName("geoname_id");

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.Longitude).HasColumnName("longitude");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.NameAscii)
                    .HasColumnName("name_ascii")
                    .HasMaxLength(200);

                entity.Property(e => e.Population).HasColumnName("population");

                entity.Property(e => e.RegionId).HasColumnName("region_id");

                entity.Property(e => e.SearchNames)
                    .HasColumnName("search_names")
                    .HasColumnType("ntext");

                entity.Property(e => e.Slug)
                    .HasColumnName("slug")
                    .HasMaxLength(50);

                entity.Property(e => e.Timezone)
                    .HasColumnName("timezone")
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<LocationCountry>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("location_country");

                entity.Property(e => e.AlternateNames)
                    .HasColumnName("alternate_names")
                    .HasColumnType("ntext");

                entity.Property(e => e.Code2)
                    .HasColumnName("code2")
                    .HasMaxLength(2);

                entity.Property(e => e.Code3)
                    .HasColumnName("code3")
                    .HasMaxLength(3);

                entity.Property(e => e.Continent)
                    .HasColumnName("continent")
                    .HasMaxLength(2);

                entity.Property(e => e.GeonameId).HasColumnName("geoname_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.NameAscii)
                    .HasColumnName("name_ascii")
                    .HasMaxLength(200);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(20);

                entity.Property(e => e.Slug)
                    .HasColumnName("slug")
                    .HasMaxLength(50);

                entity.Property(e => e.Tld)
                    .HasColumnName("tld")
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<LocationRegion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("location_region");

                entity.Property(e => e.AlternateNames)
                    .HasColumnName("alternate_names")
                    .HasColumnType("ntext");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.DisplayName)
                    .HasColumnName("display_name")
                    .HasMaxLength(200);

                entity.Property(e => e.GeonameCode)
                    .HasColumnName("geoname_code")
                    .HasMaxLength(50);

                entity.Property(e => e.GeonameId).HasColumnName("geoname_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.NameAscii)
                    .HasColumnName("name_ascii")
                    .HasMaxLength(200);

                entity.Property(e => e.Slug)
                    .HasColumnName("slug")
                    .HasMaxLength(50);
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

            modelBuilder.Entity<ParticularAction>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_action", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ParticularCategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_category", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Lft).HasColumnName("lft");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Rght).HasColumnName("rght");

                entity.Property(e => e.TreeId).HasColumnName("tree_id");
            });

            modelBuilder.Entity<ParticularClassification>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_classification", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Lft).HasColumnName("lft");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Rght).HasColumnName("rght");

                entity.Property(e => e.TreeId).HasColumnName("tree_id");
            });

            modelBuilder.Entity<ParticularDescription>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_description", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ParticularEquipment>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_equipment", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ParticularGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_group", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Lft).HasColumnName("lft");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Rght).HasColumnName("rght");

                entity.Property(e => e.TreeId).HasColumnName("tree_id");
            });

            modelBuilder.Entity<ParticularIngredient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_ingredient", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ParticularMaterial>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_material", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ParticularMeasurement>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_measurement", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<ParticularParticular>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_particular", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<ParticularType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("particular_type", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Lft).HasColumnName("lft");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Rght).HasColumnName("rght");

                entity.Property(e => e.TreeId).HasColumnName("tree_id");
            });

            modelBuilder.Entity<PublicationDigitalsurrogate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_digitalsurrogate", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Link)
                    .HasColumnName("link")
                    .HasColumnType("ntext");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<PublicationLegacywork>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_legacywork");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LegacyAuthorId).HasColumnName("legacy_author_id");

                entity.Property(e => e.LegacyData)
                    .HasColumnName("legacy_data")
                    .HasColumnType("ntext");

                entity.Property(e => e.LegacyId).HasColumnName("legacy_id");
            });

            modelBuilder.Entity<PublicationPublisher>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_publisher");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<PublicationRepository>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_repository", "old");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(1);

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<PublicationSeries>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_series", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<PublicationWork>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_work");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Citations)
                    .HasColumnName("citations")
                    .HasColumnType("ntext");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("ntext");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.CurrencyId).HasColumnName("currency_id");

                entity.Property(e => e.EditionNumber).HasColumnName("edition_number");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Isbn)
                    .HasColumnName("isbn")
                    .HasMaxLength(15);

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Pages).HasColumnName("pages");

                entity.Property(e => e.PublicationEra)
                    .HasColumnName("publication_era")
                    .HasMaxLength(1);

                entity.Property(e => e.PublicationHistory)
                    .HasColumnName("publication_history")
                    .HasColumnType("ntext");

                entity.Property(e => e.PublicationPlaceId).HasColumnName("publication_place_id");

                entity.Property(e => e.PublicationYear).HasColumnName("publication_year");

                entity.Property(e => e.PublisherId).HasColumnName("publisher_id");

                entity.Property(e => e.SeriesId).HasColumnName("series_id");

                entity.Property(e => e.Sources)
                    .HasColumnName("sources")
                    .HasColumnType("ntext");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("ntext");

                entity.Property(e => e.TitleAbbreviated)
                    .HasColumnName("title_abbreviated")
                    .HasColumnType("ntext");

                entity.Property(e => e.TitleEnglish)
                    .HasColumnName("title_english")
                    .HasColumnType("ntext");

                entity.Property(e => e.TitleLiteral)
                    .HasColumnName("title_literal")
                    .HasColumnType("ntext");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.Property(e => e.Volume).HasColumnName("volume");

                entity.Property(e => e.Volumes).HasColumnName("volumes");
            });

            modelBuilder.Entity<PublicationWorkAuthor>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_work_author");

                entity.Property(e => e.ContributorId).HasColumnName("contributor_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.WorkId).HasColumnName("work_id");
            });

            modelBuilder.Entity<PublicationWorkElectronicCopies>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_work_electronic_copies", "old");

                entity.Property(e => e.DigitalsurrogateId).HasColumnName("digitalsurrogate_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.WorkId).HasColumnName("work_id");
            });

            modelBuilder.Entity<PublicationWorkKnownCopies>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_work_known_copies", "old");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RepositoryId).HasColumnName("repository_id");

                entity.Property(e => e.WorkId).HasColumnName("work_id");
            });

            modelBuilder.Entity<PublicationWorkLanguage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_work_language");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LanguageId).HasColumnName("language_id");

                entity.Property(e => e.WorkId).HasColumnName("work_id");
            });

            modelBuilder.Entity<PublicationWorkLegacyData>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_work_legacy_data");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LegacyworkId).HasColumnName("legacywork_id");

                entity.Property(e => e.WorkId).HasColumnName("work_id");
            });

            modelBuilder.Entity<PublicationWorktype>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("publication_worktype", "old");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.Property(e => e.PublisherId)
                    .HasColumnName("PublisherID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PublisherSeq])");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<RawAuthor>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RawAuthor", "old");

                entity.Property(e => e.Affiliation).HasMaxLength(255);

                entity.Property(e => e.AuthorId).HasColumnName("Author_ID");

                entity.Property(e => e.BirthCity)
                    .HasColumnName("Birth_City")
                    .HasMaxLength(255);

                entity.Property(e => e.BirthDt)
                    .HasColumnName("Birth_DT")
                    .HasMaxLength(255);

                entity.Property(e => e.Citations).HasMaxLength(255);

                entity.Property(e => e.Comments).HasMaxLength(255);

                entity.Property(e => e.CountryOfBirth)
                    .HasColumnName("Country_Of_Birth")
                    .HasMaxLength(255);

                entity.Property(e => e.CountryOfDeath)
                    .HasColumnName("Country_Of_Death")
                    .HasMaxLength(255);

                entity.Property(e => e.DeathDt)
                    .HasColumnName("Death_DT")
                    .HasMaxLength(255);

                entity.Property(e => e.DigitizedCopy)
                    .HasColumnName("Digitized_Copy")
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .HasColumnName("First_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.Keyword).HasMaxLength(255);

                entity.Property(e => e.Language).HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .HasColumnName("Last_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.LocationDigitizedTexts)
                    .HasColumnName("Location_Digitized_Texts")
                    .HasMaxLength(255);

                entity.Property(e => e.MarriedTo)
                    .HasColumnName("Married_To")
                    .HasMaxLength(255);

                entity.Property(e => e.PenName)
                    .HasColumnName("Pen_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.PlaceOfDeath)
                    .HasColumnName("Place of Death")
                    .HasMaxLength(255);

                entity.Property(e => e.ShortName)
                    .HasColumnName("Short_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.SortDate)
                    .HasColumnName("Sort_Date")
                    .HasMaxLength(255);

                entity.Property(e => e.TextFromBibliography2002).HasColumnName("Text_From_Bibliography_2002");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.WhenActive)
                    .HasColumnName("When_Active")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<RawCountry>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RawCountry", "old");

                entity.Property(e => e.Country).HasMaxLength(255);
            });

            modelBuilder.Entity<RawInfoType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RawInfoType", "old");

                entity.Property(e => e.InfoType).HasMaxLength(255);
            });

            modelBuilder.Entity<RawUnit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RawUnit", "old");

                entity.Property(e => e.Attribution).HasMaxLength(255);

                entity.Property(e => e.AuthorName)
                    .HasColumnName("Author_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.Comments).HasMaxLength(255);

                entity.Property(e => e.InfoType)
                    .HasColumnName("Info_Type")
                    .HasMaxLength(255);

                entity.Property(e => e.LocatorId)
                    .HasColumnName("Locator_ID")
                    .HasMaxLength(255);

                entity.Property(e => e.NumbersAndWork).HasColumnName("Numbers_And_Work");

                entity.Property(e => e.Order)
                    .HasColumnName("Order#")
                    .HasMaxLength(255);

                entity.Property(e => e.Page)
                    .HasColumnName("Page#")
                    .HasMaxLength(255);

                entity.Property(e => e.ShortName)
                    .HasColumnName("Short_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.Text).HasMaxLength(255);

                entity.Property(e => e.TextAndWork)
                    .HasColumnName("Text_And_Work")
                    .HasMaxLength(255);

                entity.Property(e => e.UnitId).HasColumnName("Unit_ID");

                entity.Property(e => e.UnitIdReal)
                    .HasColumnName("Unit_ID_Real")
                    .HasMaxLength(255);

                entity.Property(e => e.UnitPartField)
                    .HasColumnName("Unit_Part_Field")
                    .HasMaxLength(255);

                entity.Property(e => e.UnitTitleEnglish)
                    .HasColumnName("Unit_Title_English")
                    .HasMaxLength(255);

                entity.Property(e => e.UnitTitleLit)
                    .HasColumnName("Unit_Title_Lit")
                    .HasMaxLength(255);

                entity.Property(e => e.UnitTitleStd)
                    .HasColumnName("Unit_Title_Std")
                    .HasMaxLength(255);

                entity.Property(e => e.WorkId)
                    .HasColumnName("Work_ID")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<RawWhatType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RawWhatType", "old");

                entity.Property(e => e.WhatType)
                    .HasColumnName("What/Type")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<RawWork>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RawWork", "old");

                entity.Property(e => e.AuthorId).HasColumnName("Author_ID");

                entity.Property(e => e.AuthorId2)
                    .HasColumnName("Author_ID2")
                    .HasMaxLength(255);

                entity.Property(e => e.AuthorName)
                    .HasColumnName("Author_Name")
                    .HasMaxLength(255);

                entity.Property(e => e.Citations).HasMaxLength(255);

                entity.Property(e => e.CollectiveWorkTitle)
                    .HasColumnName("Collective_Work_Title")
                    .HasMaxLength(255);

                entity.Property(e => e.CopiesKnown)
                    .HasColumnName("Copies_Known")
                    .HasMaxLength(255);

                entity.Property(e => e.CountryOfPublication)
                    .HasColumnName("Country_Of_Publication")
                    .HasMaxLength(255);

                entity.Property(e => e.Date).HasMaxLength(255);

                entity.Property(e => e.DatePrecise).HasColumnName("Date_Precise");

                entity.Property(e => e.DigitizedVersion)
                    .HasColumnName("Digitized_Version")
                    .HasMaxLength(255);

                entity.Property(e => e.EditionNumber)
                    .HasColumnName("Edition_Number")
                    .HasMaxLength(255);

                entity.Property(e => e.Editor).HasMaxLength(255);

                entity.Property(e => e.Isbn)
                    .HasColumnName("ISBN#")
                    .HasMaxLength(255);

                entity.Property(e => e.Languages).HasMaxLength(255);

                entity.Property(e => e.PostPublicationHist)
                    .HasColumnName("Post_Publication_hist")
                    .HasMaxLength(255);

                entity.Property(e => e.PriceOfWork)
                    .HasColumnName("Price_of_work")
                    .HasMaxLength(255);

                entity.Property(e => e.ProvenanceOfSourceCopy)
                    .HasColumnName("Provenance_of_Source_Copy")
                    .HasMaxLength(255);

                entity.Property(e => e.PubPlaceEng)
                    .HasColumnName("Pub_Place_Eng")
                    .HasMaxLength(255);

                entity.Property(e => e.PubPlaceLit)
                    .HasColumnName("Pub_Place_Lit")
                    .HasMaxLength(255);

                entity.Property(e => e.Publisher).HasMaxLength(255);

                entity.Property(e => e.SortDate)
                    .HasColumnName("Sort_date")
                    .HasMaxLength(255);

                entity.Property(e => e.SourceCopy)
                    .HasColumnName("Source_Copy")
                    .HasMaxLength(255);

                entity.Property(e => e.SourceDocument)
                    .HasColumnName("Source_Document")
                    .HasMaxLength(255);

                entity.Property(e => e.TitleAbbreviated)
                    .HasColumnName("Title_Abbreviated")
                    .HasMaxLength(255);

                entity.Property(e => e.TitleEng)
                    .HasColumnName("Title_Eng")
                    .HasMaxLength(255);

                entity.Property(e => e.TitleLit)
                    .HasColumnName("Title_Lit")
                    .HasMaxLength(255);

                entity.Property(e => e.TitleStd)
                    .HasColumnName("Title_Std")
                    .HasMaxLength(255);

                entity.Property(e => e.Translator).HasMaxLength(255);

                entity.Property(e => e.Volumes).HasMaxLength(255);

                entity.Property(e => e.WorkId).HasColumnName("Work_ID");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.RegionId)
                    .HasColumnName("RegionID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [RegionSeq])");

                entity.Property(e => e.AlternateNames).HasMaxLength(200);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.DisplayName).HasMaxLength(100);

                entity.Property(e => e.GeoNameCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GeoNameId).HasColumnName("GeoNameID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NameAscii).HasMaxLength(100);

                entity.Property(e => e.Slug).HasMaxLength(100);
            });

            modelBuilder.Entity<RegistrationRegistrationprofile>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("registration_registrationprofile", "old");

                entity.Property(e => e.Activated)
                    .HasColumnName("activated")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ActivationKey)
                    .HasColumnName("activation_key")
                    .HasMaxLength(40);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<RegistrationSupervisedregistrationprofile>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("registration_supervisedregistrationprofile", "old");

                entity.Property(e => e.RegistrationprofilePtrId).HasColumnName("registrationprofile_ptr_id");
            });

            modelBuilder.Entity<TreeCategory>(entity =>
            {
                entity.HasKey(e => e.CategorySeq)
                    .HasName("PK__TreeCate__74C21A5416B5AD0B");

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

                entity.Property(e => e.FkfullList).HasColumnName("FKFullList");

                entity.Property(e => e.FkjoinCol)
                    .HasColumnName("FKJoinCol")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fktable)
                    .HasColumnName("FKTable")
                    .HasMaxLength(100)
                    .IsUnicode(false);

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
                entity.Property(e => e.UnitId)
                    .HasColumnName("UnitID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [UnitSeq])");

                entity.Property(e => e.Attribution).HasMaxLength(200);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.LiteralTitle).HasMaxLength(100);

                entity.Property(e => e.Text).HasMaxLength(4000);

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.Property(e => e.TitleEnglish).HasMaxLength(100);

                entity.Property(e => e.WorkId).HasColumnName("WorkID");
            });

            modelBuilder.Entity<UnitCategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_category");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<UnitLegacyunit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_legacyunit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LegacyData)
                    .HasColumnName("legacy_data")
                    .HasColumnType("ntext");

                entity.Property(e => e.LegacyId)
                    .HasColumnName("legacy_id")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UnitPeople>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_people");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.ExistingRecordId).HasColumnName("existing_record_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UnitUnit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_unit");

                entity.Property(e => e.AttributionId).HasColumnName("attribution_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasColumnType("ntext");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnName("created_on")
                    .HasMaxLength(36);

                entity.Property(e => e.EndPage).HasColumnName("end_page");

                entity.Property(e => e.EnglishTitle)
                    .HasColumnName("english_title")
                    .HasColumnType("ntext");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LiteralTitle)
                    .HasColumnName("literal_title")
                    .HasColumnType("ntext");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnName("modified_on")
                    .HasMaxLength(36);

                entity.Property(e => e.Order).HasColumnName("order");

                entity.Property(e => e.RelatedWorkId).HasColumnName("related_work_id");

                entity.Property(e => e.RomanNumerals)
                    .HasColumnName("roman_numerals")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.StartPage).HasColumnName("start_page");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("ntext");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UnitUnitAction>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_unit_action", "old");

                entity.Property(e => e.ActionId).HasColumnName("action_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");
            });

            modelBuilder.Entity<UnitUnitDescription>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_unit_description", "old");

                entity.Property(e => e.DescriptionId).HasColumnName("description_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");
            });

            modelBuilder.Entity<UnitUnitEquipment>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_unit_equipment", "old");

                entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");
            });

            modelBuilder.Entity<UnitUnitIngredient>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_unit_ingredient", "old");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");
            });

            modelBuilder.Entity<UnitUnitLegacyData>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_unit_legacy_data");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LegacyunitId).HasColumnName("legacyunit_id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");
            });

            modelBuilder.Entity<UnitUnitMaterial>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_unit_material", "old");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MaterialId).HasColumnName("material_id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");
            });

            modelBuilder.Entity<UnitUnitMeasurement>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("unit_unit_measurement", "old");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MeasurementId).HasColumnName("measurement_id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");
            });

            modelBuilder.Entity<V>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("v");

                entity.Property(e => e.I).HasColumnName("i");

                entity.Property(e => e.J).HasColumnName("j");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VAuthor>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vAuthor");

                entity.Property(e => e.AlsoKnownAs).HasMaxLength(100);

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.BirthCountry).HasMaxLength(100);

                entity.Property(e => e.Comments)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.FullName).HasMaxLength(400);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsOrganization)
                    .HasMaxLength(3)
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

                entity.Property(e => e.Author).HasMaxLength(300);

                entity.Property(e => e.AuthorLanguageId).HasColumnName("AuthorLanguageID");

                entity.Property(e => e.Language).HasMaxLength(100);
            });

            modelBuilder.Entity<VUnit>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vUnit");

                entity.Property(e => e.Attribution).HasMaxLength(200);

                entity.Property(e => e.BirthCountry)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(400);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Language).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.PubCity).HasMaxLength(100);

                entity.Property(e => e.PubCountry).HasMaxLength(100);

                entity.Property(e => e.PubRegion).HasMaxLength(100);

                entity.Property(e => e.Publisher).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.Property(e => e.UnitLiteralTitle).HasMaxLength(100);

                entity.Property(e => e.UnitText).HasMaxLength(4000);

                entity.Property(e => e.UnitTitle).HasMaxLength(100);

                entity.Property(e => e.UnitTitleEnglish).HasMaxLength(100);
            });

            modelBuilder.Entity<VWork>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vWork");

                entity.Property(e => e.BirthCountry).HasMaxLength(100);

                entity.Property(e => e.Editor).HasMaxLength(400);

                entity.Property(e => e.FullName).HasMaxLength(400);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Language).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(200);

                entity.Property(e => e.PubCity).HasMaxLength(100);

                entity.Property(e => e.PubCountry).HasMaxLength(100);

                entity.Property(e => e.PubRegion).HasMaxLength(100);

                entity.Property(e => e.Publisher).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.TitleEnglish).HasMaxLength(200);

                entity.Property(e => e.TitleLiteral).HasMaxLength(200);

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
                entity.Property(e => e.WorkId)
                    .HasColumnName("WorkID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [WorkSeq])");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.EditorId).HasColumnName("EditorID");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.Property(e => e.TitleEnglish).HasMaxLength(200);

                entity.Property(e => e.TitleLiteral).HasMaxLength(200);

                entity.Property(e => e.TranslatorId).HasColumnName("TranslatorID");
            });

            modelBuilder.Entity<WorkAuthor>(entity =>
            {
                entity.Property(e => e.WorkAuthorId)
                    .HasColumnName("WorkAuthorID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [WorkAuthorSeq])");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.Role)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.WorkId).HasColumnName("WorkID");
            });

            modelBuilder.HasSequence<int>("ActionSeq").StartsAt(6059);

            modelBuilder.HasSequence<int>("ActionTypeSeq").StartsAt(6059);

            modelBuilder.HasSequence<int>("AuthorLanguageSeq").StartsAt(5296);

            modelBuilder.HasSequence<int>("AuthorRoleSeq").StartsAt(2278);

            modelBuilder.HasSequence<int>("AuthorSeq").StartsAt(5422);

            modelBuilder.HasSequence<int>("CategorySeq").StartsAt(130);

            modelBuilder.HasSequence<int>("CitySeq").StartsAt(16309);

            modelBuilder.HasSequence<int>("CountrySeq").StartsAt(253);

            modelBuilder.HasSequence<int>("CurrencySeq").StartsAt(246);

            modelBuilder.HasSequence<int>("EquipmentSeq").StartsAt(165);

            modelBuilder.HasSequence<int>("LanguageSeq").StartsAt(251);

            modelBuilder.HasSequence<int>("MeasurementSeq").StartsAt(163);

            modelBuilder.HasSequence<int>("ParticularSeq").StartsAt(424);

            modelBuilder.HasSequence<int>("ParticularTypeSeq").StartsAt(424);

            modelBuilder.HasSequence<int>("PublisherSeq").StartsAt(2856);

            modelBuilder.HasSequence<int>("RegionSeq").StartsAt(3960);

            modelBuilder.HasSequence<int>("TitleSeq").StartsAt(450);

            modelBuilder.HasSequence<int>("UnitSeq").StartsAt(124639);

            modelBuilder.HasSequence<int>("WorkAuthorSeq").StartsAt(5508);

            modelBuilder.HasSequence<int>("WorkSeq").StartsAt(5216);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
