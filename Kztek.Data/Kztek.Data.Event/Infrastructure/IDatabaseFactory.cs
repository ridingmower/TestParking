using System;

namespace Kztek.Data.Event.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        KztekEventEntities Get();
    }
}