using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaApp.Models
{
    public class Reading: IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Amount of consumed water")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = false)]
        public double UsageAmount { get; set; }

        [Display(Name = "Reading time")]
        public DateTime ReadingTime { get; set; }

        public WaterMeter WaterMeter { get; set; }
    }
}
