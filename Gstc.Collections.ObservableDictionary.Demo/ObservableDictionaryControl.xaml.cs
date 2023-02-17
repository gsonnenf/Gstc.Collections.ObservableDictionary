using Gstc.Collections.ObservableDictionary.Demo.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Gstc.Collections.ObservableDictionary.Demo;

public partial class ObservableDictionaryControl : UserControl {

    public readonly static DependencyProperty DictionarySourceProperty
        = DependencyProperty.Register(nameof(CustomerVm), typeof(ICustomerVm), typeof(ObservableDictionaryControl));

    private static readonly DependencyPropertyDescriptor DictionarySourceDpd
        = DependencyPropertyDescriptor.FromProperty(DictionarySourceProperty, typeof(ObservableDictionaryControl));

    public ICustomerVm CustomerVm {
        get => (ICustomerVm)GetValue(DictionarySourceProperty);
        set => SetValue(DictionarySourceProperty, value);
    }

    public IObservableDictionary<string, Customer> ObvDictionary => CustomerVm.ObvDictionaryCustomers;

    public ObservableDictionaryControl() {
        InitializeComponent();
        DictionarySourceDpd.AddValueChanged(this, BindEvents);
        CustomerVm = new CustomerVmListView();
        ListViewKeys.SelectedIndex = 0;
    }

    private void BindEvents(object? sender, EventArgs? e) {
        //Respond to dictionary events
        ObvDictionary.AddedKvp += (_, args) => ListViewKeys.SelectedItem = args.NewValue.TransactionId;
        ObvDictionary.RemovedKvp += (_, args) => ListViewKeys.SelectedIndex = 0;
        ObvDictionary.ResetKvp += (_, _) => ListViewKeys.SelectedIndex = 0;
        ObvDictionary.ReplacedKvp += (_, args) => {
            ListViewKeys.SelectedItem = null;
            ListViewKeys.SelectedItem = args.NewValue.TransactionId;
        };

        //Logging events.
        ObvDictionary.AddingKvp += (_, args) => Log("Adding item " + args.Key);
        ObvDictionary.RemovingKvp += (_, args) => Log("Removing item " + args.Key);
        ObvDictionary.ResettingKvp += (_, _) => Log("Dictionary Resetting.");
        ObvDictionary.ReplacingKvp += (_, args) => Log("Replacing item " + args.Key + " Version " + args.NewValue.Version);
    }

    private void KeyListView_SelectionChanged(object sender, SelectionChangedEventArgs e) { }//=>CustomerVm.SelectedCustomerKey = (string)ListViewKeys.SelectedItem;

    private void Log(string message) => LogTextBox.Text = "[" + DateTime.Now.ToString("mm:ss") + "]" + message + "\n" + LogTextBox.Text;
}

