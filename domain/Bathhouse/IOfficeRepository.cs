using System;

namespace Bathhouse
{
    public interface IOfficeRepository
    {
        Office GetById(Guid id);
    }
}