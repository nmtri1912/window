using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{

    class FullnameNormalizeOperation : StringOperation
    {
        public string Name => "Fullname Normalize";

        public StringProcesor Processor => _normalize;


        private string _normalize(string origin)
        {
            var myArgs = Args as FullnameNormalizeArgs;
            var from = myArgs.From;

            string result = null;


            string[] tokens = origin.Split(new string[] { "\\" }, StringSplitOptions.None);
            string[] tokendots = tokens[tokens.Length - 1].Split(new string[] { "." }, StringSplitOptions.None);
            string extensions = "";
            if (tokendots.Length > 1)
            {
                extensions = tokendots[tokendots.Length - 1];
            }

            string StringFinal = null;

            if (from == "NoneSpace")
            {
                tokendots[0] = tokendots[0].Replace(" ", "");
                StringFinal = tokendots[0];
            }


            while (tokendots[0].IndexOf("  ") != -1)
            {
                tokendots[0] = tokendots[0].Replace("  ", " ");
            }

            string[] chartokens = tokendots[0].Split(new string[] { " " }, StringSplitOptions.None);

            if (from == "OneSpace")
            {
                StringFinal = tokendots[0];
            }

            if (from == "Standard")
            {
                foreach (string index in tokendots)
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

            return result;

        }

        public StringArgs Args { get; set; }

        public StringOperation Clone()
        {
            return new FullnameNormalizeOperation()
            {
                Args = new FullnameNormalizeArgs()
            };
        }

        public void ShowEditDialog()
        {
            var screen = new FullnameNormalizeDialog(Args as FullnameNormalizeArgs);

            if (screen.ShowDialog() == true)
            {
                var myArgs = Args as FullnameNormalizeArgs;
                myArgs.From = screen.From;
            }
        }
    }
}
