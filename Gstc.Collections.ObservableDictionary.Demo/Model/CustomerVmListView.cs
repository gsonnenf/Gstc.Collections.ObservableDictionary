using Gstc.Collections.ObservableDictionary.CollectionView;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.Demo.Model;
public class CustomerVmListView : NotifyPropertyChanged, ICustomerVm {

    #region Properties
    private ObservableDictionary<string, Customer> _obvDictCustomer = new();
    private string? _selectedCustomerKey;
    public IObservableDictionary<string, Customer> ObvDictionaryCustomers => _obvDictCustomer;
    public ObservableListViewKvp<string, Customer> ObvListViewCustomer => _obvDictCustomer.ObservableListViewKvp;
    public IEnumerable<KeyValuePair<string, Customer>> EnumerableKvpCustomers => _obvDictCustomer.ObservableListViewKvp;
    public INotifyCollectionChanged NotifyCustomers => _obvDictCustomer.ObservableListViewKey;

    public string? SelectedCustomerKey {
        get => _selectedCustomerKey;
        set {
            _selectedCustomerKey = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedCustomer));
        }
    }

    public Customer? SelectedCustomer => (_selectedCustomerKey != null) ? _obvDictCustomer[_selectedCustomerKey] : null;
    #endregion

    #region Command Mapping
    public SimpleActionCommand AddItemCommand { get; private set; }
    public SimpleActionCommand ClearItemsCommand { get; private set; }
    public SimpleActionCommand RemoveItemCommand { get; private set; }
    public SimpleActionCommand ReplaceItemCommand { get; private set; }
    public SimpleActionCommand ReplaceItemsCommand { get; private set; }
    public SimpleActionCommand ReplaceDictCommand { get; private set; }
    #endregion

    #region Constructor
    public CustomerVmListView() {
        _obvDictCustomer.Dictionary = Customer.GenerateCustomerDictionary(5);
        AddItemCommand = new SimpleActionCommand(AddItem);
        ClearItemsCommand = new SimpleActionCommand(ClearItems, () => _obvDictCustomer.Count > 0);
        RemoveItemCommand = new SimpleActionCommand(RemoveItem, () => SelectedCustomerKey != null);
        ReplaceItemCommand = new SimpleActionCommand(ReplaceItem, () => SelectedCustomerKey != null);
        ReplaceItemsCommand = new SimpleActionCommand(() => ReplaceItems(3));
        ReplaceDictCommand = new SimpleActionCommand(() => ReplaceDict(5));


        _obvDictCustomer.DictionaryChanged += (_, _) => ClearItemsCommand.CallCanExecuteChanged();
        PropertyChanged += (_, e) => {
            if (e.PropertyName == nameof(SelectedCustomerKey)) {
                RemoveItemCommand.CallCanExecuteChanged();
                ReplaceItemCommand.CallCanExecuteChanged();
            }
        };
    }
    #endregion

    #region command logic
    public void AddItem() {
        var customer = Customer.GenerateCustomer();
        _obvDictCustomer?.Add(customer.TransactionId, customer);
    }
    public void ClearItems() => _obvDictCustomer?.Clear();

    public void RemoveItem() {
        if (SelectedCustomerKey == null) return;
        _obvDictCustomer?.Remove(SelectedCustomerKey);
    }

    public void ReplaceItem() {
        if (SelectedCustomerKey == null) return;
        var oldItem = _obvDictCustomer[SelectedCustomerKey];
        _obvDictCustomer[SelectedCustomerKey] = new Customer() {
            TransactionId = oldItem.TransactionId,
            FirstName = oldItem.FirstName,
            LastName = oldItem.LastName,
            BirthDate = oldItem.BirthDate,
            PurchaseAmount = oldItem.PurchaseAmount + 10,
            Version = oldItem.Version + 1
        };
    }

    public void ReplaceItems(int numberOfCustomers) {
        _obvDictCustomer.Clear();
        var list = Customer.GenerateCustomerList(numberOfCustomers);
        list.ForEach(item => _obvDictCustomer.Add(item.TransactionId, item));
    }

    public void ReplaceDict(int numberOfCustomers) => _obvDictCustomer.Dictionary = Customer.GenerateCustomerDictionary(numberOfCustomers);
    #endregion
}
