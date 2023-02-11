using System;
using System.ComponentModel;
using System.Collections;
using Avalonia.Controls;
using ReactiveUI;

namespace English2k23.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    ViewModelBase content;
    private Stack viewStack;

    public MainWindowViewModel()
    {
        viewStack = new Stack();
        var hmv = new HomeViewModel();
        viewStack.Push(hmv);
        Content =  hmv;
    }

    public ViewModelBase Content
    {
        get => content;
        private set => this.RaiseAndSetIfChanged(ref content, value);
    }

    public void TestButtonClicked()
    {
        var avm = new AnotherViewModel();
        viewStack.Push(avm);
        Content = avm;
    }

    public void TestButtonClicked2()
    {
        var avm2 = new AnotherViewModel2();
        viewStack.Push(avm2);
        Content = avm2;
    }
    
    public void HomeButtonClicked() {
        viewStack.Clear();
        var hvm = new HomeViewModel();
        viewStack.Push(hvm);
        Content = hvm;
    }
    public void ReturnButtonClicked()
    {
        if (viewStack.Count > 1)
        {
            viewStack.Pop();
            Content = (ViewModelBase)viewStack.Peek();
        }
    }
}
