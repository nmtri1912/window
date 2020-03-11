using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BatchRename
{
    public class UniqueNameOperation : StringOperation
    {
        public string Name => "GUID";

        public StringArgs Args { get; set; }

        public StringProcesor Processor => _guid;

        private string _guid(string origin)
        {
            string[] tokens = origin.Split(new string[] { "\\" }, StringSplitOptions.None);
            string[] tokendots = tokens[tokens.Length - 1].Split(new string[] { "." }, StringSplitOptions.None);
            string extensions = "" ;
            if (tokendots.Length > 1)
            {
                extensions = tokendots[tokendots.Length - 1];
            }
            string result = null;
            for (int i = 0; i < tokens.Length - 1; i++)
            {
                result += tokens[i] + "\\";
            }
            result += Guid.NewGuid().ToString();
            if (extensions != "")
            {
                result +=  "." + extensions;
            }
           
            return result;
        }

        public StringOperation Clone()
        {
            return new UniqueNameOperation()
            {
                Args = new UniqueNameArgs()
            };
        }

        public void ShowEditDialog()
        {
            MessageBox.Show("Khong the Edit");
        }
    }
}