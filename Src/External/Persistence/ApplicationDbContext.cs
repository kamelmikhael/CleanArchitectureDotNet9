﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}