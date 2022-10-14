namespace Saitynai.Data.Dtos.Competitions;


public record CompetitionDto(int id, string Name, string Description, string Rules);
public record CreateCompetitionDto(string Name, string Description, string Rules);
public record UpdateCompetitionDto(string Name, string Description, string Rules);
