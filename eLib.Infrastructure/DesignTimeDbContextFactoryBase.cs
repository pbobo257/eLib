﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eLib.Infrastructure
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            return Create(Directory.GetCurrentDirectory(), Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }
        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);
        public TContext Create()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = AppContext.BaseDirectory;
            return Create(basePath, environmentName);
        }
        TContext Create(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            var config = builder.Build();
            var connstr = config.GetConnectionString("default");
            if (string.IsNullOrWhiteSpace(connstr) == true)
                throw new InvalidOperationException("Could not find a connection string named 'default'.");
            return Create(connstr);
        }
        TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            Console.WriteLine($"MyDesignTimeDbContextFactory.Create(string): Connection string: {connectionString}");
            optionsBuilder.UseSqlServer(connectionString, s => s.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            DbContextOptions<TContext> options = optionsBuilder.Options;
            return CreateNewInstance(options);
        }
    }
}
