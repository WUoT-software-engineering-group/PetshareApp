﻿namespace Petshare.Domain.Entities;

public class Shelter : User 
{
    public string FullShelterName { get; set; } = default!;

    public bool IsAuthorized { get; set; }

    public List<Pet> Pets { get; set; } = new List<Pet>();
}
