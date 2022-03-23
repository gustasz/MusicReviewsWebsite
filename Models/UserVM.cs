namespace MusicReviewsWebsite.Models
{
    public class UserVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<RoleInfo> Roles { get; set; }
    }

    public class RoleInfo
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
