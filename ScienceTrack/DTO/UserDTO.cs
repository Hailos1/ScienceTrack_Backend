using ScienceTrack.Models;

namespace ScienceTrack.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string OfficialName { get; set; }
        public string Role { get; set; }

        public UserDTO(User user) 
        {
            UserId = user.Id;
            UserName = user.UserName;
            OfficialName = user.OfficialName;
            Role = user.RoleNavigation.RoleName;
        }
    }
}
