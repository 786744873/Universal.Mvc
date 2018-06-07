using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Universal.Core;
using Universal.Entities;
using Universal.Entities.Dto;

namespace Universal.Services
{
    public class SysUserService : ISysUserService
    {
        private const string MODEL_KEY = "Universal.services.user_{0}";


        public IMemoryCache _memoryCache;
        public IRepository<SysUser> _sysUserRepository;
        public IRepository<SysUserToken> _sysUserTokenRepository;
        public IHttpContextAccessor _httpContextAccessor;

        public SysUserService(IMemoryCache memoryCache, 
            IRepository<SysUser> sysUserRepository, 
            IRepository<SysUserToken> sysUserTokenRepository, 
            IHttpContextAccessor httpContextAccessor)
        {
            this._memoryCache = memoryCache;
            this._sysUserRepository = sysUserRepository;
            this._sysUserTokenRepository = sysUserTokenRepository;
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 验证用户登录状态
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <param name="r">随机值</param>
        /// <returns></returns>
        public (bool Status, string Message, string Token, SysUser User) ValidateUser(string account, string password, string r)
        {
            var user = GetByAccount(account);
            if (user == null)
            {
                return (false, "用户名或密码错误", null, null);
            }
            if (!user.Enabled)
            {
                return (false, "您的账号已被冻结", null, null);
            }
            if (user.LoginLock)
            {
                if (user.AllowLoginTime > DateTime.Now)
                {
                    return (false, $"您的账号已被锁定{(int)((user.AllowLoginTime - DateTime.Now).Value.TotalMinutes) + 1}分钟", null, null);
                }
            }
            var md5Password = EncryptorHelper.GetMD5(user.Password + r);
            //匹配密码
            if (password.Equals(md5Password, StringComparison.InvariantCultureIgnoreCase))
            {
                user.LoginLock = false;
                user.LoginFailedNum = 0;
                user.AllowLoginTime = null;
                user.LastLoginTime = DateTime.Now;
                user.LastIpAddress = IPHelper.GetIPContent(this._httpContextAccessor);

                //登录日志
                user.SysUserLoginLogs.Add(new SysUserLoginLog
                {
                    Id = Guid.NewGuid(),
                    IpAddress = IPHelper.GetIPContent(this._httpContextAccessor),
                    LoginTime = DateTime.Now,
                    Message = "登录：成功"
                });

                //单点登录,移除旧的登录token
                var userToken = new SysUserToken
                {
                    Id = Guid.NewGuid(),
                    ExpireTime = DateTime.Now.AddDays(15)
                };
                user.SysUserTokens.Add(userToken);

                _sysUserRepository.DbContext.SaveChanges();
                return (true, "登录成功", userToken.Id.ToString(), user);
            }
            else//密码不匹配
            {
                //登录日志
                user.SysUserLoginLogs.Add(new SysUserLoginLog
                {
                    Id = Guid.NewGuid(),
                    IpAddress = IPHelper.GetIPContent(this._httpContextAccessor),
                    LoginTime = DateTime.Now,
                    Message = "登录：密码错误"
                });
                user.LoginFailedNum++;
                if (user.LoginFailedNum > 5)
                {
                    user.LoginLock = true;
                    user.AllowLoginTime = DateTime.Now.AddHours(5);
                }
                _sysUserRepository.DbContext.SaveChanges();
                return (false, "用户名或密码错误", null, null);
            }
        }

        /// <summary>
        /// 通过账号获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public SysUser GetByAccount(string account)
        {
            //return new SysUser { Name = "张三" };
            return _sysUserRepository.Table.FirstOrDefault(u => u.Account == account);
        }

        /// <summary>
        /// 通过token获取登录用户的信息
        /// （缓存）
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns></returns>
        public SysUser GetLogged(string token)
        {
            //return new SysUser { Name = "张三" };
            SysUserToken sysUserToken = null;
            SysUser sysUser = null;
            _memoryCache.TryGetValue(token, out sysUserToken);
            if (sysUserToken!=null)
            {
                _memoryCache.TryGetValue(string.Format(MODEL_KEY, sysUserToken.Id.ToString()), out sysUser);
            }
            if (sysUser!=null)
            {
                return sysUser;
            }
            Guid tokenId = Guid.Empty;
            if (Guid.TryParse(token,out tokenId))
            {
                var tokenItem = _sysUserTokenRepository.Table.Include(x => x.SysUser).FirstOrDefault(o => o.Id == tokenId);
                if (tokenItem!=null)
                {
                    _memoryCache.Set(token, tokenItem,DateTime.Now.AddHours(4));
                    _memoryCache.Set(string.Format(MODEL_KEY, tokenItem.Id.ToString()), tokenItem.SysUser, DateTime.Now.AddHours(4));
                    return tokenItem.SysUser;
                }
            }
            return null;
        }

        /// <summary>
        /// 搜索数据
        /// </summary>
        /// <param name="arg">搜索条件</param>
        /// <param name="page">第几页</param>
        /// <param name="size">大小</param>
        /// <returns></returns>
        public IPagedList<SysUser> SearchUser(SysUserSearchArg arg, int page, int size)
        {
            var query = _sysUserRepository.Table.Where(o => !o.IsDeleted);
            if (arg!=null)
            {
                if (!string.IsNullOrEmpty(arg.keyWord))
                {
                    query = query.Where(o => o.Account.Contains(arg.keyWord) || o.MobilePhone.Contains(arg.keyWord) || o.Email.Contains(arg.keyWord) || o.Name.Contains(arg.keyWord));
                }
                if (arg.enabled.HasValue)
                {
                    query = query.Where(o => o.Enabled == arg.enabled);
                }
                if (arg.unlock.HasValue)
                {
                    query = query.Where(o => o.LoginLock == arg.unlock);
                }
                if (arg.roleId.HasValue)
                {
                    query = query.Where(o => o.SysUserRoles.Any(r => r.RoleId == arg.roleId));
                }
            }
            query = query.OrderBy(o => o.Account).ThenBy(o => o.Name).ThenByDescending(o => o.CreationTime);
            return new PagedList<SysUser>(query, page, size);
        }

        /// <summary>
        /// 根据Id获取用户详情
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        public SysUser GetById(Guid id)
        {
            //return new SysUser { Name = "张三" };
            return _sysUserRepository.GetById(id);
        }

        /// <summary>
        /// 插入、新增
        /// </summary>
        /// <param name="model">实体</param>
        public void InsertSysUser(SysUser model)
        {
            if (ExistAccount(model.Account))
            {
                return;
            }
            _sysUserRepository.Insert(model);
        }

        /// <summary>
        /// 更新修改
        /// </summary>
        /// <param name="model">实体</param>
        public void UpdateSysUser(SysUser model)
        {
            //_sysUserRepository.DbContext.Entry(model).State = EntityState.Unchanged;
            //_sysUserRepository.DbContext.Entry(model).Property("Name").IsModified = true;
            //_sysUserRepository.DbContext.Entry(model).Property("Email").IsModified = true;
            //_sysUserRepository.DbContext.Entry(model).Property("MobilePhone").IsModified = true;
            //_sysUserRepository.DbContext.Entry(model).Property("Sex").IsModified = true;
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Name", model.Name));
            param.Add(new SqlParameter("@Email", model.Email));
            param.Add(new SqlParameter("@MobilePhone", model.MobilePhone));
            param.Add(new SqlParameter("@Sex", model.Sex));
            param.Add(new SqlParameter("@Modifier", model.Modifier));
            param.Add(new SqlParameter("@Id", model.Id));

            _sysUserRepository.DbContext.Database.ExecuteSqlCommand("UPDATE dbo.SysUser SET Name=@Name,Email=@Email,MobilePhone=@MobilePhone,Sex=@Sex,ModifiedTime=GETDATE(),Modifier=@Modifier WHERE Id=@Id", param);
        }

        /// <summary>
        /// 重置密码（默认重置成和账号一样）
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="modifer">修改人Id</param>
        public void ResetPassword(Guid id, Guid modifer)
        {
            var user = _sysUserRepository.GetById(id);
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.Salt))
                {
                    user.Salt = EncryptorHelper.CreateSaltKey();
                }
                user.Password = EncryptorHelper.GetMD5(user.Account+user.Salt);
                user.ModifiedTime = DateTime.Now;
                user.Modifier = modifer;
                _sysUserRepository.DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 验证账号是否已经存在
        /// </summary>
        /// <param name="account">用户账号</param>
        public bool ExistAccount(string account)
        {
            return _sysUserRepository.Table.Any(u => u.Account == account);
        }

