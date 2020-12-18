using System;

namespace Bathhouse.Memory
{
    public class OfficeRepository : IOfficeRepository
    {
        public Office GetById(Guid id) => new Office() { Id = id };

    }
}