using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petshare.Domain.Entities;

public class ShelterReport : Report
{
    public Guid ShelterID { get; set; }
}