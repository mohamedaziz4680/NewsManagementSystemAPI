using NewsManagementSystem.DataAccess.Data;
using NewsManagementSystem.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Author = new AuthorRepository(_db);
            News = new NewsRepository(_db);
            User = new UserRepository(_db);
        }
        public IAuthorRepository Author { get; private set; }
        public INewsRepository News { get; private set; }
        public IUserRepository User { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
