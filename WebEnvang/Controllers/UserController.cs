using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebEnvang.Models;
using WebEnvang.Models.Roles;
using WebEnvang.Models.Users;

namespace WebEnvang.Controllers
{
    public class UserController : ApiController
    {
        private UserService UserService = null;
        private RoleService RoleService = null;
        public UserController()
        {
            this.UserService = new UserService();
            this.RoleService = new RoleService();
        }

        [Authorize(Roles = "System")]
        [HttpPost]
        public async Task<dynamic> GetList([FromBody]UserDTO dto)
        {
            return await UserService.GetList(dto);
        }

        [Authorize(Roles = "System")]
        public async Task<dynamic> Save([FromBody]UserDTO dto)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (dto.SaveType == 1)
            {
                var user = new ApplicationUser() { UserName = dto.UserName, PhoneNumber = dto.PhoneNumber, Email = dto.Email, FullName = dto.FullName, Address = dto.Address };
                var result = await userManager.CreateAsync(user, "123456");
                if (result.Succeeded)
                {
                    user = await userManager.FindByNameAsync(dto.UserName);
                    var addRoleResult = await userManager.AddToRoleAsync(user.Id, dto.RoleName);
                    if (addRoleResult.Succeeded)
                    {
                        return new
                        {
                            success = true,
                            status = 0,
                            message = "Tạo user thành công"
                        };
                    }
                    else
                    {
                        await userManager.DeleteAsync(user);
                        return new
                        {
                            success = false,
                            status = 1,
                            message = "Tạo user thất bại. Xin vui lòng thực hiện lại sau"
                        };
                    }
                }
                else
                {
                    return new
                    {
                        success = false,
                        status = 2,
                        message = string.Join(",", result.Errors)
                    };
                }
            }
            else
            {
                var user = await userManager.FindByNameAsync(dto.UserName);
                user.UserName = dto.UserName;
                user.PhoneNumber = dto.PhoneNumber;
                user.FullName = dto.FullName;
                user.Address = dto.Address;
                user.Email = dto.Email;
                var result = await userManager.UpdateAsync(user);
                var roles = await userManager.GetRolesAsync(user.Id);
                foreach (string role in roles)
                {
                    await userManager.RemoveFromRoleAsync(user.Id, role);
                }
                await userManager.AddToRoleAsync(user.Id, dto.RoleName);
                if (result.Succeeded)
                {
                    return new
                    {
                        success = true,
                        status = 0,
                        message = "Lưu user thành công"
                    };
                }
                else
                {
                    return new
                    {
                        success = false,
                        status = 2,
                        message = string.Join(",", result.Errors)
                    };
                }
            }
        }

        [Authorize(Roles = "System")]
        public async Task<dynamic> Remove([FromBody]UserDTO dto)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByNameAsync(dto.UserName);
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new
                {
                    success = true,
                    status = 0,
                    message = "Xóa user thành công"
                };
            }
            else
            {
                return new
                {
                    success = false,
                    status = 1,
                    message = string.Join(",", result.Errors)
                };
            }
        }

        [Authorize]
        public async Task<dynamic> ResetPassword([FromBody]UserDTO dto)
        {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByNameAsync(dto.UserName);
            var token = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await userManager.ResetPasswordAsync(user.Id, token, dto.Password);
            if (result.Succeeded)
            {
                return new
                {
                    success = true,
                    status = 0,
                    message = "Đổi mật khẩu thành công"
                };
            }
            else
            {
                return new
                {
                    success = false,
                    status = 1,
                    message = string.Join(",", result.Errors)
                };
            }
        }

        [Authorize]
        public async Task<dynamic> Info()
        {
            string userId = User.Identity.GetUserId();
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(userId);

            var pages = await this.RoleService.GetPagesByUser(userId);

            return new
            {
               FullName = user.FullName,
               Email = user.Email,
               Address = user.Address,
               PhoneNumber = user.PhoneNumber,
               Pages = pages
            };
        }

        [Authorize]
        public async Task<dynamic> Update([FromBody]UserDTO dto)
        {
            try
            {
                var username = User.Identity.GetUserName();
                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await userManager.FindByNameAsync(username);
                user.PhoneNumber = dto.PhoneNumber;
                user.FullName = dto.FullName;
                user.Address = dto.Address;
                user.Email = dto.Email;
                var result = await userManager.UpdateAsync(user);
                return new { success = result.Succeeded };
            }
            catch
            {
                return new { success = false };
            }
        }

        [AllowAnonymous]
        public async Task<dynamic> Register([FromBody]UserDTO dto)
        {
            try
            {
                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await userManager.FindByNameAsync(dto.UserName);
                if (user != null)
                {
                    return new { success = false, message = "Đã tồn tại tài khoản" };
                }
                else
                {
                    user = new ApplicationUser() { UserName = dto.UserName, PhoneNumber = dto.PhoneNumber, Email = dto.Email, FullName = dto.FullName, Address = dto.Address };
                    var result = await userManager.CreateAsync(user, dto.Password);
                    if (result.Succeeded)
                    {
                        user = await userManager.FindByNameAsync(dto.UserName);
                        var addRoleResult = await userManager.AddToRoleAsync(user.Id, "Customer");
                        if (addRoleResult.Succeeded)
                        {
                            return new
                            {
                                success = true,
                                status = 0,
                                message = "Tạo user thành công"
                            };
                        }
                        else
                        {
                            await userManager.DeleteAsync(user);
                            return new
                            {
                                success = false,
                                status = 1,
                                message = "Tạo user thất bại. Xin vui lòng thực hiện lại sau"
                            };
                        }
                    }
                    else
                    {
                        return new
                        {
                            success = false,
                            status = 2,
                            message = string.Join(",", result.Errors)
                        };
                    }
                }
            }
            catch
            {
                return new { success = false, message = "Lỗi hệ thống" };
            }
        }
    }
}
