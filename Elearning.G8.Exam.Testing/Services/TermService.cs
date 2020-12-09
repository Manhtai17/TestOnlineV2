using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Elearning.G8.Exam.Testing.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elearning.G8.Exam.Testing.Services
{
	public class TermService : ITermService
	{
		private readonly ITermRepository _termRepo;
		private readonly IBaseRepository<Term> _baseRepository;

		public TermService(ITermRepository termRepo, IBaseRepository<Term> baseRepository)
		{
			_termRepo = termRepo;
			_baseRepository = baseRepository;
		}

		public Task<IEnumerable<Term>> Paging(string userID, int pageIndex, int pageSize, string keyword)
		{
			var result = _termRepo.Paging(userID, pageIndex, pageSize, keyword);
			return Task.FromResult(result);
		}

		public Task<int> GetTotalRecords(string userID, string keyword)
		{
			var result = _baseRepository.GetTotalRecords("Proc_GetTotalTermRecords", new object[] { userID, keyword });
			return Task.FromResult(result);
		}
	}
}
