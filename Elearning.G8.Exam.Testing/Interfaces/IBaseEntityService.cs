using Elearning.G8.Exam.ApplicationCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elearning.G8.Exam.Testing.Interfaces
{
	public interface IBaseEntityService<T>
	{
		/// <summary>
		/// Lấy toàn bộ dữ liệu
		/// </summary>
		/// <returns></returns>
		Task<IReadOnlyList<T>> GetEntities();

		/// <summary>
		/// Lấy dữ liệu phân trang
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<T>> GetEntitiesPaging();

		/// <summary>
		/// Lấy theo khóa chính
		/// </summary>
		/// <param name="id">PK bản ghi trong CSDL</param>
		/// <returns></returns>
		Task<T> GetEntityById(object id);

		/// <summary>
		/// Thêm mới dữ liệu
		/// </summary>
		/// <param name="entity">đối tượng thêm mới</param>
		/// <param name="returnSingleValue">lấy giá trị trả về từ store hay không (true: có)</param>
		/// <returns>Số bản ghi được thêm mới</returns>
		Task<ActionServiceResult> Insert(T entity, bool returnSingleValue = false);

		/// <summary>
		/// Sửa dữ liệu
		/// </summary>
		/// <param name="entity"></param>
		/// <returns>Số bản ghi sửa thành công</returns>
		Task<ActionServiceResult> Update(T entity);

		/// <summary>
		/// Xóa dữ liệu theo khóa chính
		/// </summary>
		/// <param name="id">PK bản ghi trong CSDL</param>
		/// <returns>Số bản ghi được xóa trong Database</returns>
		Task<int> Delete(object id);


	}
}
