using System;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class UserG12
	{
		public Guid? ID { get; set; }
		public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Class { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string GroupCode { get; set; }
    }
}
