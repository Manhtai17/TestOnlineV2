using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.ApplicationCore.Entitty.Base;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Elearning.G8.Exam.Testing.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Testing.Services
{
	public class BaseService<T> : IBaseEntityService<T> where T : BaseEntity, IAggregateRoot
	{

		/// <summary>
		/// base Repository
		/// </summary>
		protected readonly IBaseRepository<T> _baseRepository;
		/// <summary>
		/// Khởi tạo service
		/// </summary>
		/// <param name="baseRepository"></param>
		public BaseService(IBaseRepository<T> baseRepository)
		{
			_baseRepository = baseRepository;
		}

		/// <summary>
		/// Lấy toàn bộ dữ liệu
		/// </summary>
		/// <returns></returns>
		/// CreateBy: NVMANH (20/04/2020)
		public async virtual Task<IReadOnlyList<T>> GetEntities()
		{
			var entities = await _baseRepository.GetListAsync();
			return entities;
		}

		/// <summary>
		/// Lấy dữ liệu có phân trang
		/// </summary>
		/// <returns></returns>
		/// CreateBy: NVMANH (20/04/2020)
		public async Task<IEnumerable<T>> GetEntitiesPaging()
		{
			return await _baseRepository.GetListPagedAsync();
		}

		/// <summary>
		/// Lấy thông tin đối tượng theo khóa chính
		/// </summary>
		/// <param name="id">ID của bản ghi</param>
		/// <returns></returns>
		/// CreateBy: NVMANH (20/04/2020)
		public async virtual Task<T> GetEntityById(object id)
		{
			return await _baseRepository.GetEntityByIdAsync(id);
		}

		/// <summary>
		/// Sửa thông tin bản ghi
		/// </summary>
		/// <param name="entity">đối tượng thực hiện sửa</param>
		/// <returns></returns>
		public async virtual Task<ActionServiceResult> Update(T entity)
		{
			if (Validate(entity) == true)
				return new ActionServiceResult
				{
					Success = true,
					Code = Code.Success,
					Message = Resources.Success,
					Data = await _baseRepository.Update(entity)
				};

			return new ActionServiceResult
			{
				Success = false,
				Code = Code.ValidateBussiness,
				Message = Resources.ValidateBussiness,
				Data = null
			};
		}

		/// <summary>
		/// Thêm mới bản ghi vào CSDL
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="returnSingleValue">Có trả về dữ liệu tùy chọn từ store hay không</param>
		/// <returns></returns>
		public async virtual Task<ActionServiceResult> Insert(T entity, bool returnSingleValue = false)
		{
			if (Validate(entity) == true)
			{
				var res = await _baseRepository.AddAsync(entity, returnSingleValue);
				if (res == null)
					return new ActionServiceResult
					{
						Success = false,
						Code = Code.ErrorAddEntity,
						Message = Resources.ErrorAddEntity,
					};
				return new ActionServiceResult
				{
					Success = true,
					Code = Code.Success,
					Message = Resources.Success,
					Data = res
				};
			}
			else
				return new ActionServiceResult
				{
					Success = false,
					Code = Code.ValidateBussiness,
					Message = Resources.ValidateBussiness,
					Data = null
				};
		}

		/// <summary>
		/// Xóa bản ghi dựa vào khóa chính
		/// </summary>
		/// <param name="id">Khóa chính</param>
		/// <returns></returns>
		public async virtual Task<int> Delete(object id)
		{
			return await _baseRepository.Delete(new object[] { id });
		}


		/// <summary>
		/// Hàm thực hiện validate trước khi cất dữ liệu
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected virtual bool Validate(T entity)
		{
			return true;
		}

	}
}
