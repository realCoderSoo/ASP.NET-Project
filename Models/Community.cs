using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Community
    {
        [Required]
        [DisplayName("Registration Number")]
        public string ID
        {
            get;
            set;
        }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title
        {
            get;
            set;
        }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget
        {
            get;
            set;
        }

        //Community.cs
        //public List<Student> Student { get; set; }
        public ICollection<CommunityMembership> CommunityMembership { get; set; }
        //public Student Student { get; set; }
    }
}
