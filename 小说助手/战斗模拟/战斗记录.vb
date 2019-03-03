Imports 小说助手.数据类型

Namespace 战斗模拟
	Class 统计条目
		Implements I界面条目, I回合条目
		Protected Friend 键 As I有名称, 值 As UShort
		Sub New()
		End Sub
		Sub New(键 As I有名称, 值 As UShort)
			Me.键 = 键
			Me.值 = 值
		End Sub

		Private Property I统计树节点_键 As I界面成员 Implements I统计树节点(Of I界面成员, UShort).键
			Get
				Return 键
			End Get
			Set(value As I界面成员)
				键 = value
			End Set
		End Property

		Private Property I统计树节点_值 As UShort Implements I统计树节点(Of I界面成员, UShort).值
			Get
				Return 值
			End Get
			Set(value As UShort)
				值 = value
			End Set
		End Property

		Private Property I统计树节点_键1 As I回合成员 Implements I统计树节点(Of I回合成员, UShort).键
			Get
				Return 键
			End Get
			Set(value As I回合成员)
				键 = value
			End Set
		End Property

		Private Property I统计树节点_值1 As UShort Implements I统计树节点(Of I回合成员, UShort).值
			Get
				Return 值
			End Get
			Set(value As UShort)
				值 = value
			End Set
		End Property

		Public Function CompareTo(other As I界面条目) As Integer Implements IComparable(Of I界面条目).CompareTo
			Return CInt(other.值) - 值
		End Function

		Overrides Function ToString() As String Implements IToString.ToString
			Return 键.名称 & "，总伤害：" & 值
		End Function
	End Class
	Class 伤害统计
		Inherits 统计条目
		Implements I界面统计, I战场统计, I回合统计, I成员统计
		ReadOnly i统计表 As New Dictionary(Of I有名称, 统计条目)
		Sub New()
		End Sub
		Sub New(键 As I有名称)
			MyBase.New(键, 0)
		End Sub

		Public Property 死 As Boolean = False Implements I回合统计.死

		Private ReadOnly Property I回合统计_统计表 As IReadOnlyCollection(Of I回合条目) Implements I回合统计.统计表
			Get
				Return i统计表.Values
			End Get
		End Property

		Private Property I统计树节点_键 As I战场成员 Implements I统计树节点(Of I战场成员, UShort).键
			Get
				Return 键
			End Get
			Set(value As I战场成员)
				键 = value
			End Set
		End Property

		Private Property I统计树节点_值 As UShort Implements I统计树节点(Of I战场成员, UShort).值
			Get
				Return 值
			End Get
			Set(value As UShort)
				值 = value
			End Set
		End Property
		''' <summary>
		''' 不会检查两个对象的键是否是同一个成员，由调用方负责检查。如果不一致，舍弃参数的键
		''' </summary>
		''' <param name="合并项"></param>
		Public Sub 合并(合并项 As I回合统计) Implements I回合统计.合并
			For Each a As 统计条目 In DirectCast(合并项, 伤害统计).i统计表.Values
				If i统计表.ContainsKey(a.键) Then
					i统计表(a.键).值 += a.值
				Else
					i统计表.Add(a.键, a)
				End If
				值 += a.值
			Next
		End Sub
		Public Sub 添加条目(目标 As 成员, 伤害 As UShort) Implements I成员统计.添加条目
			If i统计表.ContainsKey(目标) Then
				i统计表(目标).值 += 伤害
			Else
				i统计表.Add(目标, New 统计条目(目标, 伤害))
			End If
			值 += 伤害
		End Sub

		Public Function 添加条目(目标 As I回合成员, 伤害 As UShort) As I回合统计 Implements I回合统计.添加条目
			If i统计表.ContainsKey(目标) Then
				i统计表(目标).值 += 伤害
			Else
				i统计表.Add(目标, New 统计条目(目标, 伤害))
			End If
			值 += 伤害
			Return Me
		End Function
		Public Overrides Function ToString() As String
			Return MyBase.ToString() & If(死, "，被杀", "")
		End Function

		Public Function Get统计表() As IReadOnlyList(Of I界面条目) Implements I界面统计.Get统计表
			Dim a As New List(Of I界面条目)
			For Each b As I界面条目 In i统计表.Values
				a.Add(b)
			Next
			Return a
		End Function
	End Class
End Namespace