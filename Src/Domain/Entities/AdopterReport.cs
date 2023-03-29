using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petshare.Domain.Entities;

public class AdopterReport : Report
{
    public Guid AdopterID { get; set; }
}
