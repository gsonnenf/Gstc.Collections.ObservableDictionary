using Gstc.Collections.ObservableDictionary.Demo.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Gstc.Collections.ObservableDictionary.Demo.Components;
/// <summary>
/// Interaction logic for DictionaryButtonControl.xaml
/// </summary>
/// 
public partial class DictionaryButtonControl : UserControl {

    public event Action? AddItem;
    public event Action? RemoveItem;
    public event Action? ReplaceItem;
    public event Action? ClearItems;
    public event Action? ReplaceItems;
    public Func<string?>? GetSelectedKey;

    public readonly static DependencyProperty DictionarySourceProperty =
     DependencyProperty.Register(nameof(DictionarySource), typeof(IDictionary<string, Customer>),
         typeof(DictionaryButtonControl), new PropertyMetadata(new Dictionary<string, Customer>()));

    public IDictionary<string, Customer> DictionarySource {
        get => (IDictionary<string, Customer>)GetValue(DictionarySourceProperty);
        set => SetValue(DictionarySourceProperty, value);
    }

    ICustomerCrud customerCrud = CustomerCrud.Instance;

    public DictionaryButtonControl() {
        InitializeComponent();
    }

    private void Button_Click_Add(object sender, System.Windows.RoutedEventArgs e) {
        customerCrud.AddItem(DictionarySource);
        AddItem?.Invoke();
    }
    private void Button_Click_ClearAll(object sender, System.Windows.RoutedEventArgs e) {
        customerCrud.ClearItems(DictionarySource);
        ClearItems?.Invoke();
    }

    private void Button_Click_Remove(object sender, System.Windows.RoutedEventArgs e) {
        customerCrud.RemoveItem(DictionarySource, GetSelectedKey?.Invoke());
        RemoveItem?.Invoke();
    }

    private void Button_Click_Replace(object sender, System.Windows.RoutedEventArgs e) {
        customerCrud.ReplaceItem(DictionarySource, GetSelectedKey?.Invoke());
        ReplaceItem?.Invoke();
    }


    private void Button_Click_ReplaceAll(object sender, System.Windows.RoutedEventArgs e) {
        customerCrud.ReplaceItems(DictionarySource, 5);
        ReplaceItems?.Invoke();
    }
}
