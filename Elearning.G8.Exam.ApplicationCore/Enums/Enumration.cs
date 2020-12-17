namespace Elearning.G8.Exam.ApplicationCore
{
	public class Enumration
	{
		public enum ProcdureTypeName
		{
			/// <summary>
			///  Lấy dữ liệu
			/// </summary>
			Get,

			/// <summary>
			/// Lấy dữ liệu theo khóa chính
			/// </summary>
			GetById,

			/// <summary>
			/// Thêm mới
			/// </summary>
			Insert,

			/// <summary>
			/// Sửa/ cập nhật dữ liệu
			/// </summary>
			Update,

			/// <summary>
			/// Xóa dữ liệu
			/// </summary>
			Delete,

			/// <summary>
			/// Lấy dữ liệu có phân trang
			/// </summary>
			GetPaging
		}

		/// <summary>
		/// Kiểu phương thức 
		/// </summary>
		public enum EntityState
		{
			/// <summary>
			/// Lấy dữ liệu
			/// </summary>
			GET,

			/// <summary>
			/// Thêm mới dữ liệu
			/// </summary>
			INSERT,

			/// <summary>
			/// Sửa dữ liệu
			/// </summary>
			UPDATE,

			/// <summary>
			/// Xóa dữ liệu
			/// </summary>
			DELETE
		}

		public enum Code
		{
			Success = 200,
			ValidateBussiness = 402,
			ErrorAddEntity = 405,
			Exception = 500,
			ValidateEntity=202,
			NotFound=404,
			ErrorDeleteEntity=601,

			NotTimeToDo=602,
			TimeOut=603,
			/// <summary>
			/// Đang có người thi
			/// </summary>
			IsDoing=604,
			/// <summary>
			/// Da ghi nhan submit truoc do
			/// </summary>
			SubmitDone=605,
			NotSubmit = 606
		}
	}
}
