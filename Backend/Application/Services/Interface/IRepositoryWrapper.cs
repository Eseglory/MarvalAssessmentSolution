using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IRepositoryWrapper
    {
        IPersonRepository Person { get; }
        Task Save();
    }
}
