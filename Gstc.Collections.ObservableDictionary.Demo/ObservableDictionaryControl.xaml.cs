using Gstc.Collections.ObservableDictionary.ComponentModel;
using Gstc.Collections.ObservableDictionary.Demo.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Gstc.Collections.ObservableDictionary.Demo;

public partial class ObservableDictionaryControl : UserControl {

    #region Static
    public readonly static DependencyProperty DictionarySourceProperty
        = DependencyProperty.Register(
            nameof(CustomerVm),
            typeof(ICustomerVm),
            typeof(ObservableDictionaryControl)
            );

    private static readonly DependencyPropertyDescriptor DictionarySourceDpd
        = DependencyPropertyDescriptor.FromProperty(DictionarySourceProperty, typeof(ObservableDictionaryControl));
    #endregion

    public ICustomerVm CustomerVm {
        get => (ICustomerVm)GetValue(DictionarySourceProperty);
        set {
            UnbindEvents();
            SetValue(DictionarySourceProperty, value); //TOdo: add back in bind.
            BindEvents();
        }
    }

    public ObservableDictionaryControl() {
        InitializeComponent();
        DictionarySourceDpd.AddValueChanged(this, BindEvents);
        var customerVm = new CustomerVmEnumerableView();
        CustomerVm = customerVm;
        ListViewKeys.SelectedIndex = 0;
        var a = customerVm.EnumerableKeyCustomers;
    }

    private void BindEvents() {
        //Respond to dictionary events
        CustomerVm.ObvDictionaryCustomers.DictionaryChanging += LogDictionaryChanging;
        CustomerVm.ObvDictionaryCustomers.AddedKvp += AddKvpCallback;
        CustomerVm.ObvDictionaryCustomers.RemovedKvp += RemovedKvpCallback;
        CustomerVm.ObvDictionaryCustomers.ResetKvp += ResetKvpCallback;
        CustomerVm.ObvDictionaryCustomers.ReplacedKvp += ReplaceKvpCallback;
    }

    private void UnbindEvents() {
        //Respond to dictionary events
        CustomerVm.ObvDictionaryCustomers.DictionaryChanging -= LogDictionaryChanging;
        CustomerVm.ObvDictionaryCustomers.AddedKvp -= AddKvpCallback;
        CustomerVm.ObvDictionaryCustomers.RemovedKvp -= RemovedKvpCallback;
        CustomerVm.ObvDictionaryCustomers.ResetKvp -= ResetKvpCallback;
        CustomerVm.ObvDictionaryCustomers.ReplacedKvp -= ReplaceKvpCallback;
    }

    #region List View Special behavior;


    void AddKvpCallback(object sender, DictAddEventArgs<string, Customer> args) => ListViewKeys.SelectedItem = args.NewValue.TransactionId;
    void RemovedKvpCallback(object sender, DictRemoveEventArgs<string, Customer> args) => ListViewKeys.SelectedIndex = 0;
    void ResetKvpCallback(object sender, DictResetEventArgs<string, Customer> args) => ListViewKeys.SelectedIndex = 0;
    void ReplaceKvpCallback(object sender, DictReplaceEventArgs<string, Customer> args) {
        ListViewKeys.SelectedItem = null;
        ListViewKeys.SelectedItem = args.NewValue.TransactionId;
    }

    void LogDictionaryChanging(object sender, IDictionaryChangedEventArgs<string, Customer> args) {
        switch (args.Action) {
            case NotifyDictionaryChangedAction.Add:
                Log("Attempting to Add item " + args.Key);
                break;
            case NotifyDictionaryChangedAction.Remove:
                Log("Attempting to Remove item " + args.Key);
                break;
            case NotifyDictionaryChangedAction.Replace:
                Log("Attempting to replace item " + args.Key + ", Version " + args.NewValue.Version);
                break;
            case NotifyDictionaryChangedAction.Reset:
                Log("Attempting to Reset Dictionary.");
                break;
            default:
                throw new InvalidEnumArgumentException();
        }
    }
#endregion
    private void KeyListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => CustomerVm.SelectedCustomerKey = (string)ListViewKeys.SelectedItem;

    private void Log(string message) => LogTextBox.Text = "[" + DateTime.Now.ToString("mm:ss") + "]" + message + "\n" + LogTextBox.Text;
}
