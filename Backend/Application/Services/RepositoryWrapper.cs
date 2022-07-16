using Application.Service;
using Application.Services.Interface;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DataContext _repoContext;
        private IPersonRepository _person;
        public IPersonRepository Person
        {
            get
            {
                if (_person == null)
                {
                    _person = new PersonRepository(_repoContext);
                }
                return _person;
            }
        }
        public RepositoryWrapper(DataContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
        public async Task Save() => await _repoContext.SaveChangesAsync();

    }
}
