using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Elearning.G8.Exam.Testing.Utility
{
	public static class Utils
	{
		/// <summary>
		/// Lấy claim jwt từ token
		/// </summary>
		/// <param name="token"></param>
		/// <param name="claimName"></param>
		/// <returns></returns>
		public static string GetClaimFromToken(string token, string claimName)
		{
			var result = string.Empty;
			if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(claimName))
			{
				var jwtToken = new JwtSecurityToken(token.StartsWith("Bearer ") || token.StartsWith("bearer ") ? token.Substring(7) : token);
				result = jwtToken.Claims.First(c => c.Type == claimName)?.Value ?? string.Empty;
			}
			return result;
		}

		/// <summary>
		/// Lấy thời gian thực theo internet
		/// </summary>
		/// <returns></returns>
		public static DateTime GetNistTime()
		{
			//var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
			//var response = myHttpWebRequest.GetResponse();
			//string todaysDates = response.Headers["date"];
			//return DateTime.ParseExact(todaysDates,
			//						   "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
			//						   CultureInfo.InvariantCulture.DateTimeFormat,
			//						   DateTimeStyles.AssumeLocal);
			return DateTime.Now;
		}


		public static int TinhDiem(string answer, string result)
		{
			var point = 0;

			var arranswer = answer.Trim().Split("|");

			var arrresult = result.Trim().Split("|");
			var arrLength = (arranswer.Length < arrresult.Length) ? (arranswer.Length - 1) : (arrresult.Length - 1);
			for (var i = 1; i < arrLength; i++)
			{
				var tmparr = arranswer[i].Split("#");
				var tmparr2 = arrresult[i].Split("#");
				if (tmparr.Length != tmparr2.Length)
				{
					i++;
				}
				else
				{
					var flag = 0;
					for (var j = 0; j < tmparr.Length; j++)
					{
						if (!tmparr[j].Trim().ToLower().Equals(tmparr2[j].Trim().ToLower()))
						{
							break;
						}
						else
						{
							flag++;
						}
					}

					if (flag == tmparr.Length)
					{
						point++;
					}

				}
			}
			return (int)(point * 1.0 / (arrLength - 1) * 10.0);
		}

	}
}
