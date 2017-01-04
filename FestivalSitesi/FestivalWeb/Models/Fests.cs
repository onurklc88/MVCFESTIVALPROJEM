using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FestivalWeb.Models
{
    public class Fests
    {
        [Key]
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string ShortDesc { get; set; }
        public string Descrition { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime InsertDate { get; set; }

        public bool IsFree { get; set; }
        public decimal? Price { get; set; }

        public string FestPicture { get; set; }
    }
}