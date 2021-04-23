using System;
using System.ComponentModel.DataAnnotations;

namespace WebTodoList.Models
{
    public class TodoItem
    {
		[Key]
		public int ItemId { get; set; }

		[DataType(DataType.EmailAddress)]
		public String Email { get; set; }

		[MaxLength(100)]
        public String Title { get; set; }

		[MaxLength(300)]
        public String Description { get; set; }

		[Display(Name = "Date Added")]
		public DateTime AddedDate { get; set; } = DateTime.Now;

		[Display(Name = "Due Date")]
		public DateTime DueDate { get; set; } = DateTime.Now;

		[Display(Name = "Date Completed")]
		public DateTime DoneDate { get; set; } = DateTime.Now;

		public Boolean Done { get; set; } = false;
	}
}
