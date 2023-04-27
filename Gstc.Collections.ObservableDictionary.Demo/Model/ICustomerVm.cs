using Gstc.Collections.ObservableDictionary.ObservableList;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.Demo.Model;
public interface ICustomerVm {
    public IObservableDictionary<string, Customer> ObvDictionaryCustomers { get; }
    public IObservableEnumerable<KeyValuePair<string, Customer>> EnumerableKvpCustomers { get; }
    public IObservableEnumerable<string> EnumerableKeyCustomers { get; }
    public INotifyCollectionChanged NotifyCustomers { get; }
    public string? SelectedCustomerKey { get; set; }
    void AddItem();
    void ClearItems();
    void RemoveItem();
    void ReplaceItem();
    void ReplaceItems(int numberOfCustomers);
}