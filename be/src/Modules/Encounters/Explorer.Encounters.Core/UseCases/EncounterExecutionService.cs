using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using FluentResults;
using EncounterExecutionStatus = Explorer.Encounters.API.Dtos.EncounterExecutionStatus;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Encounters.Core.UseCases
{
    public class EncounterExecutionService : CrudService<EncounterExecutionDto, EncounterExecution>, IEncounterExecutionService
    {
        private readonly IMapper _mapper;
        private readonly IEncounterExecutionRepository _encounterExecutionRepository;
        private readonly IEncounterRepository _encounterRepository;
        public EncounterExecutionService(IMapper mapper, IEncounterExecutionRepository encounterExecutionRepository, IEncounterRepository encounterRepository) : base(mapper)
        {
            _mapper = mapper;
            _encounterExecutionRepository = encounterExecutionRepository;
            _encounterRepository = encounterRepository;
        }
        public Result<EncounterExecutionDto> Get(int id)
        {
            var result = _encounterExecutionRepository.Get(id);
            return MapToDto(result);
        }
        public Result<PagedResult<EncounterExecutionDto>> GetPaged(int page, int pageSize)
        {
            var result = _encounterExecutionRepository.GetPaged(page, pageSize);
            return MapToDto(result);
        }
        public Result<EncounterExecutionDto> Update(EncounterExecutionDto encounter)
        {
            try
            {
                var result = _encounterExecutionRepository.Update(MapToDomain(encounter));
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        public Result<List<EncounterExecutionDto>> GetAllTouristEncounters(int touristId)
        {
            try
            {
                var result = _encounterExecutionRepository.GetAllByUserId(touristId);
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
        public Result<EncounterExecutionDto> GetActivatedTouristEncounter(int touristId)
        {
            try
            {
                var result = _encounterExecutionRepository.GetUserActiveEncounter(touristId);
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
        public Result ActivateEncounter(EncounterExecutionDto encounterExecution)
        {
            try
            {
                var encounter = _encounterRepository.GetById(encounterExecution.EncounterId);
                if (!IsWithinRange(encounterExecution.TouristLatitude, encounterExecution.TouristLongitude,
                        encounter.Latitude, encounter.Longitude, encounter.ActivateRange))
                {
                    throw new InvalidOperationException("Tourist is not within the activation range of the encounter.");
                } 

                if(_encounterExecutionRepository.HasAlreadyActivatedEncounter(encounterExecution.TouristId))
                {
                    throw new InvalidOperationException("Tourist has already activated an encounter.");
                }

                encounterExecution.NumberOfActiveTourists = CountActiveExecutions(encounterExecution.EncounterId, GetPaged(0,0).Value.Results) + 1;
                var newEncounterExecution = MapToDomain(encounterExecution);
                var creationResult = _encounterExecutionRepository.Create(newEncounterExecution);

                return creationResult.IsSuccess ? Result.Ok() : Result.Fail(creationResult.Errors);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371000; // Earth radius in meters
            var dLat = (lat2 - lat1) * (Math.PI / 180);
            var dLon = (lon2 - lon1) * (Math.PI / 180);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance in meters
        }
        private bool IsWithinRange(double touristLat, double touristLon, double encounterLat, double encounterLon, double activateRange)
        {
            return CalculateDistance(touristLat, touristLon, encounterLat, encounterLon) <= activateRange;
        }
        private int CountActiveExecutions(int encounterId, List<EncounterExecutionDto> allExecutions)
        {
            return allExecutions
                .Count(e => e.EncounterId == encounterId && e.Status == EncounterExecutionStatus.ACTIVATED);
        }
        private List<EncounterExecutionDto> GetActiveExecutionsByEncounter(int encounterId)
        {
            List<EncounterExecutionDto> all = GetPaged(0, 0).Value.Results;
            return all.Where(e => e.EncounterId == encounterId && e.Status == EncounterExecutionStatus.ACTIVATED).ToList();
        }

        public EncounterExecutionDto UpdateTouristLocation(EncounterExecutionDto updatedExecution, int numberOfPeople)
        {
            Encounter encounter = _encounterRepository.GetById(updatedExecution.EncounterId);

            var updateLocation = _encounterExecutionRepository.Get(updatedExecution.Id);
            updateLocation.UpdateLongitudeLatitude(updatedExecution.TouristLongitude, updatedExecution.TouristLatitude);
            _encounterExecutionRepository.Update(updateLocation);

            
                var activeTourists = CountActiveExecutions(updatedExecution.EncounterId, GetPaged(0, 0).Value.Results);

                var isCompleted = CheckIfCompleted(updatedExecution, numberOfPeople, activeTourists);
                if (isCompleted)
                {
                    var activeExecutions = GetActiveExecutionsByEncounter(updatedExecution.EncounterId);

                    updatedExecution.Status = EncounterExecutionStatus.COMPLETED;
                    updatedExecution.NumberOfActiveTourists = activeTourists;
                    foreach (var domainExecution in activeExecutions.Select(execution => _encounterExecutionRepository.Get(execution.Id)))
                    {
                        domainExecution.CompleteEncounter(activeTourists);
                        _encounterExecutionRepository.Update(domainExecution);
                    }
                }
                else
                {
                    var domainExecution = _encounterExecutionRepository.Get(updatedExecution.Id);
                    domainExecution.SetNumberOfActiveTourists(activeTourists);
                    _encounterExecutionRepository.Update(domainExecution);
                    updatedExecution.NumberOfActiveTourists = activeTourists;
                }
            
            
            return updatedExecution;
        }

        public EncounterExecutionDto LeaveEncounter(EncounterExecutionDto updatedExecution)
        {
            var abandonedEncounter = _encounterExecutionRepository.Get(updatedExecution.Id);
            abandonedEncounter.AbandonEncounter();
            _encounterExecutionRepository.Update(abandonedEncounter);
            updatedExecution.Status = EncounterExecutionStatus.ABANDONED;
            return updatedExecution;
        }

        private bool CheckIfCompleted(EncounterExecutionDto encounterExecution, int peopleNumb, int activeTourists)
        {
            //int activeParticipants = CountActiveExecutions(encounterExecution.EncounterId, GetPaged(0, 0).Value.Results);

            return activeTourists >= peopleNumb;
        }

        public Result<bool> TouristCompletedEncounterForTour(int touristId, int encounterId)
        {
           
             bool completed = _encounterExecutionRepository.TouristCompletedEncounterForTour(touristId, encounterId);
             return completed;
            
            
        }
    }
}
