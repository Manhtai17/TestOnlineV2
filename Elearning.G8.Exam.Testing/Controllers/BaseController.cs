using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Testing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Testing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController<T> : ControllerBase
	{
		protected readonly IBaseEntityService<T> _baseEntityService;

		public BaseController(IBaseEntityService<T> baseEntityService)
		{
			_baseEntityService = baseEntityService;
		}

		/// <summary>
		/// Lấy toàn bộ danh sách đối tượng
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public virtual async Task<ActionServiceResult> GetEntities()
		{
			var response = new ActionServiceResult();
			var entities = await _baseEntityService.GetEntities();
			response.Data = entities;
			return response;
		}

		/// <summary>
		/// Lấy thông tin theo mã (khóa chính)
		/// </summary>
		/// <param name="id">giá trị khóa chính trong bảng CSDL</param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public virtual async Task<ActionServiceResult> GetEntityByID(string id)
		{
			var response = new ActionServiceResult();
			if (string.IsNullOrEmpty(id))
			{
				response.Success = false;
				response.Code = Code.ValidateEntity;
				response.Message = Resources.ValidateEntity;
			}
			else
			{
				var entity = await _baseEntityService.GetEntityById(id);
				if (entity == null)
				{
					response.Success = false;
					response.Code = Code.NotFound;
					response.Message = Resources.NotFound;
				}
				else
				{
					response.Data = entity;
				}
			}

			return response;
		}

		/// <summary>
		/// Thêm mới
		/// </summary>
		/// <param name="entity">Đối tượng thêm mới</param>
		/// <returns></returns>
		[HttpPost]
		public virtual async Task<ActionServiceResult> Post([FromBody] T entity)
		{
			var response = new ActionServiceResult();
			// Validate dữ liệu theo các Attribure Property
			if (!ModelState.IsValid)
			{
				response.Success = false;
				response.Message = Resources.ValidateEntity;
				response.Code = Code.ValidateEntity;
				response.Data = ModelState;
			}
			else
			{
				var result = await _baseEntityService.Insert(entity, true);
				if (result.Success == false)
				{
					response.Success = false;
					response.Code = Code.ErrorAddEntity;
					response.Message = Resources.ErrorAddEntity;
				}
			}
			return response;

		}

		/// <summary>
		/// Cập nhật
		/// </summary>
		/// <param name="entity">Đối tượng sửa</param>
		/// <returns></returns>
		[HttpPut]
		public virtual async Task<ActionServiceResult> Put([FromBody] T entity)
		{
			var response = new ActionServiceResult();
			if (entity == null)
			{
				response.Success = false;
				response.Code = Code.NotFound;
				response.Message = Resources.NotFound;
			}
			else
			{
				response = await _baseEntityService.Update(entity);
			}
			return response;
		}

		/// <summary>
		/// Xóa theo ID
		/// </summary>
		/// <param name="listID">list id của đối tượng</param>
		/// <returns></returns>
		[HttpDelete]
		public virtual async Task<ActionServiceResult> Delete([FromBody] List<string> listID)
		{
			var response = new ActionServiceResult();
			if (listID.Count == 0 || listID == null)
			{
				response.Success = false;
				response.Code = Code.ValidateEntity;
				response.Message = Resources.ValidateEntity;
			}
			else
			{
				List<int> resultArray = new List<int>();
				var success = false;
				foreach (var id in listID)
				{
					var result = 0;
					if ((await _baseEntityService.GetEntityById(id)) == null)
					{
						result = 3;
					}
					else
						result = await _baseEntityService.Delete(id);
					if (result != 0)
					{
						success = true;
					}
					resultArray.Add(result);
				}
				response.Data = resultArray;
				if (success == false)
				{
					response.Success = false;
					response.Code = Code.ErrorDeleteEntity;
					response.Message = Resources.ErrorDeleteEntity;
				}

			}
			return response;
		}


	}
}
