using AutoMapper;
using Confluent.Kafka;
using Elearning.G8.Common.Kafka;
using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Elearning.G8.Exam.Testing.Interfaces;
using Elearning.G8.Exam.Testing.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Testing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExamsController : ControllerBase
	{
		private readonly IExamService _examService;
		private readonly IBaseRepository<Contest> _contestRepo;
		private readonly ProducerConfig _producerConfig;
		private readonly IMapper _mapper;
		private readonly IBaseEntityService<Examination> _baseEntityService;
		public ExamsController(IBaseEntityService<Examination> baseEntityService, IExamService examService, ProducerConfig producerConfig, IBaseRepository<Contest> contestRepo, IMapper mapper)
		{
			_examService = examService;
			_producerConfig = producerConfig;
			_contestRepo = contestRepo;
			_mapper = mapper;
			_baseEntityService = baseEntityService;
		}
		//[HttpGet]
		//public async Task<ActionServiceResult> GetEntityByID([FromQuery] string examID)
		//{
		//	StringValues userHeader;
		//	Request.Headers.TryGetValue("UserID", out userHeader);
		//	var userID = userHeader.FirstOrDefault().ToString();
		//	var token = Request.Headers["Authorization"].ToString();
		//	var roleName = Utils.GetClaimFromToken(token, "rolename") == "" ? "student" : Utils.GetClaimFromToken(token, "rolename");

		//	var result = new ActionServiceResult();
		//	if (userID == null || string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(contestID))
		//	{
		//		result.Success = false;
		//		result.Code = Code.NotFound;
		//	}
		//	else
		//	{
		//		var response = await _examService.(contestID);
		//		switch (roleName)
		//		{
		//			case "lecture":
		//				result.Data = response;
		//				return result;
		//			case "student":
		//				var exam = response.Where(item => item.UserId.ToString() == userID).FirstOrDefault();
		//				result.Data = exam;

		//				if (exam == null)
		//				{
		//					//Handle goi api tao de thi tu nhom 10
		//					var res = JsonConvert.SerializeObject("[{'Question':'Day la cau hoi','Type':1,'Answer':'|Dap an 1|Dap an 2 |Dap an 3'},{ 'Question':'Day la cau hoi','Type':1,'Answer':'|Dap an 1|Dap an 2 |Dap an 3'},{ 'Question':'Day la cau hoi','Type':1,'Answer':'|Dap an 1|Dap an 2 |Dap an 3'}]");
		//					//
		//					var examRes = new Exam();
		//					examRes.ContestId = Guid.Parse(contestID);
		//					examRes.CreatedDate = DateTime.Now;
		//					examRes.ModifiedDate = DateTime.Now;
		//					examRes.ExamId = Guid.NewGuid();
		//					examRes.UserId = Guid.Parse(userID);
		//					examRes.Question = res;
		//					//exam.Answer = response.Answer;
		//					examRes.IsDoing = 1;
		//					examRes.Status = 0;

		//					await _baseEntityService.Insert(examRes);

		//					var examDTO = new ExamDTO();
		//					examDTO = _mapper.Map<ExamDTO>(examRes);
		//					examDTO.Question = JsonConvert.DeserializeObject<List<QuestionDescription>>(examRes.Question);
		//					return new ActionServiceResult(true,Resources.Success, Code.Success,examDTO,0);
		//				}
		//				else
		//				{
		//					exam.ModifiedDate = DateTime.Now;
		//					result.Data = exam;
		//					await _baseEntityService.Update(exam);
		//					return result;
		//				}

		//			default:
		//				break;
		//		}
		//	}
		//	return result;
		//}

		/// <summary>
		/// Cập nhật
		/// </summary>
		/// <param name="entity">Đối tượng sửa</param>
		/// <returns></returns>
		[HttpPut]
		public async Task<ActionServiceResult> Put([FromBody] Examination entity)
		{
			StringValues userHeader;
			Request.Headers.TryGetValue("UserID", out userHeader);
			var userID = userHeader.FirstOrDefault().ToString();
			var result = new ActionServiceResult();
			if (userID == null || string.IsNullOrEmpty(userID) || entity == null)
			{
				result.Success = false;
				result.Code = Code.NotFound;
			}
			else
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
					if (entity.Status != 1)
					{
						entity.ModifiedDate = Utils.GetNistTime();
						response = await _baseEntityService.Update(entity);
					}
					return response;
				}
				return response;
			}
			return new ActionServiceResult();
		}


		[HttpGet("{examID}")]
		public async Task<ActionServiceResult> GetByID(string examID)
		{
			StringValues userHeader;
			Request.Headers.TryGetValue("UserID", out userHeader);
			var userID = userHeader.FirstOrDefault().ToString();

			var result = new ActionServiceResult();
			if (userID == null || string.IsNullOrEmpty(userID) || examID == null)
			{
				result.Success = false;
				result.Code = Code.NotFound;
			}
			else
			{
				var response = new ActionServiceResult();
				var exam = await _baseEntityService.GetEntityById(examID);
				if (exam == null)
				{
					response.Success = false;
					response.Code = Code.NotFound;
					response.Message = Resources.NotFound;

					return response;
				}
				else
				{
					var contest = await _contestRepo.GetEntityByIdAsync(exam.ContestId);
					if (DateTime.Compare(Utils.GetNistTime(), contest.FinishTime) >= 0)
					{
						if (exam.Status == 0)
						{
							//Tinh diem 
							exam.Point = 10;
							exam.Status = 1;
							await _baseEntityService.Update(exam);
						}
						exam.IsDoing = null;
						exam.Result = null;
						exam.Status = null;

					}
					else if (DateTime.Compare(Utils.GetNistTime(), contest.StartTime) < 0)
					{
						return null;
					}
					else
					{
						if (exam.TimeUsing < contest.TimeToDo)
						{
							if (exam.Status == 0)
							{
								if (exam.IsDoing == 1)
								{
									var now = Utils.GetNistTime();
									if ((now - exam.ModifiedDate.Value).TotalSeconds > 15)
									{
										if (exam.StartAgain == null)
										{
											if (exam.TimeUsing == 0)
											{
												exam.TimeUsing += (exam.ModifiedDate.Value - exam.CreatedDate.Value).TotalSeconds / 60.0;
												exam.StartAgain = exam.ModifiedDate = now;
											}
										}
										else
										{
											exam.TimeUsing += (exam.ModifiedDate.Value - exam.StartAgain.Value).TotalSeconds / 60.0;
											exam.StartAgain = exam.ModifiedDate = now;
										}

										await _baseEntityService.Update(exam);

										return new ActionServiceResult() { Success = true, Code = Code.Success, Data = exam };
									}
									else
									{
										return new ActionServiceResult() { Success = true, Code = Code.IsDoing, Data = null };
									}

								}
								else
								{
									exam.IsDoing = 1;
									exam.CreatedDate = Utils.GetNistTime();
									exam.ModifiedDate = Utils.GetNistTime();
									await _baseEntityService.Update(exam);
								}
							}
							else
							{
								exam.IsDoing = null;
								exam.Result = null;
								exam.Status = null;
							}
						}
						else
						{
							//Tinh diem 
							exam.Status = 1;
							await _baseEntityService.Update(exam);
							return new ActionServiceResult() { Success = true, Code = Code.Success, Data = exam };
						}


					}

					return new ActionServiceResult() { Success = true, Code = Code.Success, Data = exam };
				}

			}
			return new ActionServiceResult();
		}

		private bool AcceptSubmit(Examination oldExam, Examination newExam, Contest contest)
		{
			var totalTimes = oldExam.TimeUsing;
			if (oldExam.StartAgain == null)
			{
				totalTimes += ((DateTime.Now - oldExam.CreatedDate.Value).TotalSeconds) / 60.0;
			}
			else
			{
				totalTimes += ((DateTime.Now - oldExam.StartAgain.Value).TotalSeconds) / 60.0;
			}

			if (oldExam.Status == 1)
			{
				return false;
			}
			return true;
		}

		[HttpPost]
		[Route("submit")]
		public async Task<ActionServiceResult> SubmitExam(Examination exam)
		{
			StringValues userHeader;
			Request.Headers.TryGetValue("UserID", out userHeader);
			var userID = userHeader.FirstOrDefault().ToString();
			var result = new ActionServiceResult();
			if (userID == null || string.IsNullOrEmpty(userID) || exam == null)
			{
				result.Success = false;
				result.Code = Code.NotFound;
				return result;
			}

			var contest = await _contestRepo.GetEntityByIdAsync(exam.ContestId);
			var oldExam = await _baseEntityService.GetEntityById(exam.ExamId);

			if (contest == null || oldExam == null)
			{
				return new ActionServiceResult
				{
					Code = Code.Exception,
					Data = null,
					Message = "Entity null",
					Success = false

				};
			}
			exam.Question = oldExam.Question;
			exam.Answer = oldExam.Answer;
			exam.CreatedDate = oldExam.CreatedDate;
			exam.ModifiedDate = DateTime.Now;

			if (oldExam != null && oldExam.Status == 1)
			{
				return new ActionServiceResult()
				{
					Success = false,
					Code = Code.SubmitDone,
					Message = "Hệ thống đã ghi nhận bài làm trước đó"
				};
			}
			if (DateTime.Compare(Utils.GetNistTime(), contest.FinishTime) <= 0)
			{
				var totaltimes = oldExam.TimeUsing;

				totaltimes += ((DateTime.Now - oldExam.ModifiedDate.Value).TotalSeconds / 60.0);

				exam.TimeUsing = totaltimes;


				//Con thoi gian lam bai
				if (totaltimes < contest.TimeToDo)
				{
					//thuc hien tinh diem
					if (exam.Status == 0)
					{
						//tinh diem
						var message = JsonConvert.SerializeObject(exam);
						using (var producer = new ProducerWrapper<Null, string>(_producerConfig, "autosubmit"))
						{
							await producer.SendMessage(message);
						}

						return new ActionServiceResult()
						{
							Success = true,
							Code = Code.Success,
							Message = Resources.Success,
							Data = new
							{
								ExamID = exam.ExamId,
								Point = 10
							}
						};
					}
					else
					{
						//tinh diem
						await _baseEntityService.Update(exam);
						return new ActionServiceResult()
						{
							Success = true,
							Code = Code.Success,
							Message = Resources.Success,
							Data = new
							{
								ExamID = exam.ExamId,
								Point = 10
							}
						};
					}

				}
				//Vua het thoio gian lam bai
				else if (totaltimes == contest.TimeToDo)
				{
					exam.Status = 1;
					//tinh diem
					await _baseEntityService.Update(exam);
					return new ActionServiceResult()
					{
						Success = true,
						Code = Code.Success,
						Message = Resources.Success,
						Data = new
						{
							ExamID = exam.ExamId,
							Point = 10
						}
					};

				}
				//Het thoi gian lam bai
				else
				{
					exam.Status = 1;
					await _baseEntityService.Update(exam);
					return new ActionServiceResult()
					{
						Success = true,
						Code = Code.Success,
						Message = Resources.Success,
						Data = new
						{
							ExamID = exam.ExamId,
							Point = 10
						}
					};
				}



			}

			return new ActionServiceResult
			{
				Code = Code.TimeOut,
				Data = new
				{
					ExamID = exam.ExamId,
					Point = oldExam.Point
				},
				Message = "Hết thời gian làm bài",
				Success = false

			};
		}
	}
}
