using BankingSystem.ApplicationLogic.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.EFDataAccess
{
    public class BaseRepository<T>: IRepository<T>
    {
        public T Add(T itemToAdd)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T itemToDelete)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T Update(T itemToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
