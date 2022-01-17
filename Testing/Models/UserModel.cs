using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Testing.Models
{
    public class UserModel
    {
        public int? ID { get; set; }
        [Required, MinLength(3), MaxLength(30), RegularExpression("^(?=[a-zA-Z0-9._]{8,20}$)(?!.*[_.]{2})[^_.].*[^_.]$", ErrorMessage = "Invalid username, please check and try again.")]

        public string Username
        {
            get
            {
                return username;
            }

            set { username = value?.ToLower(); }
        }
        private string username { get; set; }

        [Required, MinLength(8), MaxLength(30), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool? OnlineStatus { get; set; }
        public DateTime? LastLogin { get; set; }


        private DateTime? createdAt = null;

        public DateTime? CreatedAt
        {
            get
            {
                //return createdAt.HasValue
                //   ? createdAt.Value
                //   : DateTime.UtcNow;
                return createdAt;
            }

            set { createdAt = value; }
        }

        [DefaultValue(false)]
        public bool IsDelete { get; set; }

    }
}
