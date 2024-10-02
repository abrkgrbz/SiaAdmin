using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using SiaAdmin.Application.Interfaces.Firebase.Models;

namespace SiaAdmin.Application.Interfaces.Firebase
{
    public interface IFirebaseService
    {

        public Task<string> CreateCustomTokenAsync(string userId); 
        public Task<FirebaseAuthResponse> SignInWithCustomTokenAsync(string token, bool returnSecureToken); 
        public Task<FirebaseToken> VerifyIdTokenAsync(string idToken); 
        public Task<UserImportResult> ImportUsersAsync(List<ImportUserRecordArgs> users); 
        public Task<FirebaseToken> DecodeIdTokenAsync(string idToken); 
        public Task<UserRecord> GetUserWithIdAsync(string uid); 
        public Task<RefreshToken> GetTokenWithRefreshToken(string refreshToken);
        public Task<dynamic> SendMessagesToMultipleDevice(List<string> tokenList,string title,string body,Dictionary<string,string> data);


    }
}
