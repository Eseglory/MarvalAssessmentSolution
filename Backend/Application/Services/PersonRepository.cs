using Application.Services.Interface;
using Common.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(DataContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
