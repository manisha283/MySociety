using Microsoft.Extensions.Configuration;

namespace MySociety.Service.Configuration;

public class JwtConfig
{
     public static string Key { get; set; } = "ede48b7cbd1d9573b132ba4ab44cae19b2b837591f1f8579e4046223d3b99794b99d615402faa79ca5c989846afe0fb6ad3b17f97a2ecfc0f79baa80acc155fa31f51ee8509af1f067b96a1336aa7e06ad11440f23f4b13826545c6f9f27dca3621a3f26186beca4b608aa342af94eabc145bbb740ed13fd49644f55c9d7a3f7fb5d15a0af71ac83401897a109f7e6f134deb3bc2a2241bf189506aa0efc7fbb9afc2227706523d384de961a034cf3be856f028e150e50791114cee9b80e75d668300d451e8d43724c5955adc7cd032d69e8933710944d042466cfc2ad8e574d9558ee6a88b15c11617b0e140868cdf845866bca2bc244ae4e63aaf9d2e502c0";
    public static string Issuer { get; set; } = "localhost";
    public static string Audience { get; set; } = "localhost";
    public static int TokenDuration { get; set; } = 24;

    public static void LoadJwtConfiguration(IConfiguration configuration)
    {
        Key = configuration["JwtConfig:Key"] ?? Key;
        Issuer = configuration["JwtConfig:Issuer"] ?? Issuer;
        Audience = configuration["JwtConfig:Audience"] ?? Audience;

        if (int.TryParse(configuration["JwtConfig:TokenDuration"], out int duration))
        {
            TokenDuration = duration;
        }
    }
}
