﻿using Bathhouse.EF.InMemory;
using Bathhouse.EF.Repositories;
using Bathhouse.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bathhouse.EF.Test
{
  public class RepositoryTest : IClassFixture<SharedBathhouseDbFixture>
  {
    public RepositoryTest(SharedBathhouseDbFixture fixture) => Fixture = fixture;

    public SharedBathhouseDbFixture Fixture { get; }

    [Fact]
    public void Add_New_Entity()
    {
      //var repository = new Repository<Employee, Guid>(Fixture.CreateContext());
    }
  }
}
