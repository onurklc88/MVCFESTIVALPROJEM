using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FestivalWeb.Models
{
    public class UserComments
    {
        [Key]
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid FestID { get; set; }
        public string Comment { get; set; }
        public int LikeCount { get; set; }
        public DateTime InsertDate { get; set; }

        [ForeignKey("UserID")]
        public Users User { get; set; }
        [ForeignKey("FestID")]
        public Fests Fest { get; set; }
    }
}