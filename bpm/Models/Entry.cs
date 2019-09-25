using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bpm.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Date / Time")]
        public DateTime DateEntered { get; set; }

        [Required]
        [Range(1, 300, ErrorMessage = "Please check your Systolic entry number")]
        public int Systolic { get; set; }

        [Required]
        [Range(1, 300, ErrorMessage = "Please check your Diastolic entry number")]
        public int Diastolic { get; set; }

        [Required]
        [Range(1, 300, ErrorMessage = "Number should not exceed 200")]
        public int Pulse { get; set; }

        [Required]
        public int Weight { get; set; }

        [StringLength(280, ErrorMessage = "Please limit notes to 280 characters")]
        public string Notes { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
