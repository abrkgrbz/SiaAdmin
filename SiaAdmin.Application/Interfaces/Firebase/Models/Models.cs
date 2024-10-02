 
namespace SiaAdmin.Application.Interfaces.Firebase.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
    public class LookupRequest
    {
        public string IdToken { get; set; }
    }

    public class PhoneRegistrationRequest
    {
        public string PhoneNumber { get; set; }  // format +1234567890 +90 - +1
        public string Token { get; set; }
    }

    public class PhoneVerificationRequest
    {
        public string SessionInfo { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SendVerificationCodeRequest
    {
        public string PhoneNumber { get; set; } = default;
    }

    public class VerifyCodeRequest
    {
        public string PhoneNumber { get; set; } = default;
        public string Code { get; set; } = default;
    }


    public class AuthenticationResponse
    {
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresIn { get; set; }
        public string LocalId { get; set; }
        public bool IsNewUser { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class FirebaseAuthResponse
    {
        public string IdToken { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresIn { get; set; }
        public string LocalId { get; set; }
    }


    public class GetAccountInfoResponse
    {
        public string Kind { get; set; }
        public List<User> Users { get; set; }
    }

    public class GetRefreshToken
    {
        public string RefreshToken { get; set; }
    }
    public class User
    {
        public string LocalId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailVerified { get; set; }
        public long PasswordUpdatedAt { get; set; }
        public List<ProviderUserInfo> ProviderUserInfo { get; set; }
        public string ValidSince { get; set; }
        public bool Disabled { get; set; }
        public string LastLoginAt { get; set; }
        public string CreatedAt { get; set; }
        public DateTime LastRefreshAt { get; set; }
    }

    public class ProviderUserInfo
    {
        public string ProviderId { get; set; }
        public string FederatedId { get; set; }
        public string Email { get; set; }
        public string RawId { get; set; }
    }

    public class RefreshToken
    {
        public string? access_token { get; set; }
        public string expires_in { get; set; }
        public string? token_type { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
        public string user_id { get; set; }
        public string project_id { get; set; }
    }
}
