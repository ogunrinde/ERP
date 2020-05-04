using ICSERP.Entities.UserManagament;
using Microsoft.EntityFrameworkCore;

namespace ICSERP.DataContext
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(bc => new { bc.UserId, bc.RoleId });  
            modelBuilder.Entity<UserRole>()
                .HasOne(bc => bc.Users)
                .WithMany(b => b.UserRoles)
                .HasForeignKey(bc => bc.RoleId);  
            modelBuilder.Entity<UserRole>()
                .HasOne(bc => bc.Roles)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(bc => bc.UserId);


            modelBuilder.Entity<RolePermission>()
                .HasKey(bc => new { bc.RoleId, bc.PermissionId });  
            modelBuilder.Entity<RolePermission>()
                .HasOne(bc => bc.Roles)
                .WithMany(b => b.RolePermissions)
                .HasForeignKey(bc => bc.PermissionId);  
            modelBuilder.Entity<RolePermission>()
                .HasOne(bc => bc.Permissions)
                .WithMany(c => c.RolePermissions)
                .HasForeignKey(bc => bc.RoleId);    
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Branch> Branches{get; set;}

        public DbSet<Company> Companies { get; set;}    

        public DbSet<Department> Departments {get; set;}

        public DbSet<Level> Level {get; set;}

        public DbSet<PersonalInformation> PersonalInformation {get; set;}

        public DbSet<Role> Roles {get; set;}

        public DbSet<Unit> Units {get; set;}

        public DbSet<Permission> Permissions {get;set;}

        public DbSet<UserRole> UserRoles {get;set;}

        public DbSet<RolePermission> RolePermissions {get;set;}

        public DbSet<SpecialPermission> SpecialPermissions {get; set;}
    }
}