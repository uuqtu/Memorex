using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Memorex.ViewModel
{
    public class ViewModel
    {
        ICommand Add { get; set; }
        ICommand Search { get; set; }
        ICommand Delete { get; set; }
        
        public ViewModel()
        {
           // Add = new RelayCommand
        }

    }
}
