using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

using SimpleMvvmToolkit;

namespace NavisionObjectImporterExporterTool.Models
{
    public class SelectedConnectionModel : ModelBase<SelectedConnectionModel>
    {
        private bool _ischecked;
        public bool IsChecked
        {
            get { return _ischecked; }
            set
            {
                _ischecked = value;
                NotifyPropertyChanged(m => m.IsChecked);
            }
        }

        private NavisionConnection _connection;
        public NavisionConnection Connection
        {
            get { return _connection; }
            set
            {
                _connection = value;
                NotifyPropertyChanged(m => m.Connection);
            }
        }
    }
}
