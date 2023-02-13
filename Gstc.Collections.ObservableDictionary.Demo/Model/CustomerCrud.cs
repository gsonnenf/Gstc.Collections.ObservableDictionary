using System;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.Demo.Model;
internal class CustomerCrud : ICustomerCrud {

    public static readonly CustomerCrud Instance = new CustomerCrud();
    public static bool isInstance;

    public CustomerCrud() => isInstance = !isInstance ? true : throw new NotSupportedException("Singeton does not support multiple instances");

    public void AddItem(IDictionary<string, Customer> dictionary) {
        var customer = Customer.GenerateCustomer();
        dictionary?.Add(customer.TransactionId, customer);
    }
    public void ClearItems(IDictionary<string, Customer> dictionary) => dictionary?.Clear();

    public void RemoveItem(IDictionary<string, Customer> dictionary, string? key) {
        if (key == null) return;
        dictionary?.Remove(key);
    }

    public void ReplaceItem(IDictionary<string, Customer> dictionary, string? key) {
        if (key == null) return;
        var oldItem = dictionary[key];
        dictionary[key] = new Customer() {
            TransactionId = oldItem.TransactionId,
            FirstName = oldItem.FirstName,
            LastName = oldItem.LastName,
            BirthDate = oldItem.BirthDate,
            PurchaseAmount = oldItem.PurchaseAmount + 10,
            Version = oldItem.Version + 1
        };
    }

    public void ReplaceItems(IDictionary<string, Customer> dictionary, int numberOfCustomers) {
        dictionary.Clear();
        var list = Customer.GenerateCustomerList(numberOfCustomers);
        list.ForEach(item => dictionary.Add(item.TransactionId, item));
    }
}
