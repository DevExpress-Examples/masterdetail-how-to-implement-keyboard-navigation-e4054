Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports DevExpress.Xpf.Grid
Imports DevExpress.Xpf.Utils
Imports DevExpress.Xpf.Core.Native

Namespace MasterDetail
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			DataContext = New VM()
			InitializeComponent()
		End Sub
	End Class
	#Region "Data"
	Public Class VM
		Private privateSource As List(Of Group)
		Public Property Source() As List(Of Group)
			Get
				Return privateSource
			End Get
			Private Set(ByVal value As List(Of Group))
				privateSource = value
			End Set
		End Property
		Public Sub New()
			Source = New List(Of Group)()
			Dim gr As Group
			gr = New Group() With {.GroupName = "Group 1"}
			gr.Persons.Add(New Person() With {.FirstName = "First Name 1", .LastName = "Last Name1"})
			gr.Persons.Add(New Person() With {.FirstName = "First Name 2", .LastName = "Last Name2"})
			Source.Add(gr)
			gr = New Group() With {.GroupName = "Group 2"}
			gr.Persons.Add(New Person() With {.FirstName = "First Name 1", .LastName = "Last Name1"})
			gr.Persons.Add(New Person() With {.FirstName = "First Name 2", .LastName = "Last Name2"})
			Source.Add(gr)
			gr = New Group() With {.GroupName = "Group 3"}
			gr.Persons.Add(New Person() With {.FirstName = "First Name 1", .LastName = "Last Name1"})
			gr.Persons.Add(New Person() With {.FirstName = "First Name 2", .LastName = "Last Name2"})
			Source.Add(gr)
		End Sub
	End Class
	Public Class Group
		Public Sub New()
			Me.Persons = New List(Of Person)()
		End Sub

		Private privateGroupName As String
		Public Property GroupName() As String
			Get
				Return privateGroupName
			End Get
			Set(ByVal value As String)
				privateGroupName = value
			End Set
		End Property
		Private privatePersons As List(Of Person)
		Public Property Persons() As List(Of Person)
			Get
				Return privatePersons
			End Get
			Set(ByVal value As List(Of Person))
				privatePersons = value
			End Set
		End Property
	End Class
	Public Class Person
		Private privateFirstName As String
		Public Property FirstName() As String
			Get
				Return privateFirstName
			End Get
			Set(ByVal value As String)
				privateFirstName = value
			End Set
		End Property
		Private privateLastName As String
		Public Property LastName() As String
			Get
				Return privateLastName
			End Get
			Set(ByVal value As String)
				privateLastName = value
			End Set
		End Property
	End Class
	#End Region

	Public Class MasterTableView
		Inherits TableView
		Public Sub New()
			AddHandler PreviewKeyDown, AddressOf MasterTableView_PreviewKeyDown
		End Sub
		Private Sub SetDetailRowVisibility(ByVal rowHandle As Integer, ByVal value As Boolean)
			Dim rowElement As FrameworkElement = GetRowElementByRowHandle(rowHandle)
			Dim rowData As RowData = CType(rowElement.DataContext, RowData)
			rowData.RowState.SetValue(DXDetailPresenter.IsDetailVisibleProperty, value)
		End Sub
		Private Function GetDetailGrid(ByVal rowElement As FrameworkElement) As GridControl
            Return TryCast(LayoutHelper.FindElement(rowElement, Function(el) Not (TryCast(el, GridControl) Is Nothing)), GridControl)
		End Function
		Private Sub MasterTableView_PreviewKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If KeyboardHelper.IsControlPressed Then
				If e.Key = Key.Down Then
					SetDetailRowVisibility(FocusedRowHandle, True)
					e.Handled = True
					Return
				End If
				If e.Key = Key.Up Then
					SetDetailRowVisibility(FocusedRowHandle, False)
					e.Handled = True
					Return
				End If
				Return
			End If
			If e.Key = Key.Down Then
				Dim rowElement As FrameworkElement = GetRowElementByRowHandle(FocusedRowHandle)
				Dim gc As GridControl = GetDetailGrid(rowElement)
				If gc Is Nothing Then
					Return
				End If
				Dim v As TableView = CType(gc.View, TableView)
				If gc.GetRowVisibleIndexByHandle(v.FocusedRowHandle) = gc.VisibleRowCount - 1 Then
					MoveNextRow()
					Focus()
					e.Handled = True
					Return
				End If
				v.MoveNextRow()
				v.Focus()
				e.Handled = True
				Return
			End If
			If e.Key = Key.Up Then
				Dim rowElement As FrameworkElement = GetRowElementByRowHandle(FocusedRowHandle)
				Dim gc As GridControl = GetDetailGrid(rowElement)
				If gc Is Nothing Then
					Return
				End If
				Dim v As TableView = CType(gc.View, TableView)
				If gc.GetRowVisibleIndexByHandle(v.FocusedRowHandle) = 0 Then
					MovePrevRow()
					Focus()
					e.Handled = True
					Return
				End If
				v.MovePrevRow()
				v.Focus()
				e.Handled = True
				Return
			End If
		End Sub
	End Class
End Namespace
