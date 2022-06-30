using System;

namespace Kztek.Data.AccessEvent.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        KztekAccessEventEntities Get();
    }
}