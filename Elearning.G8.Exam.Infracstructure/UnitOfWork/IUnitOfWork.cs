using Elearning.G8.Exam.Infrastructure.DatabaseContext;
using MySql.Data.MySqlClient;
using System;

namespace Elearning.G8.Exam.Infrastructure.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		IDatabaseContext DataContext { get; }
		MySqlTransaction BeginTransaction();
		void Commit();
		void RollBack();
	}
}
