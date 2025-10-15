namespace BlogV3._1.Models
{
    public class UserEditYouViewModel
    {
      
        public int Id { get; set; }


        public string FullName { get; set; } 
        public string Email { get; set; }

   
        public string OldPassword { get; set; }  
        public string NewPassword { get; set; }      
        public string ConfirmPassword { get; set; }  

        public bool IsTwoFactorEnabled { get; set; }  

    }
}
