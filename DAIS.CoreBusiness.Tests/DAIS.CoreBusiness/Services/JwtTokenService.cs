using DAIS.CoreBusiness.Dtos;
using DAIS.CoreBusiness.Helpers;
using DAIS.CoreBusiness.Interfaces;
using DAIS.DataAccess.Data;
using DAIS.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DAIS.CoreBusiness.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtTokenSettings _jwtTokenSettings;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _context;

        private readonly ILogger<JwtTokenService> _logger;
        public JwtTokenService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
             AppDbContext context,
            IOptions<JwtTokenSettings> jwtTokenSettings,
            ILogger<JwtTokenService> logger)
        {

            _jwtTokenSettings = jwtTokenSettings.Value;

            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        public async Task<string> GenarateToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
          
           var permissions = await GetUserRoleFeatures(userRoles).ConfigureAwait(false);

            try
            {
                var claims = new[]
                 {

                    new Claim(ClaimTypes.Name,user.UserName.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("ProjectId", user.ProjectId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserType",user.UserType.ToString()),
                    new Claim(ClaimTypes.Role,string.Join(",", userRoles)),
                    new Claim("Permissions",string.Join(",", permissions.Select(x=>x.RoleFeatureName)))
                 };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSettings.SecurityKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtTokenSettings.Issuer,
                    audience: _jwtTokenSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return string.Empty;
        }

        private async Task<List<UserRoleFeature>> GetUserRoleFeatures(IList<string> userRoles)
        {
            try
            {
                _logger.LogInformation("Rolename {0}", string.Join(",", userRoles));
                // Force evaluation of roles query separately to avoid CTE issues
                List<Role> roles = new List<Role>();

                for (int i = 0; i < userRoles.Count; i++)
                {
                    var role = await _roleManager.Roles
                    .FirstOrDefaultAsync(role => role.Name == userRoles[i]);

                    roles.Add(role!);
                }


                if (!roles.Any())
                {
                    return new List<UserRoleFeature>();
                }

                var roleIds = roles.Select(role => role.Id).ToList();
                _logger.LogInformation("roleIds {0}", string.Join(",", roleIds));

                // Force evaluation of roleFeatures query
                List<RoleFeature> roleFeatures = new List<RoleFeature>();

                for (int i = 0; i < roleIds.Count; i++)
                {
                    _logger.LogInformation("roleId {0}", roleIds[i]);
                    var roleFeature = await _context.RoleFeatures.FirstOrDefaultAsync(x => x.RoleId == roleIds[i]);
                    if(roleFeature!=null) 
                        roleFeatures.Add(roleFeature!);

                }

                _logger.LogInformation("roleFeatures");

                if (!roleFeatures.Any())
                {
                    return new List<UserRoleFeature>();
                }

                var featureIds = roleFeatures.Select(rf => rf.FeatureId).Distinct().ToList();
                var permissionIds = roleFeatures.Select(rf => rf.PermissionId).Distinct().ToList();

                // Split queries to avoid complex CTE generation
                Dictionary<Guid, string> features = new Dictionary<Guid, string>();
                Dictionary<Guid, string> permissions = new Dictionary<Guid, string>();

                for (int i = 0; i < featureIds.Count; i++)
                {
                    _logger.LogInformation("featureIds");
                    var feature = await _context.Features.FindAsync(featureIds[i]);
                    if (features.ContainsKey(featureIds[i]))
                    {
                        continue;
                    }
                    features.Add(feature!.Id, feature.Name);
                }

                for (int i = 0; i < permissionIds.Count; i++)
                {
                    _logger.LogInformation("permissionIds");

                    var permission = await _context.Permissions.FindAsync(permissionIds[i]);
                    if (permissions.ContainsKey(permissionIds[i]))
                    {
                        continue;
                    }
                    permissions.Add(permission!.Id, permission.Name);   
                }
                
                // Perform the join in memory
                return roleFeatures
                    .Select(rf => new UserRoleFeature
                    {
                        FeatureName = features.GetValueOrDefault(rf.FeatureId, "Unknown"),
                        PermissionName = permissions.GetValueOrDefault(rf.PermissionId, "Unknown")
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                // Add logging here
                throw new ApplicationException("Error retrieving user role features", ex);
            }
        }
    }
}
