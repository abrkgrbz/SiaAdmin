namespace SiaAdmin.WebUI.Models
{
    public class SiaUserProfileViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public int Active { get; set; }
        public int LoginCount { get; set; }
        public string Msisdn { get; set; }
        public string Email { get; set; }
        public string LastIP { get; set; }
        public int ProfilPuani { get; set; }
    }
}
