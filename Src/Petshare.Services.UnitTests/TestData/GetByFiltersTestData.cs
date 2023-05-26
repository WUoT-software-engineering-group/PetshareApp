using Petshare.CrossCutting.DTO.Announcement;
using Petshare.Domain.Entities;
using System.Collections;

namespace Petshare.Services.UnitTests.TestData
{
    public class GetByFiltersTestData : IEnumerable<object[]>
    {
        private static readonly List<Guid> _guids = new()
        {
            Guid.Parse("8bea3bb9-c99a-4c12-94d8-ffac5e89bd33"),
            Guid.Parse("68851f74-a311-40f3-9f8e-329c6a6a80f1"),
            Guid.Parse("3a3fa4d3-1ae0-44d6-92c1-92a4172b43fa"),
        };

        public static List<Announcement> RepositoryData => new()
        {
            new()
            {
                ID = _guids[0],
                Pet = new()
                {
                    Species = "pies",
                    Breed = "rasowy",
                    Shelter = new()
                    {
                        FullShelterName = "Psiakowy Raj",
                        Address = new() { City = "Warszawa" }
                    },
                    Birthday = DateTime.Parse("01-01-2011")
                },
                LikedBy = new ()
            },
            new()
            {
                ID = _guids[1],
                Pet = new()
                {
                    Species = "kot",
                    Breed = "rasowy",
                    Shelter = new()
                    {
                        FullShelterName = "Psiakowy Raj",
                        Address = new() { City = "Lublin" }
                    },
                    Birthday = DateTime.Parse("01-01-2013")
                },
                LikedBy = new ()
            },
            new()
            {
                ID = _guids[2],
                Pet = new()
                {
                    Species = "słoń",
                    Breed = "elegancki",
                    Shelter = new()
                    {
                        FullShelterName = "Safari",
                        Address = new() { City = "Płock" }
                    },
                    Birthday = DateTime.Parse("01-01-2018")
                },
                LikedBy = new ()
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            // 1
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Species = new(),
                    Breeds = new() { "rasowy", "elegancki" },
                    Cities = new() { "Warszawa" },
                    ShelterNames = new() { "Psiakowy Raj" }
                },
                new List<Guid>
                {
                    _guids[0],
                }
            };

            // 2
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Species = new() { "kot" },
                    Breeds = new(),
                    Cities = new(),
                    ShelterNames = new()
                },
                new List<Guid>
                {
                    _guids[1],
                }
            };

            // 3
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Species = new() { "kot", "słoń" },
                    Breeds = new(),
                    Cities = new(),
                    ShelterNames = new()
                },
                new List<Guid>
                {
                    _guids[1],
                    _guids[2],
                }
            };

            // 4
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Species = new(),
                    Breeds = new() { "rasowy", "elegancki" },
                    Cities = new(),
                    ShelterNames = new()
                },
                new List<Guid>
                {
                    _guids[0],
                    _guids[1],
                    _guids[2],
                }
            };

            // 5
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Species = new(),
                    Breeds = new(),
                    Cities = new() { "Warszawa" },
                    ShelterNames = new()
                },
                new List<Guid>
                {
                    _guids[0],
                }
            };

            // 6
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Species = new(),
                    Breeds = new(),
                    Cities = new() { "Warszawa" },
                    ShelterNames = new() { "Psiakowy Raj" }
                },
                new List<Guid>
                {
                    _guids[0],
                }
            };

            // 7
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Species = new(),
                    Breeds = new(),
                    Cities = new(),
                    ShelterNames = new(),
                    MinAge = 11
                },
                new List<Guid>
                {
                    _guids[0],
                }
            };

            // 8
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Species = new(),
                    Breeds = new(),
                    Cities = new(),
                    ShelterNames = new(),
                    MinAge = 4,
                    MaxAge = 6
                },
                new List<Guid>
                {
                    _guids[2],
                }
            };

            // 9
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Species = new(),
                    Breeds = new(),
                    Cities = new(),
                    ShelterNames = new(),
                },
                _guids
            };

            // 10
            yield return new object[]
            {
                new GetAnnouncementsRequest(),
                _guids
            };

            // 11
            yield return new object[]
            {
                new GetAnnouncementsRequest()
                {
                    Breeds = new() { "rasowy", "pospolity" },
                    ShelterNames = new() { "Psiakowy Raj", "Koci Raj", "Safari" },
                    MinAge = 6,
                    MaxAge = 13
                },
                new List<Guid>
                {
                    _guids[0],
                    _guids[1],
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
