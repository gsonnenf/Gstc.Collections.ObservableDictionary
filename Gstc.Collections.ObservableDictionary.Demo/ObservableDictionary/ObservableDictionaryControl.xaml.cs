using Gstc.Collections.ObservableDictionary.Demo.Model;
using System;
using System.Linq;
using System.Windows.Controls;
namespace Gstc.Collections.ObservableDictionary.Demo.ObservableDictionary;
/// <summary>
/// Interaction logic for ObservableDictionaryControl.xaml
/// </summary>
public partial class ObservableDictionaryControl : UserControl {

    ObservableDictionary<string, Customer> ObservableDictionaryCustomer { get; set; } = new();
    ICustomerCrud customerCrud = CustomerCrud.Instance;

    public ObservableDictionaryControl() {
        InitializeComponent();

        //Setting up controls
        DictButtonControl.DictionarySource = ObservableDictionaryCustomer;
        DictButtonControl.GetSelectedKey += () => (KeyListView?.SelectedItem != null) ? (string)KeyListView.SelectedItem! : null;

        //Populating initial data
        customerCrud.ReplaceItems(ObservableDictionaryCustomer, 5);
        KeyListView.ItemsSource = ObservableDictionaryCustomer.Select(kvp => kvp.Key);


        //Registering Dictionary Events
        ObservableDictionaryCustomer.DictionaryChanged += (_, _)
            => KeyListView.ItemsSource = ObservableDictionaryCustomer.Select(kvp => kvp.Key);

        ObservableDictionaryCustomer.AddedDict += (_, args) => {
            KeyListView.SelectedItem = args.NewValue.TransactionId;
            Log("Added item " + args.Key);
        };
        ObservableDictionaryCustomer.RemovingDict += (_, args) => {
            KeyListView.SelectedIndex = 0;
            Log("Removing item " + args.Key);
        };
        ObservableDictionaryCustomer.ResetDict += (_, _) => {
            KeyListView.SelectedIndex = 0;
            Log("Dictionary Cleared.");
        };

        ObservableDictionaryCustomer.ReplacedDict += (_, args) => {
            KeyListView.SelectedItem = null;
            KeyListView.SelectedItem = args.NewValue.TransactionId;
            Log("Replacing item " + args.Key + " Version " + args.NewValue.Version);
        };
    }

    private void KeyListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        var item = KeyListView.SelectedItem;
        ItemPropertyGrid.SelectedObject = (item != null) ? ObservableDictionaryCustomer[(string)item] : null;
    }

    private void Log(string message)
        => LogTextBox.Text = "[" + DateTime.Now.ToString("mm:ss") + "]" + message + "\n" + LogTextBox.Text;
}
