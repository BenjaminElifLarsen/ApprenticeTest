//using CustomerFrontEnd.Services.Contracts;
//using System.Security.Claims;
//using System.Text.Json;

//namespace CustomerFrontEnd.Services;

//public class CustomAuthStateProvider : AuthenticationStateProvider
//{
//    private readonly IAuthenticationStorage _authenticationStorage;

//    public CustomAuthStateProvider(IAuthenticationStorage authenticationStorage)
//    {
//        _authenticationStorage = authenticationStorage;        
//    }

//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        //string token = await _authenticationStorage.GetTokenAsync();
//        string token = _authenticationStorage.Token;

//        var identify = new ClaimsIdentity(token);
        
//        if(!string.IsNullOrWhiteSpace(token))
//        {
//            var payload = token.Split('.')[1];
//            var bytes = HandleMissingPadding(payload);
//            var keyValues = JsonSerializer.Deserialize<Dictionary<string, object>>(bytes)!;
//            identify = new ClaimsIdentity(keyValues.Select(x => new Claim(x.Key, x.Value.ToString()!)));
//        }
//        var user = new ClaimsPrincipal(identify);
//        var state = new AuthenticationState(user);
//        NotifyAuthenticationStateChanged(Task.FromResult(state));
//        return state;
//    }

//    private static byte[] HandleMissingPadding(string base64)
//    {
//        switch (base64.Length % 4)
//        {
//            case 2: base64 += "=="; break;
//            case 3: base64 += "="; break;
//        }
//        return Convert.FromBase64String(base64);
//    }
//}
