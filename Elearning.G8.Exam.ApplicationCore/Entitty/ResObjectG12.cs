using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class ResObjectG12<T>
	{
		public int  Code { get; set; }
		public T Data { get; set; }

		public string Message { get; set; }
	}

	public class UserInfo
	{
		public Guid? Id { get; set; }
		public string UserName { get; set; }
		public string Token { get; set; }
		public string Email { get; set; }
	}
}
