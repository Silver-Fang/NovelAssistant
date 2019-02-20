Imports 小说助手.数据类型
Namespace 战斗模拟
	Interface I回合统计
		Inherits I统计树节点(Of Object, UShort)
		Sub 合并(合并项 As I回合统计)
	End Interface
	Interface IGet受伤统计
		Function Get受伤统计() As IList(Of I回合统计)
	End Interface
	Class 战斗回合
		Implements I界面回合, I战场回合
		ReadOnly i输出统计 As New Dictionary(Of I有名称, I回合统计), i受伤统计 As New Dictionary(Of I有名称, I回合统计)
		Public ReadOnly Property 键 As Byte Implements I统计树节点(Of Byte, UShort).键

		Public ReadOnly Property 值 As UShort Implements I统计树节点(Of Byte, UShort).值

		Public Function Get输出统计() As List(Of I界面统计) Implements I界面回合.Get输出统计
			Return i输出统计.Values.AsEnumerable
		End Function
		Sub New(回合号 As Byte)
			键 = 回合号
		End Sub

		Overrides Function ToString() As String Implements I统计树节点(Of Byte, UShort).ToString
			Return "第" & 键 & "回合，总伤害：" & 值
		End Function

		Public Sub 添加输出统计(统计 As IEnumerable(Of I战场统计)) Implements I战场回合.添加输出统计
			For Each a As I回合统计 In 统计
				If i输出统计.ContainsKey(a.键) Then
					i输出统计(a.键).合并(a)
				End If
			Next
		End Sub

		Public Function Get受伤统计() As IList(Of I回合统计) Implements IGet受伤统计.Get受伤统计
			Return i受伤统计.Values.ToList
		End Function
	End Class
End Namespace