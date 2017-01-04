using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FestivalWeb.Models
{
    public class Users
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime InsertDate { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Surname { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        [StringLength(150)]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime BirthDay { get; set; }
        public string Avatar { get; set; }
    }
}