using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gstc.Collections.ObservableDictionary.Demo.Model;
public interface ICustomerVm {
    public IObservableDictionary<string, Customer> ObvDictionaryCustomers { get; }
    public IEnumerable<string> EnumerableCustomers { get; }
    public INotifyCollectionChanged NotifyCustomers { get; }

    public string? SelectedCustomerKey { get; set; }
    void AddItem();
    void ClearItems();
    void RemoveItem();
    void ReplaceItem();
    void ReplaceItems(int numberOfCustomers);
}