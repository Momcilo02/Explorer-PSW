using AutoMapper;
using FluentResults;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using System.Collections.Generic;
using System.Linq;
using Explorer.Stakeholders.API.Public;

namespace Explorer.Stakeholders.Core.UseCases;

public class ApplicationGradeService : CrudService<ApplicationGradeDto, ApplicationGrade>, IApplicationGradeService
{
    private readonly IMapper _mapper;

    public ApplicationGradeService(ICrudRepository<ApplicationGrade> repository, IMapper mapper)
        : base(repository, mapper)
    {
      
    }

}


