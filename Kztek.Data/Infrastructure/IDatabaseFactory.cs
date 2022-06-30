using System;

namespace Kztek.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        KztekEntities Get();
    }
}