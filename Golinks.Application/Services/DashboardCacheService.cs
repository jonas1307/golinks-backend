namespace Golinks.Application.Services;

public class DashboardCacheService
{
    private int _generation = 0;

    public string BuildKey(int days, Guid? linkId, bool excludeBots)
        => $"dashboard:v{_generation}:{days}:{linkId}:{excludeBots}";

    public void Invalidate() => Interlocked.Increment(ref _generation);
}
