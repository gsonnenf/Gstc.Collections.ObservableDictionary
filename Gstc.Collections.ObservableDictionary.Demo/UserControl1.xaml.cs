﻿using System.Windows.Controls;

namespace Gstc.Collections.ObservableDictionary.Demo;
/// <summary>
/// Interaction logic for UserControl1.xaml
/// </summary>
public partial class UserControl1 : UserControl {


    public UserControl1() {
        InitializeComponent();
        CustomerListView.ItemsSource = (new Class1());
    }


}
