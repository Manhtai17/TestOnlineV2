using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Elearning.G8.Exam.Testing.Interfaces;

namespace Elearning.G8.Exam.Testing.Services
{
	public class UserService : BaseService<User>, IUserService
	{
		public UserService(IBaseRepository<User> baseRepository) : base(baseRepository)
		{

		}
	}
}
