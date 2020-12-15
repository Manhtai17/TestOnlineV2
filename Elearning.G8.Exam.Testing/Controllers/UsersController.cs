using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Testing.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.G8.Exam.Testing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : BaseController<User>
	{
		private readonly IBaseEntityService<User> _baseEntityService;

		public UsersController(IBaseEntityService<User> baseEntityService):base(baseEntityService)
		{

		}
	}
}
