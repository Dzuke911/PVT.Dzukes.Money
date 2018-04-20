using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DocumentationAttribute : Attribute
    {
        public DocumentationAttribute(string message)
        {
            this.message = message;
        }

        public string message { get; set; }
    }
}
