namespace TaskList.BLL.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {
            IsActive = true;
        }

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string TelegramContact { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        public UserDTO Copy()
        {
            return new UserDTO
            {
                UserId = UserId,
                FullName = FullName,
                TelegramContact = TelegramContact,
                Role = Role,
                IsActive = IsActive
            };
        }
    }
}
