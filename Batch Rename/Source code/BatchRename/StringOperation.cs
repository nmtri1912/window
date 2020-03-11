using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public delegate string StringProcesor(string origin);

    public interface StringOperation
    {
        string Name { get; }
        StringProcesor Processor { get; }
        StringArgs Args { get; set; }

        StringOperation Clone();

        void ShowEditDialog();
    }
}
