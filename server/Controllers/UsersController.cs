using Microsoft.AspNetCore.Mvc;
using TypeRunnerBE.Models;
using TypeRunnerBE.Responses;
using TypeRunnerBE.Services.Data;

namespace TypeRunnerBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public ActionResult<IList<UserInfoResponse>> GetAll()
        {
            return _usersService.GetAll().Select(u => new UserInfoResponse(u)).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<UserInfoResponse> GetById(int id)
        {
            var user = _usersService.GetById(id);
            return Ok(user != null ? new UserInfoResponse(user) : null);
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            var createResult = _usersService.Create(user);
            if (!createResult.Success)
                return Conflict(ErrorObject("Username has already been taken"));
            return CreatedAtAction(nameof(GetById), new { id = createResult.Result!.Username }, new UserInfoResponse(createResult.Result));
        }

        [HttpPatch("username/{id}")]
        public IActionResult ChangeUsername(int id, [FromBody] string username)
        {
            var result = _usersService.UpdateUsername(id, username);
            if (!result.Success)
            {
                switch (result.Error)
                {
                    case DataOpError.InfoEmpty:
                        return _usernameEmptyErr;
                    case DataOpError.UsernameTaken:
                        return _usernameTakenErr;
                    case DataOpError.IdNotExist:
                        return _idNotExistErr;
                }
            }
            return Ok(new UserInfoResponse(result.Result!));
        }

        [HttpPatch("pwd/{id}")]
        public IActionResult ChangePassword(int id, [FromBody] string password)
        {
            var result = _usersService.UpdatePassword(id, password);
            if (!result.Success)
            {
                switch (result.Error)
                {
                    case DataOpError.InfoEmpty:
                        return _usernameEmptyErr;
                    case DataOpError.IdNotExist:
                        return _idNotExistErr;
                }
            }
            return NoContent();
        }

        [HttpGet("friends/{id}")]
        public ActionResult<IList<UserInfoResponse>> GetFriendsById(int id)
        {
            var result = _usersService.GetFriendsById(id);
            if (!result.Success)
                return NotFound(ErrorObject("User Id does not exist"));
            return result.Result!.Select(u => new UserInfoResponse(u)).ToList();
        }

        [HttpPost("friends/{id}")]
        public IActionResult AddFriend(int id, [FromBody] int friendId)
        {
            var result = _usersService.AddFriendByIdAndFriendId(id, friendId);
            if (!result.Success)
            {
                switch (result.Error)
                {
                    case DataOpError.IdNotExist:
                        return _idNotExistErr;
                    case DataOpError.FriendIsSelf:
                        return _friendIsSelfErr;
                    case DataOpError.FriendAlreadyAdded:
                        return _friendAddedErr;
                    case DataOpError.FriendIdNotExist:
                        return _friendIdNotExistErr;
                }
            }
            return CreatedAtAction(nameof(GetFriendsById), new { id }, new { FromUserId = id, ToUserId = friendId });
        }

        [HttpDelete("friends/{id}")]
        public IActionResult Unfriend(int id, [FromBody] int friendId)
        {
            var result = _usersService.RemoveFriendByIdAndFriendId(id, friendId);
            if (!result.Success)
            {
                switch (result.Error)
                {
                    case DataOpError.IdNotExist:
                        return _idNotExistErr;
                    case DataOpError.FriendNotAdded:
                        return _friendNotAddedErr;
                }
            }
            return NoContent();
        }

        private static object ErrorObject(string msg)
        {
            return new { Error = msg };
        }

        private static readonly ActionResult _idNotExistErr = new NotFoundObjectResult(ErrorObject("User Id does not exist"));
        private static readonly ActionResult _usernameEmptyErr = new BadRequestObjectResult(ErrorObject("Username cannot be empty"));
        private static readonly ActionResult _usernameTakenErr = new ConflictObjectResult(ErrorObject("Username has already been taken"));
        private static readonly ActionResult _friendAddedErr = new ConflictObjectResult(ErrorObject("Friend already added"));
        private static readonly ActionResult _friendNotAddedErr = new NotFoundObjectResult(ErrorObject("User does not exist in friend list"));
        private static readonly ActionResult _friendIdNotExistErr = new NotFoundObjectResult(ErrorObject("Friend Id does not exist"));
        private static readonly ActionResult _friendIsSelfErr = new NotFoundObjectResult(ErrorObject("Friend Id cannot be the same as this user's Id"));
    }
}
