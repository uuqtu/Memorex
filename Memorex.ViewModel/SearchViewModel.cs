using Memorex.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Memorex.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        public static EventHandler SearchViewModelChanged;
        public ICommand SearchCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private KnoledgeElement _selectedElement = null;

        public KnoledgeElement SelectedElement
        {
            get => _selectedElement;
            set
            {
                SetField(ref _selectedElement, value);
                Clipboard.SetText(value?.Link ?? "");
            }
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set => SetField(ref _searchTerm, value);
        }
        public ObservableCollection<KnoledgeElement> Elements { get; set; } = new ObservableCollection<KnoledgeElement>();


        public SearchViewModel()
        {
            SearchCommand = new RelayCommand<object>(ExecuteSearch, CanExecuteSearch);
            ClearCommand = new RelayCommand<object>(ExecuteClear);
            DeleteCommand = new RelayCommand<object>(ExecuteDeleteCommand, CanDeleteCommand);
        }
        #region SearchCommand

        [DebuggerStepThrough]
        private bool CanExecuteSearch(object obj)
        {
            return SearchTerm != String.Empty && SearchTerm != null;
        }

        private void ExecuteSearch(object obj)
        {
            Elements.Clear();
            foreach (var i in DatabaseHandler.Instance.SearchEntry(SearchTerm).OrderBy(x => x.MatchingScore)) Elements.Add(i);
            SearchViewModelChanged?.Invoke(null, null);
        }
        #endregion

        #region ClearCommand

        private void ExecuteClear(object obj)
        {
            Elements.Clear();
            SearchTerm = String.Empty;
            SelectedElement = null;
            SearchViewModelChanged?.Invoke(null, null);
        }
        #endregion

        #region DeleteCommand

        [DebuggerStepThrough]
        private bool CanDeleteCommand(object obj)
        {
            return SelectedElement != null;
        }

        private void ExecuteDeleteCommand(object obj)
        {
            var rslt = System.Windows.MessageBox.Show($"Eintrag mit den Suchworten \n\"{SelectedElement?.Searchwords}\" \nund Link \n\"{SelectedElement?.Link}\" \n wirklich löschen?", "", MessageBoxButton.YesNo);
            if (rslt == MessageBoxResult.Yes)
            {
                DatabaseHandler.Instance.DeleteEntry(SelectedElement.Id);
                ExecuteSearch(null);
            }
        }

        #endregion
    }
}
