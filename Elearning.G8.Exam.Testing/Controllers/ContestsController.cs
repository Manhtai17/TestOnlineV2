using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Testing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Testing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContestsController : ControllerBase
	{
		private readonly IContestService _contestService;
		private readonly IBaseEntityService<Contest> _baseEntityService;
		public ContestsController(IBaseEntityService<Contest> baseEntityService, IContestService contestService)
		{
			_contestService = contestService;
			_baseEntityService = baseEntityService;
		}


		[HttpGet]
		public async Task<ActionServiceResult> Paging(string termID, string keyword, int index = 1, int size = 15)
		{
			StringValues userHeader;
			Request.Headers.TryGetValue("UserID", out userHeader);
			var userID = userHeader.FirstOrDefault().ToString();
			var result = new ActionServiceResult();
			if (userID == null || string.IsNullOrEmpty(userID))
			{
				result.Success = false;
				result.Code = Code.NotFound;
			}
			else
			{
				var response = await _contestService.GetByTermID(termID, index, size, keyword);
				result.TotalRecords = await _contestService.GetTotalRecords(termID, keyword);
				result.Data = response;
			}
			return result;

		}

		[HttpGet]
		[Route("{contestID}")]
		public async Task<ActionServiceResult> GetByID(string contestID)
		{
			StringValues userHeader;
			Request.Headers.TryGetValue("UserID", out userHeader);
			var userID = userHeader.FirstOrDefault().ToString();
			var result = new ActionServiceResult();
			if (userID == null || string.IsNullOrEmpty(userID))
			{
				result.Success = false;
				result.Code = Code.NotFound;
			}
			else
			{
				var response = await _contestService.CheckScreen(userID,contestID,"student");
				result.Data = response;
			}
			return result;

		}

		[HttpGet]
		[Route("ScoreStatistics")]
		public Task<ActionServiceResult> ScoreStatistics(string contestID)
		{
			StringValues userHeader;
			Request.Headers.TryGetValue("UserID", out userHeader);
			var userID = userHeader.FirstOrDefault().ToString();
			var result = new ActionServiceResult();
			if (userID == null || string.IsNullOrEmpty(userID))
			{
				result.Success = false;
				result.Code = Code.NotFound;
			}

			return Task.FromResult(_contestService.ThongKe(userID, contestID, "student"));
		}

	}
}
