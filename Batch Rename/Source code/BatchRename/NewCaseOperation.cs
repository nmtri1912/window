using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class NewCaseOperation : StringOperation
    {
        public string Name => "NewCase";

        public StringProcesor Processor => _newcase;

        public StringArgs Args { get; set; }

        public StringOperation Clone()
        {
            return new NewCaseOperation()
            {
                Args = new NewCaseArgs()
            };
        }

        public void ShowEditDialog()
        {
            var screen = new NewCaseDialog(Args as NewCaseArgs);

            if (screen.ShowDialog() == true)
            {
                var myArgs = Args as NewCaseArgs;
                myArgs.From = screen.From;
            }
        }

        private string _newcase(string origin)
        {

            var myArgs = Args as NewCaseArgs;
            var needle = myArgs.From;
            string result = null;
            if (needle == "UpperCase")
            {
                result = origin.ToUpper();

            }
            else if (needle == "LowerCase")
            {
                result = origin.ToLower();
            }
            else
            {
                string[] tokens = origin.Split(new string[] { "\\" }, StringSplitOptions.None);
                string[] tokendots = tokens[tokens.Length - 1].Split(new string[] { "." }, StringSplitOptions.None);
                string extensions = "";

                if (tokendots.Length > 1)
                {
                    extensions = tokendots[tokendots.Length - 1];
                }

                tokendots[0] = tokendots[0].Trim();

                while (tokendots[0].IndexOf("  ") != -1)
                {
                    tokendots[0] = tokendots[0].Replace("  ", " ");
                }

                string[] chartokens = tokendots[0].Split(new string[] { " " }, StringSplitOptions.None);

                string StringFinal = null;

                if (needle == "Standard")
                {
                    foreach (string index in chartokens)
                    {
                        string firstchar = index.Substring(0, 1);
                        firstchar = firstchar.ToUpper();
                        string temp = index.Remove(0, 1);
                        temp = temp.ToLower();

                        StringFinal += firstchar + temp + " ";
                    }
                }

                for (int i = 0; i < tokens.Length - 1; ++i)
                {
                    result += tokens[i] + "\\";
                }

                result += StringFinal.Trim();
                if (extensions != "")
                {
                    result += "." + extensions;
                }
            }

            return result;
        }
    }
}
