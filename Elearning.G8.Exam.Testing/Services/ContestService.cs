using AutoMapper;
using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Elearning.G8.Exam.Testing.Interfaces;
using Elearning.G8.Exam.Testing.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Testing.Services
{
	public class ContestService : IContestService
	{
		private readonly IContestRepository _contestRepo;
		private readonly IBaseRepository<Contest> _contestBaseRepository;
		private readonly IBaseRepository<Examination> _examBaseRepository;
		private readonly IBaseRepository<Transcript> _transcriptBaseRepository;
		private readonly IBaseRepository<User> _userBaseRepository;

		private readonly IExamService _examService;
		private readonly IMapper _mapper;
		private readonly Dictionary<string, string> _role;

		public ContestService(IContestRepository contestRepo, IBaseRepository<Contest> baseRepository, IBaseRepository<Examination> examBaseRepository, IBaseRepository<Transcript> transcriptBaseRepository, IMapper mapper, IExamService examService, IBaseRepository<User> userBaseRepository)
		{
			_contestRepo = contestRepo;
			_contestBaseRepository = baseRepository;
			_examBaseRepository = examBaseRepository;
			_transcriptBaseRepository = transcriptBaseRepository;
			_mapper = mapper;
			_examService = examService;
			_userBaseRepository = userBaseRepository;

			_role = new Dictionary<string, string>()
			{
				{ "01014640-4992-8450-3665-126150814651","lecture"},
				{"01625518-9205-2988-5145-017982868048","student" }
			};
		}



		public Task<IEnumerable<Contest>> GetByTermID(string termID, int indexPage, int sizePage, string keyword)
		{
			var result = _contestRepo.GetByTermID(termID, indexPage, sizePage, keyword);
			return Task.FromResult(result);
		}

		public Task<int> GetTotalRecords(string termID, string keyword)
		{
			var result = _contestBaseRepository.GetTotalRecords("Proc_GetTotalContestRecords", new object[] { termID, keyword });
			return Task.FromResult(result);
		}

		/// <summary>
		/// Kiểm tra màn hình sau click vào kì thì sẽ là bắt đầu , tiếp tục hay thống kê
		/// </summary>
		/// <param name="termID"></param>
		/// <param name="indexPage"></param>
		/// <param name="sizePage"></param>
		/// <param name="keyword"></param>
		/// <returns></returns>
		public async Task<ActionServiceResult> CheckScreen(string userID, string contestID)
		{
			var res = new ActionServiceResult();
			var result = await _contestBaseRepository.GetEntityByIdAsync(contestID);

			if (result == null)
			{
				return null;
			}
			else
			{
				var user =await  _userBaseRepository.GetEntityByIdAsync(userID);
				if (user == null)
				{
					return null;
				}

				var roleName = string.IsNullOrEmpty(_role.GetValueOrDefault(user.RoleId.ToString()))?"student": _role.GetValueOrDefault(user.RoleId.ToString());

				var contestDTO = new ContestDTO();
				var exams = _examBaseRepository.GetEntitites("Proc_GetExamByContestID", new object[] { contestID }).Result.ToList();
				var exam = exams.Where(item => item.UserId == Guid.Parse(userID)).FirstOrDefault();

				if (roleName.Equals("student"))
				{
					
					if (DateTime.Compare(Utils.GetNistTime(), result.StartTime) < 0)
					{
						//Tao de thi
						var exam_new = await _examService.CreateExam(contestID, userID);
						await _examBaseRepository.AddAsync(exam_new, true);
						return new ActionServiceResult(true, "Chưa đến thời gian làm bài", Code.NotTimeToDo, exam.ExamId, 0);
					}

					else if (DateTime.Compare(Utils.GetNistTime(), result.FinishTime) > 0)
					{
						if (exam == null)
						{
							var exam_new = await _examService.CreateExam(contestID, userID);
							exam_new.Status = 1;
							exam_new.Point = 0;
							exam_new.CreatedDate = exam_new.ModifiedDate = Utils.GetNistTime();
							await _examBaseRepository.AddAsync(exam_new, true);
							contestDTO.ExamID = exam_new.ExamId;
						}
						else
						{
							contestDTO.ExamID = exam.ExamId;
						}
						contestDTO.Continue = 2;
						return new ActionServiceResult(true, "Đã hết thời gian làm bài", Code.TimeOut, contestDTO, 0);
					}
					else
					{
						if (exam == null)
						{
							contestDTO.Continue = 0;
							var exam_new = await _examService.CreateExam(contestID, userID);
							await _examBaseRepository.AddAsync(exam_new, true);
							contestDTO.ExamID = exam_new.ExamId;
							//Tao bai thi
						}
						else
						{
							contestDTO.ExamID = exam.ExamId;
							if (exam.IsDoing == 1 && DateTime.Compare(Utils.GetNistTime(), exam.ModifiedDate.Value) > 0 && exam.Status == 0)
							{
								contestDTO.Continue = 1;
							}
							if (exam.Status == 1)
							{
								contestDTO.Continue = 2;
							}
						}
					}

					res.Data = contestDTO;
				}
				else
				{
					if(DateTime.Compare(Utils.GetNistTime(),result.FinishTime) < 0)
					{
						var data = new
						{
							NumOfJoining = exams.Count(item => item.IsDoing == 1)
						};
						return new ActionServiceResult(true, "Đang trong thời gian làm bài", (Code)1001, data, 0);
					}
					else
					{
						return new ActionServiceResult(true, "", (Code)1001, exams, 0);
					}
					
				}

				
			}

			return res;
		}

		public ActionServiceResult ThongKe(string userID, string contestID, string roleName)
		{
			var result = new ActionServiceResult();
			var exams = _examBaseRepository.GetEntitites("Proc_GetExamByContestID", new object[] { contestID }).Result.ToList();
			var transcripts = _transcriptBaseRepository.GetEntitites("Proc_GetTranscriptsByContestID", new object[] { contestID }).Result;
			switch (roleName)
			{
				case "student":
					result.Data = transcripts.Where(item => item.UserId == Guid.Parse(userID));
					break;
				case "lecture":
					result.Data = transcripts;
					break;
			}
			return result;
		}
	}
}
