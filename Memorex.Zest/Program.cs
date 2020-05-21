using Memorex.Model;
using Memorex.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorex.Zest
{
    class Program
    {
        static void Main(string[] args)
        {

                var a = DatabaseHandler.Instance.AddEntry("argdfg", "dsafgdfg", "dfasdf", "dsfasdgshfgj>");
                DatabaseHandler.Instance.AddEntry("sdf", "sap", "abap", "test");
                //DatabaseHandler.Instance.Delete(a);
                DatabaseHandler.Instance.SearchEntry("sap abap test");



            SearchViewModel svm = new SearchViewModel();

            svm.SearchTerm = "sap abap test";

            svm.SearchCommand.Execute(null);

            var b = svm.Elements;


        }
    }
}
