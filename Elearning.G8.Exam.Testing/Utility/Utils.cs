using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
	}
}
