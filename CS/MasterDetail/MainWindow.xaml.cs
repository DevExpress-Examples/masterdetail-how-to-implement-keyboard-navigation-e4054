using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Utils;
using DevExpress.Xpf.Core.Native;

namespace MasterDetail {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            DataContext = new VM();
            InitializeComponent();
        }
    }
    #region Data
    public class VM {
        public List<Group> Source { get; private set; }
        public VM() {
            Source = new List<Group>();
            Group gr;
            gr = new Group() { GroupName = "Group 1" };
            gr.Persons.Add(new Person() { FirstName = "First Name 1", LastName = "Last Name1" });
            gr.Persons.Add(new Person() { FirstName = "First Name 2", LastName = "Last Name2" });
            Source.Add(gr);
            gr = new Group() { GroupName = "Group 2" };
            gr.Persons.Add(new Person() { FirstName = "First Name 1", LastName = "Last Name1" });
            gr.Persons.Add(new Person() { FirstName = "First Name 2", LastName = "Last Name2" });
            Source.Add(gr);
            gr = new Group() { GroupName = "Group 3" };
            gr.Persons.Add(new Person() { FirstName = "First Name 1", LastName = "Last Name1" });
            gr.Persons.Add(new Person() { FirstName = "First Name 2", LastName = "Last Name2" });
            Source.Add(gr);
        }
    }
    public class Group {
        public Group() {
            this.Persons = new List<Person>();
        }

        public String GroupName { get; set; }
        public List<Person> Persons { get; set; }
    }
    public class Person {
        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
    #endregion

    public class MasterTableView : TableView {
        public MasterTableView() {
            PreviewKeyDown += new KeyEventHandler(MasterTableView_PreviewKeyDown);
        }
        void SetDetailRowVisibility(int rowHandle, bool value) {
            FrameworkElement rowElement = GetRowElementByRowHandle(rowHandle);
            RowData rowData = (RowData)rowElement.DataContext;
            rowData.RowState.SetValue(DXDetailPresenter.IsDetailVisibleProperty, value);
        }
        GridControl GetDetailGrid(FrameworkElement rowElement) {
            return LayoutHelper.FindElement(rowElement, (el) => el is GridControl) as GridControl;
        }
        void MasterTableView_PreviewKeyDown(object sender, KeyEventArgs e) {
            if(KeyboardHelper.IsControlPressed) {
                if(e.Key == Key.Down) {
                    SetDetailRowVisibility(FocusedRowHandle, true);
                    e.Handled = true;
                    return;
                }
                if(e.Key == Key.Up) {
                    SetDetailRowVisibility(FocusedRowHandle, false);
                    e.Handled = true;
                    return;
                }
                return;
            }
            if(e.Key == Key.Down) {
                FrameworkElement rowElement = GetRowElementByRowHandle(FocusedRowHandle);
                GridControl gc = GetDetailGrid(rowElement);
                if(gc == null) return;
                TableView v = (TableView)gc.View;
                if(gc.GetRowVisibleIndexByHandle(v.FocusedRowHandle) == gc.VisibleRowCount - 1) {
                    MoveNextRow();
                    Focus();
                    e.Handled = true;
                    return;
                }
                v.MoveNextRow();
                v.Focus();
                e.Handled = true;
                return;
            }
            if(e.Key == Key.Up) {
                FrameworkElement rowElement = GetRowElementByRowHandle(FocusedRowHandle);
                GridControl gc = GetDetailGrid(rowElement);
                if(gc == null) return;
                TableView v = (TableView)gc.View;
                if(gc.GetRowVisibleIndexByHandle(v.FocusedRowHandle) == 0) {
                    MovePrevRow();
                    Focus();
                    e.Handled = true;
                    return;
                }
                v.MovePrevRow();
                v.Focus();
                e.Handled = true;
                return;
            }
        }
    }
}
