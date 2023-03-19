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

        public async Task<ShelterDTO?> GetById(string id)
        {
            if (!Guid.TryParse(id, out var shelterId))
                return null;

            var shelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(s => s.ID == shelterId)).SingleOrDefault();

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

        public async Task<bool> Update(string id, ShelterDTO shelter)
        {
            if (!Guid.TryParse(id, out var shelterId) || shelterId != shelter.ID)
                return false;

            var shelterToUpdate = (await _repositoryWrapper.ShelterRepository.FindByCondition(s => s.ID == shelterId))
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