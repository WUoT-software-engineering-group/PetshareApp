using Mapster;
using Petshare.CrossCutting.DTO;
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


        public async Task<List<ShelterDTO>> GetAll()
        {
            var shelters = await _repositoryWrapper.ShelterRepository.FindAll();

            return shelters.Adapt<List<ShelterDTO>>();
        }

        public async Task<ShelterDTO?> GetById(Guid id)
        {
            var shelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(s => s.ID == id)).SingleOrDefault();

            return shelter?.Adapt<ShelterDTO>();
        }

        public async Task<ShelterDTO> Create(ShelterDTO shelter)
        {
            var shelterToCreate = shelter.Adapt<Shelter>();
            shelterToCreate.ID = Guid.NewGuid();
            shelterToCreate.IsAuthorized = false;

            var createdShelter = await _repositoryWrapper.ShelterRepository.Create(shelterToCreate);
            await _repositoryWrapper.Save();

            return createdShelter.Adapt<ShelterDTO>();
        }

        public async Task<bool> Update(ShelterDTO shelter)
        {
            var shelterToUpdate = (await _repositoryWrapper.ShelterRepository.FindByCondition(s => s.ID == shelter.ID))
                .SingleOrDefault();

            if (shelterToUpdate == null)
                return false;

            shelterToUpdate = shelter.Adapt(shelterToUpdate);
            await _repositoryWrapper.ShelterRepository.Update(shelterToUpdate);
            await _repositoryWrapper.Save();

            return true;
        }
    }
}