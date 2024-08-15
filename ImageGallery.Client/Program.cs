using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(configure =>
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear(); // New JWT handler for .net 8

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ImageGalleryAPIRoot"]);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) // Congigures the cookie handler. Once an identity token is validate and transformed into a claims identity, it will be stored in a cookie.
                                                                // The cookie is used on subsequent requests to the web app to check whether or not we are making an authenticated request. 
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // this registers and configures the OIDC handler. This handles create auth/token/idenity token validation requestions.
    options.Authority = "https://localhost:5001"; // This is the address of our identity provider. The middleware uses this value to read the meta data on the discovery endpoint so it can find the different endpoints in the IDP.
    options.ClientId = "imagegalleryclient"; // Will match the clientID on the IDP. Handled via back channel or server-to-server communication. 
    options.ClientSecret = "secret"; // Also matches the secret in the IDP. The combo of secret and id ensures our client can do an authenticated call to the token endpoint.
    options.ResponseType = "code"; // This corresponds to a grant time or code flow. Requires PKCE protection. Middleware automatically enables this when code is the response type.
    //options.Scope.Add("openid"); // Already added by default.
    //options.Scope.Add("profile"); // Already added by defuault. 
    //options.CallbackPath = new PathString("signin-oidc"); // Use this to override or change the redirect URI.
    // SignedoutCallbackPath: default = host:port/signout-callback-oidc
    // Must match with the post logout redirect URI at the IDP client config.
    // If you want to autmoatically return to the application after logging out.
    // To change:
    // options.SignedOutCallbackPath = "whateverstringyouwant";
    options.SaveTokens = true; // Allows middleware to save tokens from the IDP so they can be used inside the client.
    options.GetClaimsFromUserInfoEndpoint = true; // Allows us to get additional claims from the UserInfoendpoint.
    options.ClaimActions.Remove("aud");  // This allows us to add, change, remove claim filters - OIDC Options removed some claims by default. This is to limit the size of the JWT. You can override these filters.
    options.ClaimActions.DeleteClaim("sid");
    options.ClaimActions.DeleteClaim("idp"); // https://github.com/dotnet/aspnetcore/blob/v8.0.1/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectOptions.com
    options.Scope.Add("roles");
    options.ClaimActions.MapJsonKey("role", "role"); // have to create the map to allow custom claim to be used by the client
    options.TokenValidationParameters = new() //contains configuration values on how the toke validation should happen, but it also allows us to specify the name claim and role claim types.
    {
        NameClaimType = "given_name",
        RoleClaimType = "role"
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensures all endpoints can be protected by authentcation. Goes after UseRouting (to allow calls to be routed to the endpoints), but will block any calls that are not Authenticated.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();
