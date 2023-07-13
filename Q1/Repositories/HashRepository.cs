using Q1.Models;
using Q1.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Hosting.Server;

namespace Q1.Repositories
{
    public class HashRepository
    {
        private readonly DataContext _context;

        public HashRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Hash hash)
        {
            var serverName = "GEOFF\\SQLEXPRESS"; // Replace with your SQL Server instance name
            var databaseName = "master"; // Replace with your database name
            var username = "GEOFF\\lamka"; // Replace with your SQL Server username
            var password = ""; // Replace with your SQL Server password

            var connectionString = $"Server={serverName};Database={databaseName};User Id={username};Password={password};Encrypt = false;TrustServerCertificate = true;";
           
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            connectionString = builder.ToString();
    
  
            _context.Hashes.Add(hash);
            _context.SaveChanges();
        }

        public List<HashCount> GetHashesGroupedByDate()
        {
            return _context.Hashes
                .GroupBy(h => h.DateCreated.Date)
                .Select(g => new HashCount { Date = g.Key, Count = g.Count() })
                .ToList();
        }
    }
}
