using AutoMapper;
using Confluent.Kafka;
using Elearning.G8.Common.Kafka;
using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.ApplicationCore.Entitty;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Elearning.G8.Exam.Testing.Interfaces;
using Elearning.G8.Exam.Testing.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
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


		[HttpGet("hello")]
		public async Task<object> helloAbc()
		{
			var a = await _examService.CreateExam($"{Guid.NewGuid()}", $"{Guid.NewGuid()}" );
			return a;
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
					if(entity.Status != 1)
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
		public async Task<ActionServiceResult> GetByID( string examID)
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
					if (exam.IsDoing == 1)
					{
						if ((Utils.GetNistTime() - exam.ModifiedDate.Value).TotalSeconds > 15)
						{
							exam.ModifiedDate = Utils.GetNistTime();
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
					return new ActionServiceResult() { Success=true,Code = Code.Success,Data = exam };
				}

			}
			return new ActionServiceResult();
		}

		[HttpGet]
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
			}
			else
			{
				if (exam.Status == 0)
				{
					exam.ModifiedDate = Utils.GetNistTime();
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
						Data = exam.ExamId
					};
				}
				else
				{
					var contest = await _contestRepo.GetEntityByIdAsync(exam.ContestId);
					if (DateTime.Compare(Utils.GetNistTime(), contest.FinishTime) <= 0)
					{
						//Todo tinh diem
						exam.Point = 10;
						exam.IsDoing = 0;
						exam.Status = 1;
						exam.ModifiedDate = Utils.GetNistTime();
						await _baseEntityService.Update(exam);
						result.Data = exam.ExamId;
					}
					else
					{
						return new ActionServiceResult
						{
							Code = Code.NotFound,
							Data = null,
							Message = Resources.NotFound,
							Success = false

						};
					}
				}


			}
			return result;

		}
	}
}
