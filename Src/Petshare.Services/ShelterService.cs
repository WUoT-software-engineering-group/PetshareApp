using Mapster;
using Petshare.CrossCutting.DTO;
using Petshare.CrossCutting.DTO.Shelter;
using Petshare.CrossCutting.Utils;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;

namespace Petshare.Services
{
    public class ShelterService : IShelterService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ShelterService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ServiceResponse> Create(PostShelterRequest shelter)
        {
            var shelterToCreate = shelter.Adapt<Shelter>();

            var createdShelter = await _repositoryWrapper.ShelterRepository.Create(shelterToCreate);
            await _repositoryWrapper.Save();

            return ServiceResponse.Created(createdShelter.ID);
        }

        public async Task<ServiceResponse> Update(Guid id, PutShelterRequest shelter)
        {
            var shelterToUpdate = (await _repositoryWrapper.ShelterRepository.FindByCondition(s => s.ID == id))
                .SingleOrDefault();

            if (shelterToUpdate is null)
                return ServiceResponse.BadRequest();

            shelterToUpdate = shelter.Adapt(shelterToUpdate);
            await _repositoryWrapper.ShelterRepository.Update(shelterToUpdate);
            await _repositoryWrapper.Save();

            return ServiceResponse.Ok();
        }

        public async Task<ServiceResponse> GetAll(PagingRequest pagingRequest)
        {
            var shelters = await _repositoryWrapper.ShelterRepository.FindAll();

            return ServiceResponse.Ok(new PagedShelterResponse
            {
                Shelters = shelters.Skip(pagingRequest.PageNumber * pagingRequest.PageCount).Take(pagingRequest.PageCount).Adapt<List<ShelterResponse>>(),
                PageNumber = pagingRequest.PageNumber,
                Count = shelters.Count()
            });
        }

        public async Task<ServiceResponse> GetById(Guid id)
        {
            var shelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(s => s.ID == id)).SingleOrDefault();

            return shelter is not null
                ? ServiceResponse.Ok(shelter.Adapt<ShelterResponse>())
                : ServiceResponse.NotFound();
        }
    }
}