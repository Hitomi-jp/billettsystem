using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppExamPart2.Models;

namespace WebAppExamPart2.DAL
{
    public interface ILogInOutRepository
    {
        Task<bool> LoggInn(Bruker bruker);
    }
}
