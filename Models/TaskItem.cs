using Microsoft.AspNetCore.Identity;

public class TaskItem
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    
    // Chave estrangeira que se relaciona com o usuário
    public string? UserId { get; set; }
    public virtual ApplicationUser? User { get; set; } // Propriedade de navegação
}



