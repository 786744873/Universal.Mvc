using System;
using System.Collections.Generic;
using System.Text;
using Universal.Core;
using Universal.Entities;
using Universal.Entities.Dto;

namespace Universal.Services
{
    public interface ISysUserService
    {
        /// <summary>
        /// 验证用户登录状态
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <param name="r">随机值</param>
        /// <returns></returns>
        (bool Status, string Message, string Token, SysUser User) ValidateUser(string account, string password, string r);

        /// <summary>
        /// 通过账号获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        SysUser GetByAccount(string account);

        /// <summary>
        /// 通过token获取登录用户的信息
        /// （缓存）
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns></returns>
        SysUser GetLogged(string token);

        /// <summary>
        /// 搜索数据
        /// </summary>
        /// <param name="arg">搜索条件</param>
        /// <param name="page">第几页</param>
        /// <param name="size">大小</param>
        /// <returns></returns>
        IPagedList<SysUser> SearchUser(SysUserSearchArg arg, int page, int size);

        /// <summary>
        /// 根据Id获取用户详情
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        SysUser GetById(Guid id);

        /// <summary>
        /// 插入、新增
        /// </summary>
        /// <param name="model">实体</param>
        void InsertSysUser(SysUser model);

        /// <summary>
        /// 更新修改
        /// </summary>
        /// <param name="model">实体</param>
        void UpdateSysUser(SysUser model);

        /// <summary>
        /// 重置密码（默认重置成和账号一样）
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="modifer">修改人Id</param>
        void ResetPassword(Guid id, Guid modifer);

        /// <summary>
        /// 验证账号是否已经存在
        /// </summary>
        /// <param name="account">用户账号</param>
        bool ExistAccount(string account);

        /// <summary>
        /// 启用禁用账号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="enable">启用/禁用</param>
        /// <param name="modifer">修改人Id</param>
        void Enabled(Guid id, bool enable, Guid modifer);

        /// <summary>
        /// 登录锁与解锁
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="enable">登录锁/解锁</param>
        /// <param name="midifer">修改人Id</param>
        void LoginLock(Guid id, bool enable, Guid modifer);

        /// <summary>
        /// 删除用户（无法删除超级管理员用户）
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="modifer">修改人id</param>
        void DeleteSysUser(Guid id, Guid modifer);

        /// <summary>
        /// 添加用户头像
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="avatar">头像字节流</param>
        void AddAvatar(Guid id, byte[] avatar);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="password">新密码</param>
        void ChangePassword(Guid id, string password);

        /// <summary>
        /// 设置用户最后活动时间
        /// </summary>
        /// <param name="id">用户id</param>
        void LastActivityTime(Guid id);

        /// <summary>
        /// 判断是否存在用户
        /// </summary>
        /// <returns></returns>
        bool ExistUser();
    }
}
