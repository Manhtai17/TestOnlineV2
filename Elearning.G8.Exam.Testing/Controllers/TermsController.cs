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
	public class TermsController : ControllerBase
	{
		private readonly ITermService _termService;
		private readonly IBaseEntityService<Term> _baseEntityService;
		public TermsController(IBaseEntityService<Term> baseEntityService, ITermService termService, IContestService contestService)
		{
			_termService = termService;
			_baseEntityService = baseEntityService;
		}
		[HttpGet]
		public async Task<ActionServiceResult> Paging(string keyword, int index = 1, int size = 15)
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
				var response = await _termService.Paging(userID, index, size, keyword);
				result.Data = response;
				result.TotalRecords = await _termService.GetTotalRecords(userID, keyword);
			}
			return result;

		}

		//[HttpGet]
		//[Route("{termID}/Contests/")]
		//public ActionServiceResult GetContestsByTermID(string termID)
		//{
		//	StringValues userHeader;
		//	Request.Headers.TryGetValue("UserID", out userHeader);
		//	var userID = userHeader.FirstOrDefault().ToString();
		//	var result = new ActionServiceResult();
		//	if (userID == null || string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(termID))
		//	{
		//		result.Success = false;
		//		result.Code = ApplicationCore.Enums.Enumration.Code.NotFound;
		//	}
		//	else
		//	{
		//		var response = _contestService.GetByTermID(termID);
		//		result.Data = response;
		//	}
		//	return result;

		//}
	}
}
