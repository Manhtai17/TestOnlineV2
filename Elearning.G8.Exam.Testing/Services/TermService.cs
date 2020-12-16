using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.ApplicationCore.Integrations;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Elearning.G8.Exam.Testing.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Testing.Services
{
	public class TermService : ITermService
	{
		private readonly ITermRepository _termRepo;
		private readonly IBaseRepository<Term> _baseRepository;
		private readonly IBaseRepository<User> _baseUserRepository;
		private readonly IBaseRepository<Contest> _baseContestRepository;
		private readonly IBaseRepository<Userterm> _baseUserTermRepository;

		public TermService(ITermRepository termRepo, IBaseRepository<Term> baseRepository, IBaseRepository<User> baseUserRepository, IBaseRepository<Contest> baseContestRepository, IBaseRepository<Userterm> baseUserTermRepository)
		{
			_termRepo = termRepo;
			_baseRepository = baseRepository;
			_baseUserRepository = baseUserRepository;
			_baseContestRepository = baseContestRepository;
			_baseUserTermRepository = baseUserTermRepository;
		}

		public async Task<ActionServiceResult> Paging(string userID, int pageIndex=1, int pageSize=99, string keyword=null)
		{
			try
			{
				var result = (_termRepo.Paging(userID, pageIndex, pageSize, keyword)).ToList();

				if (result.Count == 0)
				{
					var user = await _baseUserRepository.GetEntityByIdAsync(userID);
					if (user == null)
					{
						return new ActionServiceResult()
						{
							Success = false,
							Code = Code.NotFound
						};
					}
					else
					{
						var baseUrl = String.Format("http://my-app-dkmh.herokuapp.com");
						var url = new Uri($"{baseUrl}/api/getclassbyid?id={user.IntegrationID}");

						var httpClient = new HttpClient();

						var jsonSettings = new JsonSerializerSettings()
						{
							ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
							NullValueHandling = NullValueHandling.Ignore,

						};

						var res = (await httpClient.GetAsync(url));

						if (res.StatusCode == System.Net.HttpStatusCode.OK)
						{
							var response = JsonConvert.DeserializeObject<List<IntegrationTerm>>(res.Content.ReadAsStringAsync().Result);
							var terms = new List<Term>();
							foreach (var resp in response)
							{
								var term = new Term()
								{
									TermId = Guid.NewGuid(),
									TermCode = resp.Class_id,
									TermName = resp.Subject_name
								};
								terms.Add(term);
								var existterm =( await _baseRepository.GetEntitites("Proc_GetTermByTermCode", new object[] { term.TermCode})).FirstOrDefault();
								if (existterm == null)
								{
									await _baseRepository.AddAsync(term,true);
									await _baseUserTermRepository.AddAsync(new Userterm() { TermId = term.TermId, UserId = Guid.Parse(userID), UserTermId = Guid.NewGuid() },true);

								}
								else
								{
									await _baseUserTermRepository.AddAsync(new Userterm() { TermId = existterm.TermId, UserId = Guid.Parse(userID), UserTermId = Guid.NewGuid() }, true);

								}
							}
							return new ActionServiceResult()
							{
								Success = true,
								Code = Code.Success,
								Data = terms
							};
						}
					}
				}
				return new ActionServiceResult()
				{
					Success = true,
					Code = Code.Success,
					Data = result
				};
			}
			catch(Exception ex)
			{
				var err = ex.Message;
			}

			return new ActionServiceResult()
			{
				Success = false,
				Code = Code.NotFound
			};
		}

		public Task<int> GetTotalRecords(string userID, string keyword)
		{
			var result = _baseRepository.GetTotalRecords("Proc_GetTotalTermRecords", new object[] { userID, keyword });
			return Task.FromResult(result);
		}

		/// <summary>
		/// Lấy thông tin lớp học phần và thông tin kì thi
		/// </summary>
		/// <param name="termID"></param>
		/// <returns></returns>
		public async Task<ActionServiceResult> GetByTermID(string termID)
		{
			try
			{
				var result =await _baseRepository.GetEntitites("Proc_GetTermByID", new object[] { termID });
				var contest = await _baseContestRepository.GetEntitites("Proc_GetContestsByTermID", new object[] {termID,1,99,null });
				return new ActionServiceResult()
				{
					Success = true,
					Code = Code.Success,
					Data =  new
					{
						Term = result,
						Contests = contest
					}
				};
			}
			catch(Exception ex)
			{
				
			}
			return new ActionServiceResult()
			{
				Success = false,
				Code = Code.Exception
			};

		}
	}
}
