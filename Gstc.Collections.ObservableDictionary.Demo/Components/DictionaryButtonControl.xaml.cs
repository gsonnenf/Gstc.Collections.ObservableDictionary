using Gstc.Collections.ObservableDictionary.Demo.Model;
using System.Windows;
using System.Windows.Controls;

namespace Gstc.Collections.ObservableDictionary.Demo.Components;
/// <summary>
/// Interaction logic for DictionaryButtonControl.xaml
/// </summary>
/// 
public partial class DictionaryButtonControl : UserControl {

    public readonly static DependencyProperty CustomerVmProperty =
     DependencyProperty.Register(nameof(CustomerVm), typeof(ICustomerVm), typeof(DictionaryButtonControl));

    public ICustomerVm CustomerVm {
        get => (ICustomerVm)GetValue(CustomerVmProperty);
        set => SetValue(CustomerVmProperty, value);
    }

    public DictionaryButtonControl() {
        InitializeComponent();
    }


}
