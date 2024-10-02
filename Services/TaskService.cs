using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TaskService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public TaskService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }
    public async Task<List<TaskItem>> GetTasksAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _httpClient.GetAsync("http://localhost:5223/api/tasks");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            // Console.WriteLine($"Resposta JSON: {jsonString}");

            using (var jsonDoc = JsonDocument.Parse(jsonString))
            {
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("$values", out JsonElement valuesElement))
                {
                    var taskList = JsonSerializer.Deserialize<List<TaskItem>>(valuesElement.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    // Log para verificar se as tarefas estão sendo desserializadas corretamente
                    // Console.WriteLine($"Tarefas desserializadas: {taskList.Count}");
                    foreach (var task in taskList)
                    {
                        //Console.WriteLine($"Tarefa: {task.Title}, Descrição: {task.Description}, IsCompleted: {task.IsCompleted}");
                    }
                    return taskList;
                }
            }
        }
        return new List<TaskItem>(); // Retorna uma lista vazia caso a resposta não seja válida
    }

    // Adiciona o método AddTaskAsync
    public async Task AddTaskAsync(TaskItemDto taskItemDto)
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Enviando o objeto TaskItemDto para a API
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5223/api/tasks", taskItemDto);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Erro ao adicionar a tarefa.");
        }
    }

   public async Task UpdateTaskAsync(int taskId, TaskItemDto taskItemDto)
{
    var token = await _localStorage.GetItemAsync<string>("authToken");
    if (!string.IsNullOrEmpty(token))
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    var response = await _httpClient.PutAsJsonAsync($"http://localhost:5223/api/tasks/{taskId}", taskItemDto);

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception("Erro ao atualizar a tarefa.");
    }
}


    public async Task DeleteTaskAsync(int id)
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _httpClient.DeleteAsync($"http://localhost:5223/api/tasks/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Erro ao deletar a tarefa.");
        }
    }


}

public class TaskItemDto
{
    public int Id { get; set; }  // Torna a propriedade Id pública
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}





