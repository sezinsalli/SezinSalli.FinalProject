
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Service.Exceptions;
using System.Data.Entity;
using System.Security.Claims;

namespace Simpra.Service.Service;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    public UserService(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<List<AppUser>> GetAllAsycn()
    {
        try
        {
            var userList = await _userManager.Users.ToListAsync();
            return userList;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetAllAsycn Exception");
            throw new Exception($"Something went wrong. Error message:{ex.Message}");
        }
    }

    public async Task<AppUser> GetByIdAsync(string id)
    {
        try
        {
            var user = await _userManager.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null)
                throw new NotFoundException($"User ({id}) not found!");

            return user;
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException)
            {
                Log.Warning(ex, "GetByIdAsync Exception - Not Found Error");
                throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
            }
            Log.Error(ex, "GetByIdAsync Exception");
            throw new Exception($"Something went wrong. Error message:{ex.Message}");
        }
    }

    public async Task<AppUser> InsertAsync(AppUser user, string password)
    {
        try
        {
            user.EmailConfirmed = true;
            user.TwoFactorEnabled = false;
            user.CreatedAt = DateTime.Now;

            var response = await _userManager.CreateAsync(user, password);
            if (!response.Succeeded)
            {
                var errorMessages = response.Errors.Select(error => error.Description);
                var errorMessage = string.Join(", ", errorMessages);
                throw new Exception(errorMessage);
            }

            return user;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "InsertAsync Exception");
            throw new Exception($"User cannot create. Error message:{ex.Message}");
        }
    }

    // Direkt olarak model "userManager.UpdateAsync" metotuna verdiğimizde; Error message:The instance of entity type 'AppUser' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked.
    public async Task UpdateAsync(AppUser user, string id)
    {
        try
        {
            var userExist = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            if (userExist == null)
                throw new NotFoundException($"User ({id}) not found!");
            userExist.UserName = user.UserName;
            userExist.FirstName = user.FirstName;
            userExist.LastName = user.LastName;
            userExist.Email = user.Email;
            userExist.PhoneNumber = user.PhoneNumber;
            userExist.DigitalWalletBalance = user.DigitalWalletBalance;
            userExist.DigitalWalletInformation = user.DigitalWalletInformation;

            var response = await _userManager.UpdateAsync(userExist);

            if (!response.Succeeded)
            {
                var errorMessages = response.Errors.Select(error => error.Description);
                var errorMessage = string.Join(", ", errorMessages);
                throw new Exception(errorMessage);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "UpdateAsync Exception");
            throw new Exception($"User cannot update. Error message:{ex.Message}");
        }
    }


    public async Task Delete(string id)
    {
        //if (string.IsNullOrWhiteSpace(id))
        //{
        //    throw new Exception("Hatalı");
        //}

        var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
        await _userManager.DeleteAsync(user);

    }

    public async Task<AppUser> GetUserAsync(ClaimsPrincipal User)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            return user;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetUserAsync Exception");
            throw new Exception($"Something went wrong! Error message:{ex.Message}");
        }
    }

    public string GetUserId(ClaimsPrincipal User)
    {
        var id = _userManager.GetUserId(User);
        return id;
    }
}
