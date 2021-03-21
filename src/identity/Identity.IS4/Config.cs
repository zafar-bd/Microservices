// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity.IS4
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                 new[]
                 {
                new IdentityResources.OpenId(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResource("profile",
                     new[] { "role","email","phone_number", "user_name", "given_name"})
                 };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                 new ApiScope("api_gateway"),
                 new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<ApiResource> Apis =>
                        new[]
                        {
                              new ApiResource
                          {
                              Name = "api_gateway",
                              DisplayName = "API Gateway",
                              Description = "Allow the application to access API Gateway on your behalf",
                              Scopes = new [] {"api_gateway"},
                              UserClaims = new [] { "role", "email", "phone_number", "user_name", "given_name" }
                          },
                            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
                        };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "microservice_ui",
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedGrantTypes=  GrantTypes.Code,
                    AllowedCorsOrigins= { "https://localhost:44342" },
                    RedirectUris = { "https://localhost:44342/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:44342/authentication/logout-callback" },
                    Enabled=true,
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api_gateway",
                        IdentityServerConstants.LocalApi.ScopeName
                    }
                }
            };
    }
}
