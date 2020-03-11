using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class ReplaceOperation : StringOperation
    {
        public string Name => "Replace";

        public StringArgs Args { get; set; }

        public StringProcesor Processor => _replace;

        private string _replace(string origin)
        {
            var myArgs = Args as ReplaceArgs;
            var from = myArgs.From;
            var to = myArgs.To;

            string result = origin.Replace(from, to);

            return result;
        }

        public StringOperation Clone()
        {
            return new ReplaceOperation()
            {
                Args = new ReplaceArgs()
            };
        }

        public void ShowEditDialog()
        {
            var screen = new ReplaceDialog(Args as ReplaceArgs);

            if (screen.ShowDialog() == true)
            {
                var myArgs = Args as ReplaceArgs;
                myArgs.From = screen.From;
                myArgs.To = screen.To;
            }
        }
    }
}
