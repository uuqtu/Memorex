using Memorex.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Memorex.View
{
    /// <summary>
    /// Interaktionslogik für CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl, INotifyPropertyChanged
    {

        public CategoryViewModel CategoryVieModelObject { get; set; } = new CategoryViewModel();

        public CategoryView()
        {
            InitializeComponent();
        }

        private void CategoryViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (CategoryViewModel.CategoryViewModelChanged == null)
                CategoryViewModel.CategoryViewModelChanged += delegate (object o, EventArgs s) { OnPropertyChanged(nameof(CategoryVieModelObject)); };
            this.DataContext = CategoryViewControl;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
