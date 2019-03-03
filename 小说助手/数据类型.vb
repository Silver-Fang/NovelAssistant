Imports System.Collections.Specialized

Namespace 数据类型
	Public Interface I改名提醒
		Event 名称更改(sender As I改名提醒)
	End Interface
	Class 实时更新列表
		Inherits ObservableCollection(Of I改名提醒)
		Sub 成员改名(哪个成员 As I改名提醒)
			'RemoveHandler CollectionChanged, AddressOf 成员增减
			SetItem(IndexOf(哪个成员), 哪个成员)
			'AddHandler CollectionChanged, AddressOf 成员增减
		End Sub

		Private Sub 成员增减(sender As Object, e As NotifyCollectionChangedEventArgs) Handles Me.CollectionChanged
			Static 成员改名处理 As I改名提醒.名称更改EventHandler = AddressOf 成员改名
			Select Case e.Action
				Case NotifyCollectionChangedAction.Add
					For Each a As I改名提醒 In e.NewItems
						AddHandler a.名称更改, 成员改名处理
					Next
				Case NotifyCollectionChangedAction.Remove
					For Each a As I改名提醒 In e.OldItems
						RemoveHandler a.名称更改, 成员改名处理
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
	Interface IToString
		Function ToString() As String
	End Interface
	Interface I统计树节点(Of TKey, TValue)
		Inherits IToString
		Property 键 As TKey
		Property 值 As TValue
	End Interface
		''' <summary>
		''' 这是一个增加了随机抽取功能的字典。键是待抽取项，值是权重。权重越高越容易抽中。
		''' </summary>
		''' <typeparam name="TKey">待抽取项的类型</typeparam>
		Class 随机抽取列表(Of TKey)
			ReadOnly 权重字典 As New Dictionary(Of TKey, Decimal)
			Private i总权重 As Decimal
			ReadOnly Property 总权重 As Decimal
				Get
					Return i总权重
				End Get
			End Property
			Sub 添加(随机项 As TKey, 权重 As Decimal)
				If 权重 < 0 Then
					Throw New ArgumentOutOfRangeException("权重", 权重, "权重不能为负")
				Else
					权重字典.Add(随机项, 权重)
					i总权重 += 权重
				End If
			End Sub
			Default Property 权重(随机项 As TKey) As Decimal
				Get
					Return 权重字典(随机项)
				End Get
				Set(value As Decimal)
					If value < 0 Then
						Throw New ArgumentOutOfRangeException("权重", value, "权重不能为负")
					Else
						If 权重字典.ContainsKey(随机项) Then i总权重 -= 权重字典(随机项)
						权重字典(随机项) = value
						i总权重 += value
					End If
				End Set
			End Property
			Sub 移除(随机项 As TKey)
				If 权重字典.ContainsKey(随机项) Then i总权重 -= 权重字典(随机项)
				权重字典.Remove(随机项)
			End Sub
			Function 抽取() As TKey
				If i总权重 = 0 Then Throw New InvalidOperationException("所有项的权重皆为0，无法抽取")
				Static 随机生成器 As New Random
				Dim a As Decimal = 总权重 * 随机生成器.NextDouble
				For Each b As TKey In 权重字典.Keys
					If 权重字典(b) > a Then
						Return b
					Else
						a -= 权重字典(b)
					End If
				Next
				Throw New Exception("意外错误")
			End Function
		End Class
End Namespace