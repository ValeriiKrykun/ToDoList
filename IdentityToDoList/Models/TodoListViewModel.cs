using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityToDoList.Models
{
    public class TodoListViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Datetime")]
        public DateTime Datetime { get; set; }

        public DateTime SpendTime { get; set; }

        public DateTime Start { get; set; }

        public DateTime Stop { get; set; }

        [Display(Name = "Task priority")]
        [Required(ErrorMessage = "Priority is Required")]
        [Range(1, 10, ErrorMessage = "Priority must be a positive number and not higher than 10")]
        public int Priority { get; set; }

        [Display(Name = "Lead Time")]
        [Range(1, 100, ErrorMessage = "Priority must be a positive number and not higher than 10")]
        public int LeadTime { get; set; }
        public string Message { get; set; }
        public string ApplicationUsersId { get; set; }
        public string UserName { get; set; }
    }
}
