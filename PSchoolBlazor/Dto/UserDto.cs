using PSchoolBlazor.Enum;

namespace PSchoolBlazor.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public int? ParentId { get; set; }

    }
}
