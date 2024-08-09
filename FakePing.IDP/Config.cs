using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System.Runtime.CompilerServices;

namespace FakePing.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            // List of identity-related resources. 
            // In this case, the OpenId is a user's specific identifier. 
            // Other resources that could be mapped could be things like first name, last name, etc.
            // This is the openid scope, and it must be used for user scopes
            new IdentityResources.OpenId(),
            new IdentityResources.Profile() // will return the given name and family name claims will be returned. This enables accross the entire applicaiton
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        // Maps to API scopes by requesting an api scope, a client can get access to an api.
        // These clients are defined below in clients.
        new ApiScope[]
            { };

    // Clients returns an  ienumerable of clients
    public static IEnumerable<Client> Clients =>
        // For each application, you will need to define at least one client.
        new Client[]
            { 
                new Client()
                { 
                    ClientName = "Image Gallery",
                    ClientId = "imagegalleryclient",
                    AllowedGrantTypes = GrantTypes.Code, // sets the allowed grant type to authorization code flow. This code is deliverted to the browser via URI redirection.
                    RedirectUris = // No way, here's that URI rediction.
                    { 
                        "https://localhost:7184/signin-oidc" // This is the host address of the client. The signin-oidc is Identity Servers default value, but it can be configured to a diff value in the web client.
                    },

                    // A client can only use scopes that have been defined as an identity resource above.
                    AllowedScopes =
                    { 
                        IdentityServerConstants.StandardScopes.OpenId, // Returns identity scopes.
                        IdentityServerConstants.StandardScopes.Profile // Returns profile scopes (ie familya names)
                    },

                    // This is for client authentication. This allows the client application to execute an authenticate call to the token endpoint.
                    ClientSecrets =
                    { 
                        new Secret("secret".Sha256())
                    }
                }
            };
}