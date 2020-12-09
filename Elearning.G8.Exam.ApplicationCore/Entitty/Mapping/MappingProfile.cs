using AutoMapper;
using Elearning.G8.Exam.ApplicationCore.Entitty;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{

			CreateMap<Examination, ExamDTO>();
			CreateMap<Contest, ContestDTO>();

		}
	}
}
