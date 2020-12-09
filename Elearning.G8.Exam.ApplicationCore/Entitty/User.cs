using System;

namespace Elearning.G8.Exam.ApplicationCore
{
	public partial class User : BaseEntity
	{
		public Guid Id { get; set; }
		public Guid RoleId { get; set; }
		public string UserName { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Class { get; set; }
		public string PhoneNumber { get; set; }
	}
}
