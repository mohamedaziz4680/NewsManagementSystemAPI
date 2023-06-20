using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {

        IAuthorRepository Author { get; }
        INewsRepository News { get; }
        IUserRepository User { get; }
        void Save();
    }
}
