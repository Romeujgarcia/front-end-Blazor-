using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class AuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

public async Task<string> LoginAsync(LoginModel loginModel)
{
    try
    {
        var url = "api/users/login"; // URL corrigida
        Console.WriteLine($"Chamando a URL: {url} com os dados: {JsonSerializer.Serialize(loginModel)}");

        var response = await _httpClient.PostAsJsonAsync(url, loginModel);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        return result.Token;
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Erro ao fazer login: {ex.Message}");
        throw;
    }
}



    public async Task<bool> RegisterAsync(RegisterModel registerModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Users/register", registerModel);
        return response.IsSuccessStatusCode;
    }
}

public class AuthResponse
{
    public string? Token { get; set; }
}
