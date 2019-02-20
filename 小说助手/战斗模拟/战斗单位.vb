Imports 小说助手.数据类型

Namespace 战斗模拟
	Interface I可复活
		Sub 复活(复活血量 As Byte, Optional 提醒战力变化 As Boolean = True)
	End Interface
	Interface I有战力
		ReadOnly Property 战力 As Single
	End Interface
	Interface I有名称
		ReadOnly Property 名称 As String
	End Interface
	MustInherit Class 战斗单位
		Implements I界面战斗单位, I有名称, I可复活, I有战力
		Private i名称 As String
		Protected Sub OnPropertyChanged(propertyName As String)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End Sub
		Public ReadOnly Property 名称Binding As New 转换Binding(Me, "名称") Implements I界面战斗单位.名称Binding
		ReadOnly Property 整数战力 As ULong
			Get
				Return 战力
			End Get
		End Property
		Public ReadOnly Property 整数战力Binding As New 转换Binding(Me, "整数战力", GetType(ULong), BindingMode.OneWay) Implements I界面战斗单位.整数战力Binding

		Public Property 名称 As String Implements I有名称.名称
			Get
				Return i名称
			End Get
			Set(value As String)
				i名称 = value
				RaiseEvent 名称更改(Me)
				OnPropertyChanged("名称")
			End Set
		End Property

		Public MustOverride ReadOnly Property 战力 As Single Implements I有战力.战力

		Public Event 名称更改(sender As I改名提醒) Implements I改名提醒.名称更改
		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Public Sub 战力改变() Implements I界面战斗单位.战力改变
			OnPropertyChanged("战力")
			OnPropertyChanged("整数战力")
		End Sub

		MustOverride Sub 复活(复活血量 As Byte, Optional 提醒战力变化 As Boolean = True) Implements I可复活.复活
	End Class
End Namespace