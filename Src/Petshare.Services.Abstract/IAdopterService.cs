﻿using Petshare.CrossCutting.DTO;
using Petshare.CrossCutting.DTO.Adopter;
using Petshare.CrossCutting.Utils;

namespace Petshare.Services.Abstract;

public interface IAdopterService
{
    Task<ServiceResponse> GetAll(PagingRequest pagingRequest);

    Task<ServiceResponse> GetById(Guid id);

    Task<ServiceResponse> Create(PostAdopterRequest adopterRequest);

    Task<ServiceResponse> UpdateStatus(Guid id, PutAdopterRequest adopter);

    Task<ServiceResponse> VerifyAdopterForShelter(Guid adopterId, Guid shelterId);

    Task<ServiceResponse> CheckIfAdopterIsVerified(Guid adopterId, Guid shelterId);
}