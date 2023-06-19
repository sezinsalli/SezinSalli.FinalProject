
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Service.Exceptions;
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

    public List<AppUser> GetAll()
    {
        try
        {
            var userList = _userManager.Users.ToList();
            return userList;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetAllAsycn Exception");
            throw new Exception($"Something went wrong. Error message:{ex.Message}");
        }
    }

    public AppUser GetById(string id)
    {
        try
        {
            var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
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
    public async Task<AppUser> UpdateAsync(AppUser user, string id)
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
            userExist.UpdatedAt = DateTime.Now;

            var response = await _userManager.UpdateAsync(userExist);
            if (!response.Succeeded)
            {
                var errorMessages = response.Errors.Select(error => error.Description);
                var errorMessage = string.Join(", ", errorMessages);
                throw new Exception(errorMessage);
            }
            return userExist;
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException)
            {
                Log.Warning(ex, "UpdateAsync Exception - Not Found Error");
                throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
            }
            Log.Error(ex, "UpdateAsync Exception");
            throw new Exception($"User cannot update. Error message:{ex.Message}");
        }
    }


    public async Task<bool> UpdateWalletBalanceAsync(decimal balance, string id)
    {
        try
        {
            var userExist = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            if (userExist == null)
                throw new NotFoundException($"User ({id}) not found!");

            userExist.DigitalWalletBalance = balance;
            userExist.UpdatedAt = DateTime.Now;

            var response = await _userManager.UpdateAsync(userExist);
            if (!response.Succeeded)
            {
                var errorMessages = response.Errors.Select(error => error.Description);
                var errorMessage = string.Join(", ", errorMessages);
                throw new Exception(errorMessage);
            }
            return true;
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException)
            {
                Log.Warning(ex, "UpdateWalletBalanceAsync Exception - Not Found Error");
                throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
            }
            Log.Error(ex, "UpdateWalletBalanceAsync Exception");
            throw new Exception($"User cannot update. Error message:{ex.Message}");
        }
    }


    public async Task DeleteAsync(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ClientSideException("Please put an valid id");

            var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
                throw new NotFoundException($"User ({id}) not found!");

            var response = await _userManager.DeleteAsync(user);
            if (!response.Succeeded)
            {
                var errorMessages = response.Errors.Select(error => error.Description);
                var errorMessage = string.Join(", ", errorMessages);
                throw new Exception(errorMessage);
            }
        }
        catch (Exception ex)
        {
            if (ex is ClientSideException)
            {
                Log.Warning(ex, "DeleteAsync Exception - Client Side Error");
                throw new NotFoundException($"Client Side Error. Error message:{ex.Message}");
            }
            if (ex is NotFoundException)
            {
                Log.Warning(ex, "DeleteAsync Exception - Not Found Error");
                throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
            }
            Log.Error(ex, "DeleteAsync Exception");
            throw new Exception($"User cannot delete. Error message:{ex.Message}");
        }
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
        try
        {
            var id = _userManager.GetUserId(User);
            return id;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetUserId Exception");
            throw new Exception($"Something went wrong! Error message:{ex.Message}");
        }

    }
}
