using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SelectionSort.Model;
using SelectionSort.WindowExtensions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SelectionSort
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : DesktopWindow
    {
        /// <summary>
        /// Observable collection contain generated random value for further sorting.
        /// </summary>
        private ObservableCollection<Item> _list = [];

        /// <summary>
        /// Current is sorted status of _list
        /// </summary>
        private bool _isSorted = false;

        public MainWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            if (this.AppWindow.Presenter is OverlappedPresenter overlappedPresenter)
            {
                overlappedPresenter.IsResizable = false;
                overlappedPresenter.IsMaximizable = false;
            }

        }

        /// <summary>
        /// Generate random value and add them to list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRandomValueGereatorButtonsClick(object sender, RoutedEventArgs e)
        {
            _list.Clear();

            Random random = new();
            for (int i = 0, j = 1200; i < j; i++)
            {
                _list.Add(new Item() { Value = random.Next(1, 1000) });
            }
            _isSorted = false;
        }

        /// <summary>
        /// Clear the list from generated values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnResetButtonsClick(object sender, RoutedEventArgs e)
        {
            _list.Clear();
            _isSorted = false;
        }

        /// <summary>
        /// Sort the list by selectionsort algorithm and use tupple for swaping(cleaner)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSortUseTuppleButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_isSorted)
            {

                for (int i = 0; i < _list.Count - 1; i++)
                {
                    int minimumItemIndex = i;

                    for (int j = i + 1; j < _list.Count; j++)
                    {
                        if (_list[j].Value < _list[minimumItemIndex].Value)
                        {
                            minimumItemIndex = j;
                        }
                    }

                    //If smaler value founded.
                    if (minimumItemIndex != i)
                    {
                        (_list[minimumItemIndex], _list[i]) = (_list[i], _list[minimumItemIndex]);
                    }
                }

                _isSorted = true;
            }
        }

        /// <summary>
        /// Sort the list by selectionsort algorithm and use temp variable for swaping(faster)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSortUseTempVarButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_isSorted)
            {
                for (int i = 0; i < _list.Count - 1; i++)
                {
                    int minimumItemIndex = i;

                    for (int j = i + 1; j < _list.Count; j++)
                    {
                        if (_list[j].Value < _list[minimumItemIndex].Value)
                        {
                            minimumItemIndex = j;
                        }
                    }

                    //If smaler value founded.
                    if (minimumItemIndex != i)
                    {
                        Item tempItem = _list[i];
                        _list[i] = _list[minimumItemIndex];
                        _list[minimumItemIndex] = tempItem;
                    }
                }

                _isSorted = true;
            }
        }
    }
}
