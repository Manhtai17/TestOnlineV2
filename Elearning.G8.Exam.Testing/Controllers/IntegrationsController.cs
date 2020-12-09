using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Elearning.G8.Exam.Testing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Testing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IntegrationsController : ControllerBase
	{
		private readonly Uri _uri;
		private readonly IUserService _userService;
		private readonly IBaseEntityService<Contest> _baseContestService;
		private readonly IBaseEntityService<Transcript> _baseTranscriptService;
		private readonly IBaseRepository<Transcript> _baseTranscriptRepo;


		public IntegrationsController(IUserService userService, IBaseEntityService<Contest> baseContestService, IBaseEntityService<Transcript> baseTranscriptService, IBaseRepository<Transcript> baseTranscriptRepo)
		{
			_uri = new Uri("http://api.toedu.me");
			_userService = userService;
			_baseContestService = baseContestService;
			_baseTranscriptService = baseTranscriptService;
			_baseTranscriptRepo = baseTranscriptRepo;
		}

		[HttpGet("healthz")]

		public string Health()
		{
			return "Api g8 actice";
		}
		/// <summary>
		/// Đăng ký nhưng không thấy hợp lý nên tạm bỏ
		/// 
		/// </summary>
		/// <param name="userG12"></param>
		/// <returns></returns>
		[HttpPost("register")]
		public async Task<IActionResult> Register(UserG12 userG12)
		{
			if (userG12 == null)
			{
				return BadRequest(
					new ActionServiceResult()
					{
						Success = false,
						Code = Code.ValidateEntity,
						Message = Resources.ValidateEntity,
						Data = null
					});
			}
			var url = _uri + "api/Intergrates/register";

			var httpClient = new HttpClient();

			var jsonSettings = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore
			};
			var dict = new StringContent(JsonConvert.SerializeObject(userG12, Formatting.None, jsonSettings), Encoding.UTF8, "application/json");


			var res = (await httpClient.PostAsync(url, dict));

			if (res.StatusCode == System.Net.HttpStatusCode.OK)
			{
				var result = JsonConvert.DeserializeObject<ResObjectG12<User>>(res.Content.ReadAsStringAsync().Result);
				if (result.Code == 200 && result.Data != null)
				{
					//update db
					result.Data.RoleId = Guid.Parse("01625518-9205-2988-5145-017982868048");
					return Ok(result.Data.Id);
				}
			}

			return BadRequest();

		}

		/// <summary>
		/// Đăng nhập
		/// </summary>
		/// <param name="loginForm"></param>
		/// <returns></returns>
		[HttpPost("login")]
		public async Task<IActionResult> login(LoginForm loginForm)
		{
			if (loginForm == null)
			{
				return BadRequest(
					new ActionServiceResult()
					{
						Success = false,
						Code = Code.ValidateEntity,
						Message = Resources.ValidateEntity,
						Data = null
					});
			}
			var url = _uri + "api/Intergrates/basicauth";

			var httpClient = new HttpClient();

			var jsonSettings = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore
			};
			var dict = new StringContent(JsonConvert.SerializeObject(loginForm, Formatting.None, jsonSettings), Encoding.UTF8, "application/json");


			var res = (await httpClient.PostAsync(url, dict));

			if (res.StatusCode == System.Net.HttpStatusCode.OK)
			{
				var result = JsonConvert.DeserializeObject<ResObjectG12<UserInfo>>(res.Content.ReadAsStringAsync().Result);
				if (result.Code == 200 && result.Data != null)
				{

					return Ok(result.Data.Token);
				}
			}

			return BadRequest();

		}

		[HttpGet("Transcript")]

		public async Task<ActionServiceResult> GetTranscript(string contestID)
		{
			StringValues clientIDHeader;
			Request.Headers.TryGetValue("ClientID", out clientIDHeader);
			var clientID = clientIDHeader.FirstOrDefault().ToString();
			var result = new ActionServiceResult();
			if (clientID == null || clientID.Trim() != "714b320c-1046-4e37-a3c3-20bc6fcac014" || String.IsNullOrEmpty(clientID) || false)
			{
				return new ActionServiceResult()
				{
					Success = false,
					Code = Code.ValidateEntity,
					Message = "Đầu vào dữ liệu không hợp lệ"
				};
			}
			else
			{
				var contest = await _baseContestService.GetEntityById(contestID);

				if (contest == null)
				{
					return new ActionServiceResult()
					{
						Success = false,
						Code = Code.ValidateEntity,
						Message = "Không tồn tại kì thi"
					};
				}
				else
				{
					var response = await _baseTranscriptRepo.GetEntitites("Proc_GetTranscriptsByContestID", new object[] { contestID });
					if (response == null)
						return new ActionServiceResult()
						{
							Code = Code.Exception,
							Success = false,
							Data = null
						};
					else
						result.Data = response;
				}
			}

			return result;

		}
	}
}
