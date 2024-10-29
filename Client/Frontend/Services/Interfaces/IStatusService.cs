namespace Frontend.Services.Interfaces;

public interface IStatusService
{
  public Task<(bool isOkay, string message, string statusCode)> GetStatusAsync();
}