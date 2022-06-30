using System;

namespace Kztek.Data.LockerEvent.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        KztekLockerEventEntities Get();
    }
}