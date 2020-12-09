using System;

namespace Elearning.G8.Exam.ApplicationCore
{
	public partial class Role : BaseEntity
	{
		public Guid RoleId { get; set; }
		public string RoleName { get; set; }
	}
}
