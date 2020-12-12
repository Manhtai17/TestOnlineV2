using AutoMapper;
using Elearning.G8.Exam.ApplicationCore.Entitty;
using Elearning.G8.Exam.ApplicationCore.Integrations;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{

			CreateMap<Examination, ExamDTO>();
			CreateMap<Contest, ContestDTO>();
			CreateMap<Transcript, IntegrationTranscript>();
			CreateMap<IntegrationTranscript, Transcript>();
		}
	}
}
