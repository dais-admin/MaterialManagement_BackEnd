﻿using DAIS.API.Helpers;
using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Interfaces;
using DAIS.CoreBusiness.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DAIS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly MaterialConfigSettings _materialConfig;
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("GetAllUsersByRole")]
        public async Task<ActionResult> GetAllUsersByRole(string userRole)
        {
            var userDto = await _userService.GetUsersByRole(userRole);
            if (userDto == null || !userDto.Any())
                return NotFound(new { message = $"No users found with role '{userRole}'" });
            return Ok(userDto);
        }

        [HttpPost("AssignUserRole")]
        public async Task<ActionResult<UserRoleDto>> AssignUserRole(UserRoleDto userRoleDto)
        {
            if (userRoleDto == null)
                return BadRequest(new { message = "User role data is required" });

            var userRoleDtoResult = await _userService.AssignUserRoleAsync(userRoleDto);
            if (userRoleDtoResult == null)
                return NotFound(new { message = "User or role not found" });
            return Ok(userRoleDtoResult);
        }

        [HttpGet("GetAllUserList")]
        public async Task<ActionResult> GetAllUserList()
        {
            var userListDto = await _userService.GetAllUsers();
            if (userListDto == null || !userListDto.Any())
                return NotFound(new { message = "No users found" });
            return Ok(userListDto);
        }

        [HttpGet("GetUserByEmail")]
        public async Task<ActionResult> GetUserByEmail(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                return BadRequest(new { message = "Email is required" });

            var userDto = await _userService.GetUserByEmailAsync(userEmail);
            if (userDto == null)
                return NotFound(new { message = $"User with email '{userEmail}' not found" });
            return Ok(userDto);
        }

        [HttpGet("GetAllUserRoleList")]
        public async Task<ActionResult> GetAllUserRoleList()
        {
            var userRoleListDto = await _userService.GetAllUsersRole();
            if (userRoleListDto == null || !userRoleListDto.Any())
                return NotFound(new { message = "No user roles found" });
            return Ok(userRoleListDto);
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<ActionResult> UpdateUserProfile([FromForm] IFormFile? userImage, [FromForm] string userData)
        {
            if (string.IsNullOrEmpty(userData))
                return BadRequest(new { message = "User data is required" });

            var registrationDto = JsonConvert.DeserializeObject<RegistrationDto>(userData);
            if (registrationDto == null)
                return BadRequest(new { message = "Invalid user data format" });

            if (userImage != null)
            {
                try
                {
                    registrationDto.UserPhoto = await SaveUserImage(userImage);
                }
                catch (Exception)
                {
                    return BadRequest(new { message = "Failed to save user image" });
                }
            }

            var userDto = await _userService.UpdateUser(registrationDto);
            if (userDto == null)
                return NotFound(new { message = "User not found" });
            return Ok(userDto);
        }

        [HttpDelete("DeleteUserById")]
        public async Task<ActionResult> DeleteUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { message = "User ID is required" });

            try
            {
                await _userService.DeleteUserById(userId);
                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception)
            {
                return NotFound(new { message = $"User with ID '{userId}' not found" });
            }
        }

        [HttpDelete("DeleteRoleByName")]
        public async Task<ActionResult> DeleteRoleByName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return BadRequest(new { message = "Role name is required" });

            try
            {
                await _userService.DeleteUserRoleByName(roleName);
                return Ok(new { message = "Role deleted successfully" });
            }
            catch (Exception)
            {
                return NotFound(new { message = $"Role '{roleName}' not found" });
            }
        }
        private async Task<string> SaveUserImage([FromForm] IFormFile? userImage)
        {
            var folderName = "MaterialDocument" + "\\" + "UserPhotos" + "\\";
            var basePath = _materialConfig.DocumentBasePath;
            string fileName = string.Empty;
            string filePath = string.Empty;
            fileName = userImage.FileName;
            var dir = Path.Combine(basePath, folderName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            filePath = Path.Combine(folderName, fileName);
            var fullFilePath = Path.Combine(dir, fileName);
            await userImage.CopyToAsync(new FileStream(fullFilePath, FileMode.Create));

            return filePath;
        }
    }
}
