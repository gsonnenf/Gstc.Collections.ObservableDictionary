using Gstc.Collections.ObservableDictionary.ObservableList;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.Demo.Model;

public enum DictionaryExampleType {
    ObservableDictionaryEnumerable,
    ObservableDictionaryList,
    ObservableDictionaryKeyedCollection
};

public class CustomerVmEnumerableDictionary : NotifyPropertyChanged, ICustomerVm {

    #region Fields/Properties methods for 3 different types of example dictionaries
    private DictionaryExampleType MyDictionaryType { get; set; }
    private ObservableDictionary<string, Customer> _observableDictionaryCustomer;
    private ObservableKeyedCollection<string, Customer> _observableKeyedCollectionCustomer;

    public IObservableDictionary<string, Customer> ObvDictionaryCustomers => MyDictionaryType switch {
        DictionaryExampleType.ObservableDictionaryEnumerable => _observableDictionaryCustomer,
        DictionaryExampleType.ObservableDictionaryList => _observableDictionaryCustomer,
        //DictionaryType.ObservableDictionaryKeyedCollection => _observableKeyedCollectionCustomer,
        _ => throw new System.NotImplementedException()
    };

    public IObservableEnumerable<string> EnumerableKeyCustomers => MyDictionaryType switch {
        DictionaryExampleType.ObservableDictionaryEnumerable => _observableDictionaryCustomer.ObservableEnumerableKeyUnordered,
        DictionaryExampleType.ObservableDictionaryList => _observableDictionaryCustomer.ObservableListKvpOrdered,
        //DictionaryType.ObservableDictionaryKeyedCollection => _observableKeyedCollectionCustomer,
        _ => throw new System.NotImplementedException()
    };

    private void ReplaceInternalDictionary(Dictionary<string, Customer> dictionary) {
        switch (MyDictionaryType) {
            case DictionaryExampleType.ObservableDictionaryEnumerable:
                _observableDictionaryCustomer.Dictionary = dictionary;
                break;
            case DictionaryExampleType.ObservableDictionaryList:
                _observableDictionaryCustomer.Dictionary = dictionary;
                break;
            case DictionaryExampleType.ObservableDictionaryKeyedCollection:
                //_observableKeyedCollectionCustomer.Dictionary = dictionary;
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
    #endregion

    #region Fields and Properties
    private string? _selectedCustomerKey;
    public string? SelectedCustomerKey {
        get => _selectedCustomerKey;
        set {
            _selectedCustomerKey = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedCustomer));
        }
    }
    public Customer? SelectedCustomer => (_selectedCustomerKey != null) ? ObvDictionaryCustomers[_selectedCustomerKey] : null;

    public SimpleActionCommand AddItemCommand { get; private set; }
    public SimpleActionCommand ClearItemsCommand { get; private set; }
    public SimpleActionCommand RemoveItemCommand { get; private set; }
    public SimpleActionCommand ReplaceItemCommand { get; private set; }
    public SimpleActionCommand ReplaceItemsCommand { get; private set; }
    public SimpleActionCommand ReplaceDictCommand { get; private set; }
    #endregion

    #region Constructor
    public CustomerVmEnumerableDictionary(IObservableDictionary<string, Customer> dictionary) {

        ReplaceInternalDictionary(Customer.GenerateCustomerDictionary(5));
        AddItemCommand = new SimpleActionCommand(AddItem);
        ClearItemsCommand = new SimpleActionCommand(ClearItems, () => ObvDictionaryCustomers.Count > 0);
        RemoveItemCommand = new SimpleActionCommand(RemoveItem, () => SelectedCustomerKey != null);
        ReplaceItemCommand = new SimpleActionCommand(ReplaceItem, () => SelectedCustomerKey != null);
        ReplaceItemsCommand = new SimpleActionCommand(() => ReplaceItems(3));
        ReplaceDictCommand = new SimpleActionCommand(() => ReplaceDict(5));


        ObvDictionaryCustomers.DictionaryChanged += (_, _) => ClearItemsCommand.CallCanExecuteChanged();
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
        ObvDictionaryCustomers?.Add(customer.TransactionId, customer);
    }
    public void ClearItems() => ObvDictionaryCustomers?.Clear();

    public void RemoveItem() {
        if (SelectedCustomerKey == null) return;
        ObvDictionaryCustomers?.Remove(SelectedCustomerKey);
    }

    public void ReplaceItem() {
        if (SelectedCustomerKey == null) return;
        var oldItem = ObvDictionaryCustomers[SelectedCustomerKey];
        ObvDictionaryCustomers[SelectedCustomerKey] = new Customer() {
            TransactionId = oldItem.TransactionId,
            FirstName = oldItem.FirstName,
            LastName = oldItem.LastName,
            BirthDate = oldItem.BirthDate,
            PurchaseAmount = oldItem.PurchaseAmount + 10,
            Version = oldItem.Version + 1
        };
    }

    public void ReplaceItems(int numberOfCustomers) {
        ObvDictionaryCustomers.Clear();
        var list = Customer.GenerateCustomerList(numberOfCustomers);
        list.ForEach(item => ObvDictionaryCustomers.Add(item.TransactionId, item));
    }

    public void ReplaceDict(int numberOfCustomers) =>
        ReplaceInternalDictionary(Customer.GenerateCustomerDictionary(numberOfCustomers));
    #endregion
}
