// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace FakePing.IDP;

public static class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hoover Way",
                locality = "Hooversville",
                postal_code = "69118",
                country = "The Country of Atlanta"
            };

            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Sean",
                    Password = "Sean",
                    Claims =
                    {
                        new Claim("role", "Rodanator"),
                        new Claim(JwtClaimTypes.Name, "Sean Rod"),
                        new Claim(JwtClaimTypes.GivenName, "Sean"),
                        new Claim(JwtClaimTypes.FamilyName, "Rod"),
                        new Claim(JwtClaimTypes.Email, "SeanRod@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "Jeff",
                    Password = "Jeff",
                    Claims =
                    {
                        new Claim("role", "Hoovernator"),
                        new Claim(JwtClaimTypes.Name, "J Hoovers"),
                        new Claim(JwtClaimTypes.GivenName, "Jeff"),
                        new Claim(JwtClaimTypes.FamilyName, "Hoovers"),
                        new Claim(JwtClaimTypes.Email, "joovers@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        //new Claim(JwtClaimTypes.WebSite, "http://nohello.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }
    }
}