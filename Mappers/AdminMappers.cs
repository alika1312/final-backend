using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class AdminMappers
    {
        public static AdminDto ToAdminDto(this Admins admin)
        {
            return new AdminDto
            {

                userID = admin.userID,
                username = admin.User?.UserName ?? string.Empty
            };
        }

        public static Admins ToAdminsFromDto(this CreateAdminRequestDto dto)
        {
            return new Admins
            {
                userID = dto.userID
            };
        }
    }
}
