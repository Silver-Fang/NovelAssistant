Imports 小说助手.数据类型
Namespace 战斗模拟
	Interface I回合统计
		Inherits I统计树节点(Of I回合成员, UShort)
		Sub 合并(合并项 As I回合统计)
		ReadOnly Property 统计表 As IReadOnlyCollection(Of I回合条目)
		Function 添加条目(目标 As I回合成员, 伤害 As UShort) As I回合统计
		Property 死 As Boolean
	End Interface
	Interface I回合条目
		Inherits I统计树节点(Of I回合成员, UShort)
	End Interface
	Interface I回合成员
		Inherits I有名称
		ReadOnly Property 生命 As ULong
	End Interface
	Class 战斗回合
		Implements I界面回合, I战场回合
		ReadOnly i输出统计 As New Dictionary(Of I回合成员, I回合统计), i受伤统计 As New Dictionary(Of I回合成员, I回合统计)
		Public ReadOnly Property 键 As Byte Implements I统计树节点(Of Byte, UShort).键

		Public Property 值 As UShort Implements I统计树节点(Of Byte, UShort).值

		Public Function Get输出统计() As List(Of I界面统计) Implements I界面回合.Get输出统计
			Get输出统计 = New List(Of I界面统计)
			For Each a As I回合统计 In i输出统计.Values
				Get输出统计.Add(a)
			Next
		End Function
		Sub New(回合号 As Byte)
			键 = 回合号
		End Sub

		Overrides Function ToString() As String Implements I统计树节点(Of Byte, UShort).ToString
			Return "第" & 键 & "回合，总伤害：" & 值
		End Function
		Protected Overridable Function 新统计(键 As I回合成员) As I回合统计
			Return New 伤害统计(键)
		End Function
		''' <summary>
		''' 
		''' </summary>
		''' <param name="统计">空的统计将不被添加</param>
		Public Sub 添加输出统计(统计 As IReadOnlyCollection(Of I战场统计)) Implements I战场回合.添加输出统计
			For Each a As I回合统计 In 统计
				If a.值 > 0 Then
					If i输出统计.ContainsKey(a.键) Then
						i输出统计(a.键).合并(a)
					Else
						i输出统计.Add(a.键, a)
					End If
					For Each b As I回合条目 In a.统计表
						If i受伤统计.ContainsKey(b.键) Then
							i受伤统计(b.键).添加条目(a.键, b.值)
						Else
							i受伤统计.Add(b.键, 新统计(b.键).添加条目(a.键, b.值))
						End If
					Next
					For Each b As I回合统计 In i受伤统计.Values
						If b.值 >= b.键.生命 Then b.死 = True
					Next
					值 += a.值
				End If
			Next
		End Sub

		Private Function I界面回合_Get受伤统计() As IList(Of I界面统计) Implements I界面回合.Get受伤统计
			I界面回合_Get受伤统计 = New List(Of I界面统计)
			For Each a As I回合统计 In i受伤统计.Values
				I界面回合_Get受伤统计.Add(a)
			Next
		End Function

		Private Function I战场回合_Get受伤统计() As IList(Of I战场统计) Implements I战场回合.Get受伤统计
			I战场回合_Get受伤统计 = New List(Of I战场统计)
			For Each a As I回合统计 In i受伤统计.Values
				I战场回合_Get受伤统计.Add(a)
			Next
		End Function
	End Class
End Namespace