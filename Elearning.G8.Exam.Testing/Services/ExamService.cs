using AutoMapper;
using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Elearning.G8.Exam.Testing.Interfaces;
using Elearning.G8.Exam.Testing.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Testing.Services
{
	public class ExamService : IExamService
	{

		private readonly IExamRepository _examRepo;
		private readonly IBaseRepository<Contest> _contestRepo;
		private readonly IMapper _mapper;

		public ExamService(IExamRepository examRepo, IBaseRepository<Contest> contestRepo, IMapper mapper)
		{
			_examRepo = examRepo;
			_contestRepo = contestRepo;
			_mapper = mapper;
		}

		/// <summary>
		/// Bat dau lam bai
		/// </summary>
		/// <param name="exam"></param>
		/// <returns></returns>
		public async Task<Examination> StartDoing(string examID)
		{
			var exam = await _examRepo.GetEntityByIdAsync(examID);
			exam.CreatedDate = Utils.GetNistTime();
			await _examRepo.Update(exam);
			return exam;
		}


		public async Task<ActionServiceResult> GetByUserID(string userID, string contestID, string roleName)
		{
			var result = new ActionServiceResult();
			var response = _examRepo.GetExamByContestID(contestID);
			switch (roleName)
			{
				case "lecture":
					result.Data = response;
					return result;
				case "student":
					var exam = response.Where(item => item.UserId.ToString().Trim() == userID.Trim()).FirstOrDefault();

					if (exam == null)
					{
						//Handle goi api tao de thi tu nhom 10
						/*var res = JsonConvert.SerializeObject("[{'Question':'Day la cau hoi','Type':1,'Answer':'|Dap an 1|Dap an 2 |Dap an 3'},{ 'Question':'Day la cau hoi','Type':1,'Answer':'|Dap an 1|Dap an 2 |Dap an 3'},{ 'Question':'Day la cau hoi','Type':1,'Answer':'|Dap an 1|Dap an 2 |Dap an 3'}]");
						
						var examRes = new Exam();
						examRes.ContestId = Guid.Parse(contestID);
						examRes.ExamId = Guid.NewGuid();
						examRes.UserId = Guid.Parse(userID);
						examRes.Question = res;
						//exam.Answer = response.Answer;
						examRes.IsDoing = 0;
						examRes.Status = 0;*/

						var examRes = await CreateExam(contestID, userID);


						await _examRepo.AddAsync(examRes);

						var examDTO = new ExamDTO();
						examDTO = _mapper.Map<ExamDTO>(examRes);
						examDTO.Question = JsonConvert.DeserializeObject<List<QuestionDescription>>(examRes.Question);
						return new ActionServiceResult(true, Resources.Success, Code.Success, examDTO, 0);
					}
					else
					{
						exam.ModifiedDate = DateTime.Now;
						result.Data = exam;
						await _examRepo.Update(exam);
						return result;
					}

				default:
					break;
			}
			return result;
		}


		public async Task<Examination> CreateExam(string contestID, string userID)
		{

			//Handle goi api tao de thi tu nhom 10
			var listQ = new List<Question>();
			var examRes = new Examination();
			using (var httpClient = new HttpClient())
			{
				var url = new Uri("http://qbms-public-api.azurewebsites.net/api/Exam/GetExam/3");
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				var content = await httpClient.GetAsync(url);

				if (content.IsSuccessStatusCode)
				{
					var result = content.Content.ReadAsStringAsync().Result;


					var exam = JsonConvert.DeserializeObject<G10Res<G10Contest>>(result);

					var questions = exam.Data.Questions;
					var g8Questions = new List<Question>();

					var strQ = String.Empty;
					var strA = String.Empty;
					var strAnswerCorrect = String.Empty;

					foreach (var question in questions)
					{
						if (question.Answers.Count() == 0)
						{
							var ques = new Question()
							{
								QuestionTitle = question.Content,
								Type = question.Type
							};
							var tmp = "None";
							strAnswerCorrect += ("|" + tmp );
							listQ.Add(ques);
						}
						else
						{
							foreach (var answer in question.Answers)
							{
								if ((bool)(answer?.IsCorrect))
								{
									strAnswerCorrect += "|" + (String.IsNullOrEmpty(answer.Content) ? ("None") : answer.Content);
								}
								strA += "|" + answer.Content;
							}

							var ques = new Question()
							{
								QuestionTitle = question.Content,
								Answer = strA,
								Type = question.Type
							};
							listQ.Add(ques);
						}
						
						strA = String.Empty;
					}

					
					examRes.ContestId = Guid.Parse(contestID);
					examRes.ExamId = Guid.NewGuid();
					examRes.UserId = Guid.Parse(userID);
					examRes.Question = JsonConvert.SerializeObject(listQ);
					examRes.Answer = strAnswerCorrect;
				}
				else
				{
					return null;
				}
			}
			//var res = JsonConvert.SerializeObject("[{'Question':'Day la cau hoi','Type':1,'Answer':'|Dap an 1|Dap an 2 |Dap an 3'},{ 'Question':'Day la cau hoi','Type':1,'Answer':'|Dap an 1|Dap an 2 |Dap an 3'},{ 'Question':'Day la cau hoi','Type':1,'Answer':'|Dap an 1|Dap an 2 |Dap an 3'}]");
			//
			
			examRes.IsDoing = 0;
			examRes.Status = 0;
			//await _examRepo.AddAsync(examRes);
			return examRes;

		}

		


		//public async Task<Exam> GetExamForStudent(string userID, string contestID, IEnumerable<Exam> exams)
		//{
		//	var exam = exams.Where(item => item.UserId.ToString() == userID).FirstOrDefault();
		//	if (exam == null)
		//	{
		//		//Handle goi api tao de thi tu nhom 10
		//		var response = new List<Question>{
		//					new Question{
		//						QuestionID ="123",
		//						QuestionTitle="Day la cau hoi",
		//						Type=1,
		//						Answer="|Dap an 1|Dap an 2 |Dap an 3"
		//					}
		//				};
		//		//
		//		var examRes = new Exam();
		//		examRes.ContestId = Guid.Parse(contestID);
		//		examRes.CreatedDate = DateTime.Now;
		//		examRes.ModifiedDate = exam.CreatedDate;
		//		examRes.ExamId = Guid.NewGuid();
		//		examRes.UserId = Guid.Parse(userID);
		//		examRes.Question = response;
		//		//exam.Answer = response.Answer;
		//		exam.IsDoing = 1;
		//		exam.Status = 0;

		//		await _examRepo.UpdateAsync(exam);
		//		return examRes;
		//	}
		//	else
		//	{
		//		var contest = await _contestRepo.GetEntityByIdAsync(contestID);
		//		if (exam.Status == 1 || DateTime.Compare(contest.FinishTime, DateTime.Now) <= 0)
		//		{
		//			exam.Question = null;
		//			exam.Answer = null;

		//			return exam;
		//		}
		//		else
		//		{

		//			//đang có người làm 
		//			if (exam.IsDoing == 1 || DateTime.Compare(contest.StartTime, DateTime.Now) > 0)
		//			{
		//				return new Exam();
		//			}
		//			var lastExam = exams.OrderByDescending(item => item.ModifiedDate).Take(1);
		//			if ((exam.ModifiedDate - exam.CreatedDate - TimeSpan.FromMinutes(contest.TimeToDo) > TimeSpan.Zero && DateTime.Now - exam.ModifiedDate > TimeSpan.FromSeconds(30))
		//				)
		//			{
		//				return exam;
		//			}
		//			if (DateTime.Compare(contest.FinishTime, DateTime.Now) <= 0)
		//			{

		//				exam.IsDoing = 0;
		//				exam.Status = 1;
		//				await _examRepo.Update(exam);

		//				exam.Question = null;
		//				exam.Answer = null;
		//				return exam;
		//			}

		//		}
		//		//var contest = await _contestRepo.GetEntityByIdAsync(contestID);
		//		//đang có người làm 
		//		if (exam.IsDoing == 1 || DateTime.Compare(contest.StartTime, DateTime.Now) > 0)
		//		{
		//			return new Exam();
		//		}
		//		//Đã submit không cho làm lại (xem đề)
		//		if (exam.Status == 1)
		//		{
		//			exam.Question = null;
		//			exam.Answer = null;

		//			return exam;
		//		}
		//		//var lastExam = exams.OrderByDescending(item=>item.ModifiedDate).Take(1) ;
		//		if ((exam.ModifiedDate - exam.CreatedDate - TimeSpan.FromMinutes(contest.TimeToDo) > TimeSpan.Zero && DateTime.Now - exam.ModifiedDate > TimeSpan.FromSeconds(30))
		//			||
		//			)
		//		{
		//			return exam;
		//		}
		//		if (DateTime.Compare(contest.FinishTime, DateTime.Now) <= 0)
		//		{
		//			if (exam.Status == 0)
		//			{
		//				exam.IsDoing = 0;
		//				exam.Status = 1;
		//				await _examRepo.Update(exam);
		//			}
		//			else
		//			{
		//				exam.IsDoing = 0;
		//			}
		//			exam.Question = null;
		//			exam.Answer = null;
		//			return exam;
		//		}
		//	}
		//	return new Exam();
		//}
	}
}
