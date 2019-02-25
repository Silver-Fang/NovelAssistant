Imports 小说助手.数据类型
Namespace 战斗模拟
	Interface I战场成员
		Sub 结算伤害(伤害 As UShort)
	End Interface
	Interface I战场统计
		Inherits I统计树节点(Of I战场成员, UShort)
	End Interface
	Interface I战场团队
		Inherits I有名称, I可复活, I有成员列表
		ReadOnly Property 存活 As Boolean
		Function 安排战斗计划(敌队 As IReadOnlyCollection(Of I战场团队)) As IReadOnlyCollection(Of I战场统计)
	End Interface
	Interface I战场回合
		Sub 添加输出统计(统计 As IReadOnlyCollection(Of I战场统计))
		Function Get受伤统计() As IList(Of I战场统计)
	End Interface
	''' <summary>
	''' 如果你使用自定义的回合类，必须重写“新回合”方法。
	''' </summary>
	Class 战场
		Implements I界面战场
		Private i复活血量 As Byte = 1, i当前回合 As Byte = 0
		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
		Private Sub OnPropertyChanged(propertyName)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End Sub

		Public ReadOnly Property 复活血量Binding As New 转换Binding(Me, "复活血量", GetType(Byte)) Implements I界面战场.复活血量Binding

		Public ReadOnly Property 当前回合Binding As New 转换Binding(Me, "当前回合", GetType(Byte)) Implements I界面战场.当前回合Binding

		Public ReadOnly Property 回合记录 As New ObservableCollection(Of I界面回合) Implements I界面战场.回合记录

		Public Property 复活血量 As Byte Implements I界面战场.复活血量
			Get
				Return i复活血量
			End Get
			Set(value As Byte)
				i复活血量 = value
				OnPropertyChanged("复活血量")
			End Set
		End Property

		Public Property 当前回合 As Byte Implements I界面战场.当前回合
			Get
				Return i当前回合
			End Get
			Set(value As Byte)
				i当前回合 = value
				OnPropertyChanged("当前回合")
			End Set
		End Property

		Public ReadOnly Property 团队列表 As New 实时更新列表(Of I界面团队) Implements I界面战场.团队列表

		Public Sub 全体复活() Implements I界面战场.全体复活
			For Each a As I战场团队 In 团队列表
				a.复活(i复活血量, False)
			Next
		End Sub
		Private Function 生成战斗结果(存活团队 As IEnumerable(Of I战场团队)) As String
			Select Case 存活团队.Count
				Case 0
					Return "所有团队同归于尽"
				Case 1
					Return 存活团队(0).名称 & "胜利"
				Case Else
					Return Nothing
			End Select
		End Function
		''' <summary>
		''' 欲使用自定义的回合类与此战场配合工作，必须重写此方法。
		''' </summary>
		''' <param name="回合号"></param>
		''' <returns></returns>
		Protected Overridable Function 新回合(回合号 As Byte) As I战场回合
			Return New 战斗回合(回合号)
		End Function
		''' <summary>
		''' 战场的核心战斗算法在此开始。进行一回合战斗，并输出一回合的结果。
		''' </summary>
		''' <param name="回合号">用于标识生成的回合记录</param>
		''' <returns>返回战斗结果。只要有>1个团队存活，说明战斗未结束，返回Nothing。1个团队存活，则该团队胜利；0个团队存活，则所有团队同归于尽。</returns>
		Public Function 战一回合(回合号 As Byte) As String Implements I界面战场.战一回合
			Dim d As I战场回合 = 新回合(回合号)
			当前回合 = 回合号
			'第一步，先要求每个存活的团队提交一份快照
			Dim i As New Collection(Of I战场团队)
			For Each a As I战场团队 In 团队列表
				If a.存活 Then
					i.Add(a)
				End If
			Next
			Dim h As String = 生成战斗结果(i)
			If h IsNot Nothing Then Return h
			'第二步，给每个团队发送所有敌队的快照，由其负责安排战斗计划，生成以其为输出者的伤害统计
			For Each c As I战场团队 In i.ToArray
				i.Remove(c)
				d.添加输出统计(c.安排战斗计划(i.ToArray))
				i.Add(c)
			Next
			'第三步，综合这些伤害统计，写入总的回合记录，并实际结算扣血
			回合记录.Add(d)
			For Each e As I战场统计 In d.Get受伤统计
				e.键.结算伤害(e.值)
			Next
			'第四步，如果战斗结束，返回战斗结果。
			Dim g As New Collection(Of I战场团队)
			For Each a As I战场团队 In 团队列表
				If a.存活 Then g.Add(a)
			Next
			Return 生成战斗结果(g)
		End Function

		Public Function 全场成员() As IReadOnlyCollection(Of I界面成员) Implements I界面战场.全场成员
			Dim a As New Collection(Of I界面成员)
			For Each b As I战场团队 In 团队列表
				For Each c As I战场成员 In b.成员列表
					a.Add(c)
				Next
			Next
			Return a
		End Function
	End Class
End Namespace