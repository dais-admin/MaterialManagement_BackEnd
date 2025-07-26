using DAIS.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace DAIS.DataAccess.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            // Enable tracking of original values
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public AppDbContext()
        {
        }

        // DbSet properties
        public DbSet<ExcelReaderMetadata> ExcelReaderMetadata { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleFeature> RoleFeatures { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<WorkPackage> WorkPackages { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AuditLogs> AuditLogs { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<LocationOperation> LocationOperations { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<MaterialDocument> MaterialDocuments { get; set; }
        public DbSet<MaterialServiceProvider> ServiceProviders { get; set; }
        public DbSet<MaterialWarranty> MaterialWarranties { get; set; }
        public DbSet<MaterialMaintenance> MaterialMaintenances { get; set; }
        public DbSet<MaterialHardware> MaterialHardwares { get; set; }
        public DbSet<MaterialSoftware> MaterialSoftwares { get; set; }
        public DbSet<MaterialApproval> MaterialApprovals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<MaterialMeasurement> MaterialMeasurements { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<SubDivision> SubDivisions { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<BulkUploadDetail> BulkUploadDetails { get; set; }
        public DbSet<MaterialIssueRecieveVoucher> MaterialIssueRecieveVouchers { get; set; }
        public DbSet<MaterialVoucherTrancation> MaterialVoucherTrancations { get; set; }

        public override int SaveChanges()
        {
            return SaveChangesWithAudit();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesWithAuditAsync(cancellationToken);
        }

        private int SaveChangesWithAudit()
        {
            var auditEntries = OnBeforeSaveChanges();
            var result = base.SaveChanges();
            return result;
        }

        private async Task<int> SaveChangesWithAuditAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            var auditLogs = new List<AuditLogs>();

            // First, collect all entries that need to be audited
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                if (entry.Entity is AuditLogs || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;

                // Set the user ID
                if (_httpContextAccessor?.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    auditEntry.UserId = _httpContextAccessor.HttpContext.User.Claims
                        .FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                }

                if (string.IsNullOrEmpty(auditEntry.UserId))
                {
                    auditEntry.UserId = "System";
                }

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;

                    // Handle UpdatedBy property
                    if (propertyName == "UpdatedBy" && property.CurrentValue != null)
                    {
                        auditEntry.UserId = property.CurrentValue.ToString();
                        continue;
                    }

                    // Skip certain common properties if needed
                    if (propertyName == "CreatedAt" || propertyName == "UpdatedAt")
                        continue;

                    // Handle primary key
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    // Handle based on state
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            if (property.CurrentValue != null)
                            {
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            if (property.OriginalValue != null)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                            }
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                var databaseValues = entry.GetDatabaseValues();
                                var originalValue = databaseValues?[propertyName];
                                var currentValue = property.CurrentValue;

                                if (!Equals(originalValue, currentValue))
                                {
                                    auditEntry.ChangedColumns.Add(propertyName);
                                    auditEntry.AuditType = AuditType.Update;
                                    auditEntry.OldValues[propertyName] = originalValue;
                                    auditEntry.NewValues[propertyName] = currentValue;
                                }
                            }
                            break;
                    }
                }

                if (auditEntry.HasChanges())
                {
                    auditEntries.Add(auditEntry);
                    auditLogs.Add(auditEntry.ToAudit());
                }
            }

            // Now add all audit logs at once
            if (auditLogs.Any())
            {
                Set<AuditLogs>().AddRange(auditLogs);
            }

            return auditEntries;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure AuditLogs entity
            modelBuilder.Entity<AuditLogs>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(450);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TableName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DateTime).IsRequired();
                entity.Property(e => e.PrimaryKey).IsRequired();
                entity.Property(e => e.OldValues).HasColumnType("nvarchar(max)");
                entity.Property(e => e.NewValues).HasColumnType("nvarchar(max)");
                entity.Property(e => e.AffectedColumns).HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<User>().ToTable("AspNetUsers");
            modelBuilder.Entity<User>()
               .HasOne(m => m.Region).WithMany().HasForeignKey(m => m.RegionId);
            modelBuilder.Entity<User>()
               .HasOne(m => m.Location).WithMany().HasForeignKey(m => m.LocationId);
            modelBuilder.Entity<User>()
              .HasOne(m => m.Project).WithMany().HasForeignKey(m => m.ProjectId);

            // Add unique constraint for Project
            modelBuilder.Entity<Project>()
                .HasIndex(s => new { s.ProjectName })
                .IsUnique();
            // Add unique constraint for WorkPackage
            modelBuilder.Entity<WorkPackage>()
                .HasIndex(s => new { s.WorkPackageName })
                .IsUnique();
            // Add unique constraint for SubDivision
            modelBuilder.Entity<SubDivision>()
                .HasIndex(s => new { s.SubDivisionName, s.DivisionId })
                .IsUnique();
            // Add unique constraint for Division
            modelBuilder.Entity<Division>()
                .HasIndex(s => new { s.DivisionName, s.LocationId })
                .IsUnique();
            // Add unique constraint for Location
            modelBuilder.Entity<LocationOperation>()
                .HasIndex(s => new { s.LocationOperationName, s.System })
                .IsUnique();
            // Add unique constraint for Material Type
            modelBuilder.Entity<MaterialType>()
                .HasIndex(s => new { s.TypeName, s.ProjectId })
                .IsUnique();
            // Add unique constraint for Category
            modelBuilder.Entity<Category>()
                .HasIndex(s => new { s.CategoryName, s.ProjectId })
                .IsUnique();
            // Add unique constraint for Manufacturer
            modelBuilder.Entity<Manufacturer>()
                .HasIndex(s => new { s.ManufacturerName })
                .IsUnique();
            // Add unique constraint for Supplier
            modelBuilder.Entity<Supplier>()
                .HasIndex(s => new { s.SupplierName })
                .IsUnique();
            // Add unique constraint for Contractor
            modelBuilder.Entity<Contractor>()
                .HasIndex(s => new { s.ContractorName })
                .IsUnique();
            // Add unique constraint for Measurement
            modelBuilder.Entity<MaterialMeasurement>()
                .HasIndex(s => new { s.MeasurementName })
                .IsUnique();
            // Add unique constraint for DocumentType
            modelBuilder.Entity<DocumentType>()
                .HasIndex(s => new { s.DocumentName })
                .IsUnique();
            // Add unique constraint for DocumentType
            modelBuilder.Entity<Agency>()
                .HasIndex(s => new { s.AgencyName })
                .IsUnique();

            modelBuilder.Entity<RoleFeature>()
            .HasKey(rf => new { rf.Id });

            modelBuilder.Entity<RoleFeature>()
                .HasOne(rf => rf.Role)
                .WithMany(r => r.RoleFeatures)
                .HasForeignKey(rf => rf.RoleId);

            modelBuilder.Entity<RoleFeature>()
                .HasOne(rf => rf.Feature)
                .WithMany()
                .HasForeignKey(rf => rf.FeatureId);

            modelBuilder.Entity<RoleFeature>()
                .HasOne(rf => rf.Permission)
                .WithMany()
                .HasForeignKey(rf => rf.PermissionId);

            modelBuilder.Entity<Material>()
                .HasOne(m => m.MaterialType).WithMany().HasForeignKey(m => m.TypeId);
            modelBuilder.Entity<Material>()
               .HasOne(m => m.Category).WithMany().HasForeignKey(m => m.CategoryId);
            modelBuilder.Entity<Material>()
               .HasOne(m => m.Region).WithMany().HasForeignKey(m => m.RegionId);
            modelBuilder.Entity<Material>()
               .HasOne(m => m.Location).WithMany().HasForeignKey(m => m.LocationId);
            modelBuilder.Entity<Material>()
              .HasOne(m => m.SubDivision).WithMany().HasForeignKey(m => m.SubDivisionId);
            modelBuilder.Entity<Material>()
              .HasOne(m => m.Manufacturer).WithMany().HasForeignKey(m => m.ManufacturerId);
            modelBuilder.Entity<Material>()
              .HasOne(m => m.Supplier).WithMany().HasForeignKey(m => m.SupplierId);
            
            modelBuilder.Entity<Material>()
              .HasOne(m => m.WorkPackage).WithMany().HasForeignKey(m => m.WorkPackageId);

            modelBuilder.Entity<Category>()
             .HasOne(s => s.MaterialType)
             .WithMany(c => c.Categories)
             .HasForeignKey(s => s.MaterialTypeId);
            modelBuilder.Entity<Category>()
              .HasOne(m => m.Project).WithMany().HasForeignKey(m => m.ProjectId);

            modelBuilder.Entity<MaterialDocument>()
             .HasOne(s => s.DocumentType)
             .WithMany(c => c.MaterialDocuments)
             .HasForeignKey(s => s.DocumentTypeId);

            modelBuilder.Entity<MaterialDocument>()
             .HasOne(s => s.Material)
             .WithMany(c => c.MaterialDocuments)
             .HasForeignKey(s => s.MaterialId);

            modelBuilder.Entity<MaterialServiceProvider>()
             .HasOne(m => m.Manufacturer).WithMany().HasForeignKey(m => m.ManufacturerId);

            modelBuilder.Entity<MaterialWarranty>()
             .HasOne(m => m.Manufacturer).WithMany().HasForeignKey(m => m.ManufacturerId);
            modelBuilder.Entity<MaterialWarranty>()
             .HasOne(m => m.Material).WithMany().HasForeignKey(m => m.MaterialId);
                     
            modelBuilder.Entity<MaterialMaintenance>()
             .HasOne(m => m.Material).WithMany().HasForeignKey(m => m.MaterialId);
           
            modelBuilder.Entity<MaterialHardware>()
             .HasOne(m => m.Material).WithMany().HasForeignKey(m => m.MaterialId);
            modelBuilder.Entity<MaterialHardware>()
             .HasOne(m => m.Manufacturer).WithMany().HasForeignKey(m => m.ManufacturerId);
            modelBuilder.Entity<MaterialHardware>()
             .HasOne(m => m.Supplier).WithMany().HasForeignKey(m => m.SupplierId);
            
            modelBuilder.Entity<MaterialSoftware>()
            .HasOne(m => m.Material).WithMany().HasForeignKey(m => m.MaterialId);
            modelBuilder.Entity<MaterialSoftware>()
             .HasOne(m => m.Supplier).WithMany().HasForeignKey(m => m.SupplierId);
           
            modelBuilder.Entity<MaterialApproval>()
            .HasOne(m => m.Material).WithMany().HasForeignKey(m => m.MaterialId);
            modelBuilder.Entity<MaterialApproval>()
            .HasOne(m => m.Reveiwer).WithMany().HasForeignKey(m => m.ReveiwerId);
            modelBuilder.Entity<MaterialApproval>()
            .HasOne(m => m.Approver).WithMany().HasForeignKey(m => m.ApproverId);

            // Updated MaterialIssueRecieveVoucher relationships with DeleteBehavior.NoAction
            modelBuilder.Entity<MaterialIssueRecieveVoucher>()
           .HasOne(m => m.IssueLocation)
           .WithMany()
           .HasForeignKey(m => m.IssueLocationId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MaterialIssueRecieveVoucher>()
           .HasOne(m => m.RecieveLocation)
           .WithMany()
           .HasForeignKey(m => m.RecieveLocationId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MaterialIssueRecieveVoucher>()
           .HasOne(m => m.OnBoardedLocation)
           .WithMany()
           .HasForeignKey(m => m.OnBoardedLocationId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MaterialIssueRecieveVoucher>()
           .HasOne(m => m.Material)
           .WithMany()
           .HasForeignKey(m => m.MaterialId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MaterialVoucherTrancation>()
           .HasOne(m => m.Location)
           .WithMany()
           .HasForeignKey(m => m.LocationId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MaterialVoucherTrancation>()
           .HasOne(m => m.Material)
           .WithMany()
           .HasForeignKey(m => m.MaterialId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MaterialVoucherTrancation>()
           .HasOne(m => m.MaterialIssueRecieveVoucher)
           .WithMany()
           .HasForeignKey(m => m.MaterialIssueRecieveVoucherId)
           .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
