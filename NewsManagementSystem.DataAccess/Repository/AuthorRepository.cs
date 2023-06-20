using NewsManagementSystem.DataAccess.Data;
using NewsManagementSystem.DataAccess.Repository.IRepository;
using NewsManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {

        private ApplicationDbContext _db;
        public AuthorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Author author)
        {
            _db.Authors.Update(author);
        }
    }
}
