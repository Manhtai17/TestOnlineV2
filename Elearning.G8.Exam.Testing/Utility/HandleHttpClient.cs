using Elearning.G8.Exam.ApplicationCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static Elearning.G8.Exam.ApplicationCore.Enumration;

namespace Elearning.G8.Exam.Testing.Utility
{
	public class HandleHttpClient
	{
		public static HttpResponseMessage GetResponseFromAPIService(Uri baseAddress, string apiFunction, string method, string type, object param)
		{

			HttpResponseMessage response = new HttpResponseMessage();

			var handler = new HttpClientHandler()
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};

			using (var client = new HttpClient(handler))
			{
				client.BaseAddress = baseAddress;
				client.DefaultRequestHeaders.Accept.Clear();
				if (type != "form-data")
				{
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(type));
				}

				try
				{
					if (method == "GET")
					{
						response = client.GetAsync(apiFunction).Result;
					}
					else
					{
						var jsonSettings = new JsonSerializerSettings()
						{
							ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
							NullValueHandling = NullValueHandling.Ignore
						};

						if (type == "form-data")
						{
							var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(param, Formatting.None, jsonSettings));
							response = client.PostAsync(apiFunction, new FormUrlEncodedContent(dict)).Result;
						}
						else
						{
							var dict = new StringContent(JsonConvert.SerializeObject(param, Formatting.None, jsonSettings), Encoding.UTF8, type);
							response = client.PostAsync(apiFunction, dict).Result;
						}

					}
				}
				catch (Exception)
				{
					//response. = true;
					//GHI log
					try
					{
						//Ghi log
						//MBHLog.Log.Error($"Call API  ({baseAddress}) has been exception: {ex}");
					}
					catch (Exception)
					{
						//nothing
					}
					return response;
				}

			}
			return response;
			//#endif
		}

		public static bool HandleHttpClientResponse(ActionServiceResult resultBase, HttpResponseMessage responseMsg)
		{
			var success = responseMsg.IsSuccessStatusCode;
			if (!success)
			{
				var reasonPhrase = !responseMsg.IsSuccessStatusCode ? responseMsg.ReasonPhrase : "Call G12 Service has exception!";
				var statusCode = !responseMsg.IsSuccessStatusCode ? ((int)responseMsg.StatusCode).ToString() : "888";
				resultBase.Message = $"Call G12 has error: " + $"ReasonPhrase: {reasonPhrase} " + $"- StatusCode: {statusCode}";
				resultBase.Code = Code.Exception;
			}
			return success;
		}
	}
}
