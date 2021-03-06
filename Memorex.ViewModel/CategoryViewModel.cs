﻿using Memorex.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Memorex.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        public static EventHandler CategoryViewModelChanged;
        public ICommand AddCategoryCommand { get; set; }

        private string _category = String.Empty;
     
        public string Category
        {
            get => _category;
            set => SetField(ref _category, value);
        }


        public CategoryViewModel()
        {
            AddCategoryCommand = new RelayCommand<object>(ExecuteAdd, CanExecuteAdd);
        }

        [DebuggerStepThrough]
        private bool CanExecuteAdd(object obj)
        {
            if (Category != null)
                if (Category != String.Empty)
                    return true;
            return false;
        }

        private void ExecuteAdd(object obj)
        {
            DatabaseHandler.Instance.AddCategoryIfNotExist(Category);
            Category = String.Empty;
            CategoryViewModelChanged?.Invoke(null, null);
            InputViewModel.InputViewModelChanged?.Invoke(null, null);
        }
    }
}
