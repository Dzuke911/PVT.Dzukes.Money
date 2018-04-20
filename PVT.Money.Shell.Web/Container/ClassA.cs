using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Container
{
    // just to test container
    public class ClassA
    {
        public ClassB B{get; set;}

        public ClassA( ClassB B)
        {
            this.B = B;
        }
    }
}
