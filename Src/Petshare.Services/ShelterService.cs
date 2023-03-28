using Mapster;
using Petshare.CrossCutting.DTO.Shelter;
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


        public async Task<List<ShelterResponse>> GetAll()
        {
            var shelters = await _repositoryWrapper.ShelterRepository.FindAll();

            return shelters.Adapt<List<ShelterResponse>>();
        }

        public async Task<ShelterResponse?> GetById(Guid id)
        {
            var shelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(s => s.ID == id)).SingleOrDefault();

            return shelter?.Adapt<ShelterResponse>();
        }

        public async Task<ShelterResponse> Create(PostShelterRequest shelter)
        {
            var shelterToCreate = shelter.Adapt<Shelter>();
            shelterToCreate.ID = Guid.NewGuid();
            shelterToCreate.IsAuthorized = false;

            var createdShelter = await _repositoryWrapper.ShelterRepository.Create(shelterToCreate);
            await _repositoryWrapper.Save();

            return createdShelter.Adapt<ShelterResponse>();
        }

        public async Task<bool> Update(Guid id, PutShelterRequest shelter)
        {
            var shelterToUpdate = (await _repositoryWrapper.ShelterRepository.FindByCondition(s => s.ID == id))
                .SingleOrDefault();

            if (shelterToUpdate is null)
                return false;

            shelterToUpdate = shelter.Adapt(shelterToUpdate);
            await _repositoryWrapper.ShelterRepository.Update(shelterToUpdate);
            await _repositoryWrapper.Save();

            return true;
        }
    }
}