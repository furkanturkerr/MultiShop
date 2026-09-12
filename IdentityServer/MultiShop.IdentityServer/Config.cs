using Duende.IdentityServer.Models;

namespace MultiShop.IdentityServer;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
    {
        new ApiResource("ResourceCatalog")
        {
            Scopes = { "CatalogFullPermissions" }
        },
        new ApiResource("ResourceOrdering")
        {
            Scopes = { "OrderingFullPermissions" }
        },
        new ApiResource("ResourceDiscount")
        {
            Scopes = { "DiscountFullPermissions" }
        }
    };

    public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Email(),
        new IdentityResources.Profile()
    };

    public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
    {
        new ApiScope("CatalogFullPermissions", "Full authority for Catalog operations"),
        new ApiScope("OrderingFullPermissions", "Full authority for Ordering operations"),
        new ApiScope("DiscountFullPermissions", "Full authority for Discount operations")
    };

    public static IEnumerable<Client> Clients => new Client[]
    {
        new Client
        {
            ClientId = "MultiShopVisitor.Web",
            ClientName = "MultiShopVisitorUser",
            ClientSecrets = { new Secret("MultiShopVisitor.Web".Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "CatalogFullPermissions", "OrderingFullPermissions", "DiscountFullPermissions" }
        },

        new Client
        {
            ClientId = "MultiShopManager.Web",
            ClientName = "MultiShopManagerUser",
            ClientSecrets = { new Secret("MultiShopManager.Web".Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "CatalogFullPermissions", "OrderingFullPermissions", "DiscountFullPermissions" }
        },
        
        new Client
        {
            ClientId = "MultiShopAdmin.Web",
            ClientName = "MultiShopAdminUser",
            ClientSecrets = { new Secret("MultiShopAdmin.Web".Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = { "CatalogFullPermissions", "OrderingFullPermissions", "DiscountFullPermissions" } ,
            AccessTokenLifetime = 600
        }
    };
}