        /// <summary>
        /// 启用禁用账号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="enable">启用/禁用</param>
        /// <param name="modifer">修改人Id</param>
        public void Enabled(Guid id, bool enable, Guid modifer)
        {
            var user = _sysUserRepository.GetById(id);
            if (user != null)
            {
                user.Enabled = enable;
                user.ModifiedTime = DateTime.Now;
                user.Modifier = modifer;
                _sysUserRepository.DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 登录锁与解锁
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="enable">登录锁/解锁</param>
        /// <param name="midifer">修改人Id</param>
        public void LoginLock(Guid id, bool enable, Guid modifer)
        {
            var user = _sysUserRepository.GetById(id);
            if (user != null)
            {
                user.LoginLock = enable;
                user.ModifiedTime = DateTime.Now;
                user.Modifier = modifer;
                _sysUserRepository.DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 删除用户（无法删除超级管理员用户）
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="modifer">修改人id</param>
        public void DeleteSysUser(Guid id, Guid modifer)
        {
            var user = _sysUserRepository.GetById(id);
            if (user!=null)
            {
                user.IsDeleted = true;
                user.ModifiedTime = DateTime.Now;
                user.Modifier = modifer;
                _sysUserRepository.DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 添加用户头像
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="avatar">头像字节流</param>
        public void AddAvatar(Guid id, byte[] avatar)
        { }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="password">新密码</param>
        public void ChangePassword(Guid id, string password)
        {
            var user = _sysUserRepository.GetById(id);
            if (user != null)
            {
                user.Password = EncryptorHelper.GetMD5(password + user.Salt);
                user.ModifiedTime = DateTime.Now;
                user.Modifier =user.Id;
                _sysUserRepository.DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 设置用户最后活动时间
        /// </summary>
        /// <param name="id">用户id</param>
        public void LastActivityTime(Guid id)
        {
            var user = _sysUserRepository.GetById(id);
            if (user != null)
            {
                user.LastActivityTime = DateTime.Now;
                _sysUserRepository.DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 判断是否存在用户
        /// </summary>
        /// <returns></returns>
        public bool ExistUser()
        {
            return _sysUserRepository.Entities.Any();
        }
    }
}
