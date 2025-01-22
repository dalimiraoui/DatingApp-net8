namespace API.DTOs;

public class CreateMessageDTO
{
    public required string RecipientUsername { get; set; }
    public required string Content { get; set; }

}
