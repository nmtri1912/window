using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class ReplaceArgs : StringArgs, INotifyPropertyChanged
    {

        private string _from;
        private string _to;

        public string Description => $"Replace {From} with {To}";

        public string From
        {
            get => _from; set
            {
                _from = value;
                NotifyChange("From");
                NotifyChange("Description");
            }
        }
        public string To
        {
            get => _to; set
            {
                _to = value;
                NotifyChange("To");
                NotifyChange("Description");
            }
        }

        private void NotifyChange(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(v));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
