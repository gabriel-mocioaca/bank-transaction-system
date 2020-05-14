using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.ApplicationLogic.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string id) : base($"Entity with id {id} was not found")
        {
        }
    }
}
