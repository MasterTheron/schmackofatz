namespace backend.Services;

using backend.Authorization;
using backend.Entities;
using backend.Models;
using backend.Config;
using Microsoft.Extensions.Options;

public interface IUserService
{
    AuthenticateResponse? Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
    User? GetById(int id);
}

public class UserService : IUserService
{
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications
    private List<User> _users = new List<User>();

    private readonly IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;

    public UserService(IJwtUtils jwtUtils, IOptions<AppSettings> appSettings)
    {
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
        _users.Add(new User { Id = 1, FirstName = "Test", LastName = "User", Username = _appSettings.User, Password = _appSettings.Password });
    }

    public AuthenticateResponse? Authenticate(AuthenticateRequest model)
    {
        var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

        // return null if user not found
        if (user == null) return null;

        // authentication successful so generate jwt token
        var token = _jwtUtils.GenerateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public IEnumerable<User> GetAll()
    {
        return _users;
    }

    public User? GetById(int id)
    {
        return _users.FirstOrDefault(x => x.Id == id);
    }
}