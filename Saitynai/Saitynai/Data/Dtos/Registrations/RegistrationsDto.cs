namespace Saitynai.Data.Dtos.Registrations;

public record RegistrationDto(int id, string CarNo, string Manufacturer, string Model);
public record CreateRegistrationDto(string CarNo, string Manufacturer, string Model);
public record UpdateRegistrationDto(string CarNo, string Manufacturer, string Model);


