using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.ApplicationCore.Entitty.Base;
using Elearning.G8.Exam.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elearning.G8.Exam.Infrastructure.Repository.Interfaces
{
	public interface IBaseRepository<T> where T : BaseEntity, IAggregateRoot
	{
		/// <summary>
		/// Lấy List toàn bộ dữ liệu trong bảng:
		/// </summary>
		/// <returns></returns>
		Task<IReadOnlyList<T>> GetListAsync();

		/// <summary>
		/// Lấy thông tin đối tượng theo khóa chính
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		Task<T> GetEntityByIdAsync(object Id);

		/// <summary>
		/// Lấy toàn bộ dữ liệu theo điều kiện truyền vào của đối tượng
		/// </summary>
		/// <param name="spec"></param>
		/// <returns></returns>
		Task<IReadOnlyList<T>> GetListAsyncBySpecification(ISpecification<T> spec);

		/// <summary>
		/// Lấy dữ liệu theo  tham số truyền vào (Theo thứ tự trong store)
		/// </summary>
		/// <param name="parameters">Mảng chứa các tham số truyền vào cho store theo đúng thứ tự</param>
		Task<IReadOnlyList<T>> GetListAsync(object[] parameters);

		/// <summary>
		/// Lấy dữ liệu có phần trang
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<T>> GetListPagedAsync();

		/// <summary>
		/// Thêm mới đối tượng vào bảng
		/// </summary>
		/// <param name="entity">Đối tượng</param>
		/// <param name="returnValue">Có trả về giá trị đơn hay không [VD: ID của bản ghi vừa tạo] - true: có</param>
		/// <returns>Kết quả trả về là số bản ghi ảnh hưởng hoặc 1 giá trị đơn lẻ được select khi set returnValue = true</returns>
		Task<object> AddAsync(T entity, bool returnValue = false);

		/// <summary>
		/// Thêm mới đối tượng và trả về đối tượng được thêm mới
		/// </summary>
		/// <param name="entity">Đối tượng muốn thêm mới</param>
		/// <returns>Đối tượng vừa thực hiện thêm mới</returns>
		Task<T> AddAsync(T entity);

		/// <summary>
		/// Chỉ thực hiện update dữ liệu - không trả về gì
		/// </summary>
		/// <param name="entity"></param>
		Task UpdateAsync(T entity);

		/// <summary>
		/// Chỉ xóa đối tượng, không trả về bất cứ đối tượng hay giá trị nào.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		Task DeleteAsync(T entity);

		/// <summary>
		/// Đếm số bản ghi có trong dữ liệu theo điều kiện cụ thể
		/// </summary>
		/// <param name="spec"></param>
		/// <returns></returns>
		Task<int> CountAsync(ISpecification<T> spec);



		/// <summary>
		/// PUT dữ liệu vào bảng trả về số lượng bản ghi được update
		/// </summary>
		Task<int> Update(T entity);

		/// <summary>
		/// Delete dữ liệu theo các tham số truyền vào
		/// Các tham số truyền vào theo thứ tự tương ứng với thứ tự được viết trong store
		/// </summary>
		Task<int> Delete(object[] param);

		Task<object> AddAsync(T entity, bool returnValue, DatabaseConnector databaseConnector);


		Task<IEnumerable<T>> GetEntitites(string procedureName, object[] param);

		Task<T> GetEntity(string procedureName, object[] param);
		Task<object> Get(string procedureName, object[] param);

		/// <summary>
		/// Thêm mới sử dụng procedure param
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="procedureName"></param>
		/// <param name="returnValue"></param>
		/// <returns></returns>
		Task<object> AddAsync(T entity, string procedureName, DatabaseConnector databaseConnector);

		/// <summary>
		/// Hàm update với connector truyền vào
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="databaseConnector"></param>
		/// <returns></returns>
		Task<int> Update(T entity, DatabaseConnector databaseConnector);

		/// <summary>
		/// Hàm xóa với connector truyền vào
		/// </summary>
		/// <param name="param"></param>
		/// <param name="databaseConnector"></param>
		/// <returns></returns>
		Task<int> Delete(object[] param, DatabaseConnector databaseConnector);

		/// <summary>
		/// Lấy tổng số bản ghi
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		int GetTotalRecords(string procedureName, object[] parameters);

	}
}
