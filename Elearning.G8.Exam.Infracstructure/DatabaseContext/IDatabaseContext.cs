using MySql.Data.MySqlClient;

namespace Elearning.G8.Exam.Infrastructure.DatabaseContext
{
	public interface IDatabaseContext
	{
		MySqlConnection Connection { get; }

	}
}
