using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class NewCaseArgs : StringArgs, INotifyPropertyChanged
    {
        private string _from;
        public string Description => $"New Case with {From}";

        public string From
        {
            get => _from; set
            {
                _from = value;
                NotifyChange("From");
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
