using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models
{
    public class ProductCreateViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Name { get; set; }

        public string Price { get; set; }
    }
}
