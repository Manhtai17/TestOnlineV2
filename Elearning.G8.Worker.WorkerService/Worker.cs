using Confluent.Kafka;
using Elearning.G8.Common.Kafka;
using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elearning.G8.Exam.WorkerService
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly List<string> _topics;
		private readonly IConfiguration _configuration;
		private readonly IExamRepository _examRepo;

		public Worker(ILogger<Worker> logger, IConfiguration configuration, IExamRepository examRepo)
		{
			_logger = logger;

			_configuration = configuration;
			_examRepo = examRepo;

			_topics = new List<string>()
			{
				"usersubmit","autosubmit"
			};
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{

			while (!stoppingToken.IsCancellationRequested)
			{
				foreach (var topic in _topics)
				{

					var t = new Thread(async () =>
					{
						var consumerConfig = new ConsumerConfig();
						_configuration.Bind("Kafka:ConsumerConfig", consumerConfig);
						consumerConfig.GroupId = "Elearning";
						consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
						using (var consumer = new ConsumerWrapper<Null, string>(consumerConfig, topic))
						{
							while (true)
							{
								try
								{
									var message = consumer.ReadMessage();
									if (message == null)
									{
										continue;
									}

									var exam = JsonConvert.DeserializeObject<Examination>(message);
									

									var oldExam = (_examRepo.GetEntityByIdAsync(exam.ExamId).Result);

									

									if (oldExam == null)
									{
										continue;
									}
									Console.WriteLine(oldExam.ExamId);
									if (oldExam.Status != 1)
									{
										var c = await _examRepo.Update(exam);
									}

								}
								catch (Exception ex)
								{
									Console.WriteLine(ex);
								}
							}
						}
					});
					t.Start();
					await Task.Delay(3000, stoppingToken);
					_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				}
			}

		}
	}
}
