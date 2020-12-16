using Confluent.Kafka;
using Elearning.G8.Exam.Infrastructure.DatabaseContext;
using Elearning.G8.Exam.Infrastructure.Repository;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Elearning.G8.Exam.WorkerService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					DatabaseContext.ConnectionString = "server = 104.248.149.21; port = 32267; user =root; password =12345678@Abc	; database =g8_db";
					var configuration = hostContext.Configuration;
					var consumerConfig = new ConsumerConfig();
					configuration.Bind("Kafka:ConsumerConfig", consumerConfig);

					var producerConfig = new ProducerConfig();
					configuration.Bind("Kafka:ProducerConfig", producerConfig);

					services.AddSingleton<ConsumerConfig>(consumerConfig);
					services.AddSingleton<ProducerConfig>(producerConfig);

					//services.AddTransient(typeof(IBaseEntityService<>), typeof(BaseService<>));
					services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

					services.AddTransient<IExamRepository, ExamRepository>();
					services.AddHostedService<Worker>();
				});
	}

}
