Imports System.Collections.Specialized

Namespace 超级列表
	Public Interface I改名提醒
		Event 名称更改(sender As I改名提醒)
	End Interface
	Public Class 超级列表
		Inherits ObservableCollection(Of I改名提醒)
		Sub 成员改名(哪个成员 As I改名提醒)
			'RemoveHandler CollectionChanged, AddressOf 成员增减
			SetItem(IndexOf(哪个成员), 哪个成员)
			'AddHandler CollectionChanged, AddressOf 成员增减
		End Sub

		Private Sub 成员增减(sender As Object, e As NotifyCollectionChangedEventArgs) Handles Me.CollectionChanged
			Select Case e.Action
				Case NotifyCollectionChangedAction.Add
					For Each a As I改名提醒 In e.NewItems
						AddHandler a.名称更改, AddressOf 成员改名
					Next
				Case NotifyCollectionChangedAction.Remove
					For Each a As I改名提醒 In e.OldItems
						RemoveHandler a.名称更改, AddressOf 成员改名
					Next
			End Select
		End Sub
	End Class
	Class 转换Binding
		Inherits Binding
		Private Class String转ValueType
			Implements IValueConverter
			Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
				Return value
			End Function

			Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
				Try
					Select Case parameter
						Case GetType(Byte)
							Return Byte.Parse(value)
						Case GetType(UShort)
							Return UShort.Parse(value)
						Case GetType(UInteger)
							Return UInteger.Parse(value)
						Case GetType(ULong)
							Return ULong.Parse(value)
					End Select
				Catch ex As FormatException
				Catch ex As OverflowException
				End Try
				Return value
			End Function
		End Class

		Private Shared ReadOnly 转换器 As New String转ValueType
		Sub New(源 As Object, 属性名 As String, Optional 属性类型 As Type = Nothing, Optional 模式 As BindingMode = BindingMode.TwoWay)
			Source = 源
			Path = New PropertyPath(属性名)
			Mode = 模式
			If 属性类型 IsNot Nothing Then
				Converter = 转换器
				ConverterParameter = 属性类型
			End If
		End Sub
	End Class
End Namespace