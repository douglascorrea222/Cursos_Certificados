using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace certificado.UI.Web.Data
{
    public class SeedData
    {
        private readonly ILogger<SeedData> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(ILogger<SeedData> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            _logger.LogInformation("Initializing database with seed data...");

            string[] roles = { "Admin", "User" };

            // Verificar se as roles já existem_Teste
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                    _logger.LogInformation($"Role '{role}' created.");
                }
                else
                {
                    _logger.LogInformation($"Role '{role}' already exists.");
                }
            }

            var users = new[]
            {
                new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com" },
                new IdentityUser { UserName = "user1@example.com", Email = "user1@example.com" },
                new IdentityUser { UserName = "user2@example.com", Email = "user2@example.com" },
                new IdentityUser { UserName = "douglas.correa", Email = "douglascorrea10.ti@gmail.com" }
            };

            foreach (var user in users)
            {
                if (_userManager.Users.All(u => u.UserName != user.UserName))
                {
                    var result = await _userManager.CreateAsync(user, "Password@123");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        _logger.LogInformation($"User '{user.UserName}' created and added to role 'User'.");
                    }
                    else
                    {
                        _logger.LogError($"Error creating user '{user.UserName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    _logger.LogInformation($"User '{user.UserName}' already exists.");
                }
            }
        }
    }
}
