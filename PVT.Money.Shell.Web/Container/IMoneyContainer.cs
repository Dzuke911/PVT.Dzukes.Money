using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Container
{
    public interface IMoneyContainer
    {
        void Add<T>();
        T Create<T>();
    }
}
