using System;

namespace Elearning.G8.Exam.ApplicationCore
{
	public partial class User : BaseEntity
	{
		public Guid UserID { get; set; }
		public Guid RoleId { get; set; }
		public string UserName { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Class { get; set; }
		public string PhoneNumber { get; set; }
		/// <summary>
		/// UserID cua các nhóm tích hợp
		/// </summary>
		public string IntegrationID { get; set; }
	}
}
