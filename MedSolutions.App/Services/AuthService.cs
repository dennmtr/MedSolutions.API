using AutoMapper;
using MedSolutions.App.DTOs;
using MedSolutions.App.Interfaces;
using MedSolutions.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace MedSolutions.App.Services;

public class AuthService(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IMapper mapper) : IAuthService
{
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IMapper _mapper = mapper;

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest) => throw new NotImplementedException();
    public async Task LogoutAsync() => throw new NotImplementedException();
}
