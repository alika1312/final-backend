using api.Dtos;
using api.Models;

public static class UserMappers
{
    public static UserDto ToUserDto(this ApplicationUser userModel)
    {
        return new UserDto
        {
            username = userModel.UserName ?? string.Empty,
            isCEO = userModel.isCEO,
            companyID = userModel.companyID
        };
    }

    public static ApplicationUser ToUserFromUserDto(this UserRequestDto userDto)
    {
        return new ApplicationUser
        {
            UserName = userDto.username,

            isCEO = userDto.isCEO,
            companyID = userDto.companyID
        };
    }
}
