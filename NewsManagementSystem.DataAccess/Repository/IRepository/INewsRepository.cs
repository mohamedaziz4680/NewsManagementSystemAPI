using NewsManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository.IRepository
{
    public interface INewsRepository : IRepository<News>
    {
        void Update(News news);

    }
}
