using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DataAccess.Data;
using NewsManagementSystem.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;


namespace NewsManagementSystem.DataAccess.DbInitialzer
{
    public class DbInitialzer : IDbInitialzer
    {
        private readonly ApplicationDbContext _db;

        public DbInitialzer(
            ApplicationDbContext db
            )
        {
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch { }
            if (_db.Users.Find(1)==null)
            {

                string password = "admin";
                string salt = BCryptNet.GenerateSalt();
                string hashedPassword = BCryptNet.HashPassword(password, salt);
                var user = new User { UserName = "admin", Password = hashedPassword };
                _db.Users.Add(user);
                _db.SaveChanges();
            }

            return;
        }
    }
}
