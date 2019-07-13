using System;
using System.Collections.Generic;
using System.Text;

namespace Back.DAL.ViewModel
{
    public class UserVM
    {
        public string Name { get; set; }
        
        public string FamilyName { get; set; }
        
        public string UserName { get; set; }
        
        public string Password { get; set; }

        //[Required]
        //public int AccessLevel { get; set; }
        
        //public string CheckSum { get; set; }
        
        //public string PhoneNumber { get; set; }
        
        public string Email { get; set; }
        
        public DateTime? BirthDate { get; set; }
    }
}
