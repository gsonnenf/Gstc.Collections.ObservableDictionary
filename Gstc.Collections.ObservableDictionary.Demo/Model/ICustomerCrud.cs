using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.Demo.Model;
internal interface ICustomerCrud {
    void AddItem(IDictionary<string, Customer> dictionary);
    void ClearItems(IDictionary<string, Customer> dictionary);
    void RemoveItem(IDictionary<string, Customer> dictionary, string? key);
    void ReplaceItem(IDictionary<string, Customer> dictionary, string? key);
    void ReplaceItems(IDictionary<string, Customer> dictionary, int numberOfCustomers);
}