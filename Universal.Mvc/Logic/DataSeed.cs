using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universal.Core;
using Universal.Entities;
using Universal.Services;

namespace Universal.Mvc
{
    public static class DataSeed
    {
        public static void InitData(this IApplicationBuilder app)
        {
            #region 自动创建数据库
            // Microsoft.EntityFrameworkCore.Tools Microsoft.EntityFrameworkCore.SqlServer.Design
            //dotnet ef migrations add InitialEFDbContext -c EFDbContext -o Data/Migrations/DemoDB
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<EFDbContext>();
                /*
                 * System.Data.SqlClient.SqlException:“将 FOREIGN KEY 约束 'FK_SysPermission_SysRole_RoleId' 引入表 'SysPermission' 
                 * 可能会导致循环或多重级联路径。请指定 ON DELETE NO ACTION 或 ON UPDATE NO ACTION，或修改其他 FOREIGN KEY 约束。
                 * 无法创建约束。请参阅前面的错误消息。
                 */
                context.Database.Migrate();
            }
            #endregion

            #region 初始化数据
            var _sysUserService = EnginContext.Current.Resolve<ISysUserService>();
            var _sysRoleService = EnginContext.Current.Resolve<ISysRoleService>();

            SysUser sysUser = new SysUser();
            if (!_sysUserService.ExistUser())
            {
                sysUser.Id = Guid.NewGuid();
                sysUser.Account = "admin";
                sysUser.Name = "超级管理员";
                sysUser.Email = "";
                sysUser.MobilePhone = "";
                sysUser.Salt = EncryptorHelper.CreateSaltKey();
                sysUser.Password = EncryptorHelper.GetMD5(sysUser.Account + sysUser.Salt);
                sysUser.Enabled = true;
                sysUser.IsAdmin = true;
                sysUser.CreationTime = DateTime.Now;
                sysUser.LoginLock = false;
                sysUser.IsDeleted = false;
                _sysUserService.InsertSysUser(sysUser);
            }

            if (!_sysRoleService.ExistRole())
            {
                SysRole sysRole = new SysRole()
                {
                    Id = Guid.NewGuid(),
                    Name = "超级管理员",
                    Creator = sysUser.Id,
                    CreationTime = DateTime.Now
                };
                _sysRoleService.InsertRole(sysRole);
            }
            #endregion
        }
    }
}
