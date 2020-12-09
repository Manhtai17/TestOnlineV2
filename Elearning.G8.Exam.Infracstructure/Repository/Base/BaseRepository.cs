using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.ApplicationCore.Entitty.Base;
using Elearning.G8.Exam.Infrastructure.Data;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Infrastructure.Repository
{
	public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, IAggregateRoot
	{
		#region GET_DATA
		/// <summary>
		/// Lấy toàn bộ danh sách các bản ghi của bảng có trong Database
		/// </summary>
		/// <returns></returns>
		public async Task<IReadOnlyList<T>> GetListAsync()
		{
			var storeName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Get);
			return await Task.FromResult(GetData(storeName, null).ToList());
		}

		/// <summary>
		/// Lấy thông tin đối tượng theo khóa chính
		/// </summary>
		/// <param name="id">Id của bản ghi</param>
		/// <returns></returns>
		public async Task<T> GetEntityByIdAsync(object id)
		{
			var storeName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.GetById);
			var entities = GetData(storeName, new object[] { id });
			return await Task.FromResult(entities.ToList().FirstOrDefault());
		}

		/// <summary>
		/// Lấy danh sách các đối tượng theo một bộ tham số truyền vào
		/// </summary>
		/// <returns></returns>
		public async Task<IReadOnlyList<T>> GetEntities()
		{
			var storeName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Get);
			return await Task.FromResult(GetData(storeName, null).ToList());
		}



		/// <summary>
		/// Lấy danh sách các đối tượng theo một bộ tham số truyền vào
		/// </summary>
		/// <param name="parameters">một mảng chứa các tham số sẽ truyền vào store</param>
		/// <returns></returns>
		public async Task<IReadOnlyList<T>> GetListAsync(object[] parameters)
		{
			var storeName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Get);
			return await Task.FromResult(GetData(storeName, parameters).ToList());
		}

		/// <summary>
		/// Lấy danh sách các đối tượng theo một bộ tham số truyền vào
		/// </summary>
		/// <param name="procedureName">Tên thủ thục viết trên database</param>
		/// <param name="parameters">một mảng chứa các tham số sẽ truyền vào store</param>
		/// <returns></returns>
		/// new object[] {value}
		public IEnumerable<T> GetEntities(string procedureName, object[] parameters)
		{
			var temp = GetData(procedureName, parameters);
			return temp;
		}


		public int GetTotalRecords(string procedureName, object[] parameters)
		{
			var temp = GetData(procedureName, parameters);
			return temp.Count();
		}


		//public IEnumerable<object> GetEntity(string procedureName, object[] parameters)
		//{
		//    return GetData()
		//}

		public Task<IEnumerable<T>> GetListPagedAsync()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region INSERT_DATA
		/// <summary>
		/// Thêm mới dữ liệu vào database
		/// </summary>
		/// <param name="entity">đối tượng thêm mới</param>
		/// <param name="returnValue">Có trả về dữ liệu hay không?</param>
		/// <returns></returns>
		public async Task<object> AddAsync(T entity, bool returnValue)
		{
			if (returnValue)
				return await InsertAndReturnSingleValue(entity);
			return await InsertAndReturnNumberRecordEffect(entity);
		}
		/// <summary>
		/// Thêm mới dữ liệu theo store mặc định tự sinh
		/// </summary>
		/// <param name="entity">Đối tượng sẽ thêm mới vào Database</param>
		/// <returns>Số bản ghi thêm mới thành công</returns>
		protected virtual async Task<object> InsertAndReturnNumberRecordEffect(T entity)
		{
			using (DatabaseConnector databaseConnector = new DatabaseConnector())
			{
				var storeName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Insert);
				var result = await SaveChangeAndReturnRecordEffect(databaseConnector, storeName, entity);
				databaseConnector.CommitTransaction();
				return result;
			}
		}

		/// <summary>
		/// Thêm mới dữ liệu theo store mặc định tự sinh
		/// </summary>
		/// <param name="entity">Đối tượng sẽ thêm mới vào Database</param>
		/// <returns>Số bản ghi thêm mới thành công</returns>
		protected virtual async Task<object> InsertAndReturnSingleValue(T entity)
		{
			var storeName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Insert);
			return await InsertAndReturnSingleValue(storeName, entity);
		}

		/// <summary>
		/// Thêm mới dữ liệu
		/// </summary>
		/// <param name="procedureName">Tên store</param>
		/// <param name="entity">đối tượng sẽ thêm mới</param>
		/// <returns>số bản ghi thêm mới thành công</returns>
		protected virtual async Task<object> InsertAndReturnSingleValue(string procedureName, T entity)
		{
			using (DatabaseConnector databaseConnector = new DatabaseConnector())
			{
				var value = await SaveChangeAndReturnSingleValue(databaseConnector, procedureName, entity);
				databaseConnector.CommitTransaction();
				return value;
			}
		}

		#endregion

		#region UPDATE_DATA
		/// <summary>
		/// Sửa dữ liệu
		/// </summary>
		/// <param name="procedureName">Tên store</param>
		/// <param name="entity">Đối tượng thực hiện sửa</param>
		/// <returns>Kết quả sửa</returns>
		public virtual Task<int> Update(string procedureName, T entity)
		{
			using (DatabaseConnector databaseConnector = new DatabaseConnector())
			{
				var value = SaveChangeAndReturnRecordEffect(databaseConnector, procedureName, entity);
				databaseConnector.CommitTransaction();
				return value;
			}
		}

		/// <summary>
		/// Sửa dữ liệu (tự sinh store sửa theo teamplate Update[Tên bảng]
		/// </summary>
		/// <param name="entity">Thực thể</param>
		/// <returns></returns>
		public virtual Task<int> Update(T entity)
		{
			using (DatabaseConnector databaseConnector = new DatabaseConnector())
			{
				var procedureName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Update);
				var value = SaveChangeAndReturnRecordEffect(databaseConnector, procedureName, entity);
				databaseConnector.CommitTransaction();
				return value;
			}
		}
		#endregion

		#region DELETE_DATA
		/// <summary>
		/// Xóa dữ liệu theo các điều kiện trả về số bản ghi bị xóa trong DB
		/// </summary>
		/// <param name="param">Bộ tham số truyền vào cho store - map theo thứ tự</param>
		/// <returns></returns>
		public Task<int> Delete(object[] param)
		{
			using (DatabaseConnector databaseConnector = new DatabaseConnector())
			{
				var procedureName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Delete);
				databaseConnector.SetParameterValue(procedureName, param);
				var result = databaseConnector.ExecuteNonQuery();
				databaseConnector.CommitTransaction();
				return result;
			}

		}
		public Task<int> Delete(string procedureName, object[] parameters)
		{
			using (DatabaseConnector databaseConnector = new DatabaseConnector())
			{
				if (parameters != null && parameters.Length > 0)
					databaseConnector.SetParameterValue(procedureName, parameters);
				var result = databaseConnector.ExecuteNonQuery();
				databaseConnector.CommitTransaction();
				return result;
			}

		}

		#endregion

		#region BASE_METHOD
		/// <summary>
		/// Build dữ liệu trả về từ MySqlDataReader
		/// </summary>
		/// <param name="databaseConnector">MySqlDataReader</param>
		/// <param name="procedureName">Tên procedure</param>
		/// <param name="parameters">bộ tham số</param>
		/// <returns></returns>
		protected IEnumerable<T> GetData(string procedureName, object[] parameters)
		{
			using (DatabaseConnector databaseConnector = new DatabaseConnector())
			{
				if (parameters != null && parameters.Length > 0)
					databaseConnector.SetParameterValue(procedureName, parameters);
				using (MySqlDataReader mySqlDataReader = databaseConnector.ExecuteReader(procedureName))
				{
					while (mySqlDataReader.Read())
					{
						var entity = Activator.CreateInstance<T>();
						for (int i = 0; i < mySqlDataReader.FieldCount; i++)
						{
							string fieldName = mySqlDataReader.GetName(i);
							PropertyInfo property = entity.GetType().GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
							var fieldValue = mySqlDataReader[fieldName];
							if (property != null && fieldValue != DBNull.Value)
							{
								property.SetValue(entity, fieldValue, null);
							}
						}
						yield return entity;
					}
				}
			}
		}
		protected IEnumerable<T> GetData(string procedureName, object[] parameters, DatabaseConnector databaseConnector)
		{
			if (parameters != null && parameters.Length > 0)
				databaseConnector.SetParameterValue(procedureName, parameters);
			using (MySqlDataReader mySqlDataReader = databaseConnector.ExecuteReader(procedureName))
			{
				while (mySqlDataReader.Read())
				{
					var entity = Activator.CreateInstance<T>();
					for (int i = 0; i < mySqlDataReader.FieldCount; i++)
					{
						string fieldName = mySqlDataReader.GetName(i);
						PropertyInfo property = entity.GetType().GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
						var fieldValue = mySqlDataReader[fieldName];
						if (property != null && fieldValue != DBNull.Value)
						{
							property.SetValue(entity, fieldValue, null);
						}
					}
					yield return entity;
				}
			}
		}



		/// <summary>
		/// Lưu lại các thay đổi (thêm mới hoặc sửa)
		/// </summary>
		/// <param name="databaseConnector"></param>
		/// <param name="procedureName"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected Task<int> SaveChangeAndReturnRecordEffect(DatabaseConnector databaseConnector, string procedureName, T entity)
		{
			databaseConnector.MapParameterValueAndEntityProperty<T>(procedureName, entity);
			return databaseConnector.ExecuteNonQuery();
		}

		/// <summary>
		/// Lưu lại các thay đổi (thêm mới hoặc sửa)
		/// </summary>
		/// <param name="databaseConnector"></param>
		/// <param name="procedureName"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		private Task<object> SaveChangeAndReturnSingleValue(DatabaseConnector databaseConnector, string procedureName, T entity)
		{
			databaseConnector.MapParameterValueAndEntityProperty<T>(procedureName, entity);
			return databaseConnector.ExecuteScalar();
		}

		#endregion

		#region EXTEND_METHOD

		public virtual Task<IReadOnlyList<T>> GetListAsyncBySpecification(ISpecification<T> spec)
		{
			throw new NotImplementedException();
		}

		public virtual Task UpdateAsync(T entity)
		{
			throw new NotImplementedException();
		}

		public virtual Task DeleteAsync(T entity)
		{
			throw new NotImplementedException();
		}

		public virtual Task<int> CountAsync(ISpecification<T> spec)
		{
			throw new NotImplementedException();
		}

		public virtual Task<T> AddAsync(T entity)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<T>> GetListAsync(string procedureName, object[] parameters)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region HÀM BASE KHONG COMMIT TRANSITION


		#region INSERT DATA

		/// <summary>
		/// Thêm mới với database với connector truyền vào
		/// </summary>
		/// <param name="entity">Dữ liệu thêm mới</param>
		/// <param name="procedureName">procuder thêm mới</param>
		/// <param name="databaseConnector">connector</param>
		/// <returns></returns>
		public async Task<object> AddAsync(T entity, string procedureName, DatabaseConnector databaseConnector)
		{
			return await InsertAndReturnSingleValue(procedureName, entity, databaseConnector);
		}
		/// <summary>
		/// Thêm mới nhưng chưa commit transition
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="returnValue"></param>
		/// <param name="databaseConnector"></param>
		/// <returns></returns>
		public async Task<object> AddAsync(T entity, bool returnValue, DatabaseConnector databaseConnector)
		{
			if (returnValue)
				return await InsertAndReturnSingleValue(entity, databaseConnector);
			return await InsertAndReturnNumberRecordEffect(entity, databaseConnector);
		}
		/// <summary>
		/// Thêm mới sử dụng connnector là tham số đầu vào Trả về số bản ghi thành công
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="databaseConnector"></param>
		/// <returns></returns>
		protected virtual async Task<object> InsertAndReturnNumberRecordEffect(T entity, DatabaseConnector databaseConnector)
		{

			var storeName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Insert);
			var result = await SaveChangeAndReturnRecordEffect(databaseConnector, storeName, entity);
			return result;
		}
		/// <summary>
		/// Thêm mới và trả về kết quả từ store sử dụng connector truyền vào
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="databaseConnector"></param>
		/// <returns></returns>
		protected virtual async Task<object> InsertAndReturnSingleValue(T entity, DatabaseConnector databaseConnector)
		{
			var storeName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Insert);
			return await InsertAndReturnSingleValue(storeName, entity, databaseConnector);
		}
		/// <summary>
		/// Thêm mới và trả về kết quả từ store
		/// Chưa commit transition
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="entity"></param>
		/// <param name="databaseConnector"></param>
		/// <returns></returns>
		protected virtual async Task<object> InsertAndReturnSingleValue(string procedureName, T entity, DatabaseConnector databaseConnector)
		{
			var value = await SaveChangeAndReturnSingleValue(databaseConnector, procedureName, entity);
			return value;
		}
		#endregion

		#region UPDATE DATA
		/// <summary>
		/// Update bảng sử dụng param databaseConnector
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="databaseConnector"></param>
		/// <returns></returns>
		public async Task<int> Update(T entity, DatabaseConnector databaseConnector)
		{
			var procedureName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Update);
			var value = await SaveChangeAndReturnRecordEffect(databaseConnector, procedureName, entity);
			return value;
		}

		#endregion

		#region DELETE
		/// <summary>
		/// Xóa có tham số connector
		/// </summary>
		/// <param name="param"></param>
		/// <param name="databaseConnector"></param>
		/// <returns></returns>
		public Task<int> Delete(object[] param, DatabaseConnector databaseConnector)
		{

			var procedureName = DatabaseUtility.GeneateStoreName<T>(ProcdureTypeName.Delete);
			databaseConnector.SetParameterValue(procedureName, param);
			var result = databaseConnector.ExecuteNonQuery();
			return result;

		}
		/// <summary>
		/// Xóa có tham số connectot
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public Task<int> Delete(string procedureName, object[] parameters, DatabaseConnector databaseConnector)
		{
			if (parameters != null && parameters.Length > 0)
				databaseConnector.SetParameterValue(procedureName, parameters);
			var result = databaseConnector.ExecuteNonQuery();
			databaseConnector.CommitTransaction();
			return result;

		}

		#endregion

		#region EXTEND METHOD
		/// <summary>
		/// Thực thư với databaseConnector truyền vào
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="parameters"></param>
		/// <param name="databaseConnector"></param>
		/// <returns></returns>
		public async Task<object> Excute(string procedureName, object[] parameters, DatabaseConnector databaseConnector)
		{
			if (parameters != null && parameters.Length > 0)
				databaseConnector.SetParameterValue(procedureName, parameters);
			return await databaseConnector.ExecuteScalar();
		}

		public Task<IEnumerable<T>> GetEntitites(string procedureName, object[] param)
		{
			var temp = GetData(procedureName, param);
			return Task.FromResult(temp);
		}
		/// <summary>
		/// Lấy 1 bản ghi
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public Task<T> GetEntity(string procedureName, object[] param)
		{
			var tem = GetData(procedureName, param).FirstOrDefault();
			return Task.FromResult(tem);
		}


		/// <summary>
		/// Lấy thông tin từ procudre
		/// </summary>
		/// <param name="procedureName">Tên procedure</param>
		/// <param name="parameters">Tham số truyền vào</param>
		/// <returns></returns>
		public Task<object> Get(string procedureName, object[] parameters)
		{
			using (DatabaseConnector databaseConnector = new DatabaseConnector())
			{
				if (parameters != null && parameters.Length > 0)
					databaseConnector.SetParameterValue(procedureName, parameters);
				var value = databaseConnector.ExecuteScalar();
				return value;
			}
		}

		



		#endregion


		#endregion
	}
}
