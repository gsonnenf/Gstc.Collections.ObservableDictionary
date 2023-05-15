using Gstc.Collections.ObservableDictionary.CollectionView;
using Gstc.Collections.ObservableDictionary.Demo.Model;
using System.Windows;
using System.Windows.Controls;

namespace Gstc.Collections.ObservableDictionary.Demo {
    /// <summary>
    /// Interaction logic for ObservableDictionaryListViewControl.xaml
    /// </summary>
    public partial class ObservableDictionaryListViewControl : UserControl {
        public readonly static DependencyProperty DictionarySourceProperty
            = DependencyProperty.Register(nameof(CustomerVm), typeof(CustomerVmListView), typeof(ObservableDictionaryListViewControl));

        //private static readonly DependencyPropertyDescriptor DictionarySourceDpd
        //    = DependencyPropertyDescriptor.FromProperty(DictionarySourceProperty, typeof(ObservableDictionaryListViewControl));


        public CustomerVmListView CustomerVm {
            get => (CustomerVmListView)GetValue(DictionarySourceProperty);
            set => SetValue(DictionarySourceProperty, value);
        }

        public IObservableDictionary<string, Customer> ObvDictionary => CustomerVm.ObvDictionaryCustomers;
        public ObservableListViewKvp<string, Customer> ObvListView => CustomerVm.ObvListViewCustomer;

        public ObservableDictionaryListViewControl() {
            InitializeComponent();
        }
        private void KeyListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => CustomerVm.SelectedCustomerKey = (string)ListViewKeys.SelectedItem;

    }
}
