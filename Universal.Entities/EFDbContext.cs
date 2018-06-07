using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Entities
{
    public class EFDbContext:DbContext
    {
        public EFDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SysUser> SysUsers { get; set; }
        public DbSet<SysUserToken> SysUserTokens { get; set; }
        public DbSet<SysUserLoginLog> SysUserLoginLogs { get; set; }
        public DbSet<SysPermission> SysPermissions { get; set; }
        public DbSet<SysUserRole> SysUserRoles { get; set; }

        public DbSet<SysRole> SysRoles { get; set; }

        public DbSet<SysLog> SysLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysLog>().ToTable("SysLog");
            modelBuilder.Entity<SysLog>().HasKey(l => l.Id);

            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Category>().HasKey(l => l.Id);

            modelBuilder.Entity<SysRole>().ToTable("SysRole");
            modelBuilder.Entity<SysRole>().HasKey(l => l.Id);

            modelBuilder.Entity<SysUser>().ToTable("SysUser");
            modelBuilder.Entity<SysUser>().HasKey(l => l.Id);

            modelBuilder.Entity<SysUserLoginLog>().ToTable("SysUserLoginLog");
            modelBuilder.Entity<SysUserLoginLog>().HasKey(l => l.Id);

            modelBuilder.Entity<SysUserRole>().ToTable("SysUserRole");
            modelBuilder.Entity<SysUserRole>().HasKey(l => l.Id);
            modelBuilder.Entity<SysUserRole>().HasOne(ur => ur.SysRole).WithMany(ur => ur.SysUserRole).HasForeignKey(ur => ur.RoleId);
            modelBuilder.Entity<SysUserRole>().HasOne(ur => ur.SysUser).WithMany(ur => ur.SysUserRoles).HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<SysUserToken>().ToTable("SysUserToken");
            modelBuilder.Entity<SysUserToken>().HasKey(l => l.Id);


            modelBuilder.Entity<SysPermission>().ToTable("SysPermission");
            modelBuilder.Entity<SysPermission>().HasKey(l => l.Id);
            modelBuilder.Entity<SysPermission>().HasOne(p => p.SysRole).WithMany(p => p.SysPermission).HasForeignKey(p => p.RoleId);
            modelBuilder.Entity<SysPermission>().HasOne(p => p.Category).WithMany().HasForeignKey(p=>p.CategoryId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
