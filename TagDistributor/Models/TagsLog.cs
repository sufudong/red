using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TagDistributor.Models
{
    public class TagsLog
    {
        public int ID { get; set; }
        public string Info { get; set; }
        public string Username { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DistributDate { get; set; }
    }
}