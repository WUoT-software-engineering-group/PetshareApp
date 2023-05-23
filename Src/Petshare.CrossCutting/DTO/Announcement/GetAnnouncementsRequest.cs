namespace Petshare.CrossCutting.DTO.Announcement
{
    public class GetAnnouncementsRequest
    {
        public List<string> Species { get; set; } = new();
        public List<string> Breeds { get; set; } = new();
        public List<string> Cities { get; set; } = new();
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public List<string> ShelterNames { get; set; } = new();
        public bool? IsLiked { get; set; }
    }
}
