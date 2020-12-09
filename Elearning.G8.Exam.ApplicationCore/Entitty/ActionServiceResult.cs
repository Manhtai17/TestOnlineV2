using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class ActionServiceResult
	{
		public bool Success { get; set; }
		public string Message { get; set; }
		public Code Code { get; set; }
		public object Data { get; set; }
		public int TotalRecords { get; set; }

		/// <summary>
		/// Hàm khởi tạo mặc định
		/// </summary>
		public ActionServiceResult()
		{
			Success = true;
			Message = "Success";
			Code = Code.Success;
			Data = null;
			TotalRecords = 0;
		}

		public ActionServiceResult(bool success, string message, Code code, object data, int totalRecords)
		{
			Success = success;
			Message = message;
			Code = code;
			Data = data;
			TotalRecords = totalRecords;
		}
	}
}
