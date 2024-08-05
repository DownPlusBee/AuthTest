using Duende.IdentityServer.Models;

namespace FakePing.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            // List of identity-related resources. 
            // In this case, the OpenId is a user's specicig identifier. 
            // Other resources that could be mapped could be things like first name, last name, etc.
            new IdentityResources.OpenId()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        // Maps to API scopes by requesting an api scope, a client can get access to an api.
        // These clients are defined below in clients.
        new ApiScope[]
            { };

    public static IEnumerable<Client> Clients =>
        // For each application, you will need to define at least one client.
        new Client[]
            { };
}