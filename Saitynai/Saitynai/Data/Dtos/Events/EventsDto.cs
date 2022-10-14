namespace Saitynai.Data.Dtos.Events;

public record EventDto(int id, string Name, DateTime Date, string Description);
public record CreateEventDto(string Name, DateTime Date, string Description);
public record UpdateEventDto(string Name, DateTime Date, string Description);



