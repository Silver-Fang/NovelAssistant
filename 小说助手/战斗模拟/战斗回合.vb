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
		Inherits I有名称, I有生命
	End Interface
	Class 战斗回合(Of T统计 As {New, I回合统计}, T文件伤害 As {New, I文件伤害})
		Implements I界面回合, I战场回合, I文件回合
		ReadOnly i输出统计 As New Dictionary(Of I回合成员, T统计), i受伤统计 As New Dictionary(Of I回合成员, T统计)

		Public Property 值 As UShort Implements I统计树节点(Of Byte, UShort).值

		Public Property 回合号 As Byte Implements I战场回合.回合号， I统计树节点(Of Byte, UShort).键, I文件回合.回合号

		Public Property 伤害条目 As IReadOnlyCollection(Of I文件伤害) Implements I文件回合.伤害条目
			Get
				Dim c As New Collection(Of I文件伤害)
				For Each a As T统计 In i输出统计.Values
					For Each b As I回合条目 In a.统计表
						c.Add(New T文件伤害 With {.攻方 = a.键, .守方 = b.键, .伤害 = b.值})
					Next
				Next
				Return c
			End Get
			Set(value As IReadOnlyCollection(Of I文件伤害))
				i输出统计.Clear()
				i受伤统计.Clear()
				值 = 0
				For Each a As I文件伤害 In value
					If i输出统计.ContainsKey(a.攻方) Then
						i输出统计(a.攻方).添加条目(a.守方, a.伤害)
					Else
						i输出统计.Add(a.攻方, New T统计 With {.键 = a.攻方}.添加条目(a.守方, a.伤害))
					End If
					If i受伤统计.ContainsKey(a.守方) Then
						i受伤统计(a.守方).添加条目(a.攻方, a.伤害)
					Else
						i受伤统计.Add(a.守方, New T统计 With {.键 = a.守方}.添加条目(a.攻方, a.伤害))
					End If
					值 += a.伤害
				Next
			End Set
		End Property

		Public Property 战死者 As IEnumerable(Of I文件成员) Implements I文件回合.战死者
			Get
				Dim a As New Collection(Of I文件成员)
				For Each b As I回合成员 In i受伤统计.Keys
					If i受伤统计(b).死 Then
						a.Add(b)
					End If
				Next
				Return a
			End Get
			Set(value As IEnumerable(Of I文件成员))
				For Each a As I回合成员 In value
					If i受伤统计.ContainsKey(a) Then
						i受伤统计(a).死 = True
					Else
						i受伤统计.Add(a, New T统计 With {.死 = True})
					End If
				Next
			End Set
		End Property

		Overrides Function ToString() As String Implements I统计树节点(Of Byte, UShort).ToString
			Return "第" & 回合号 & "回合，总伤害：" & 值
		End Function
		''' <summary>
		''' 
		''' </summary>
		''' <param name="统计">空的统计将不被添加</param>
		Public Sub 添加输出统计(统计 As IReadOnlyCollection(Of I战场统计)) Implements I战场回合.添加输出统计
			For Each a As T统计 In 统计
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
							i受伤统计.Add(b.键, New T统计 With {.键 = b.键}.添加条目(a.键, b.值))
						End If
					Next
					For Each b As T统计 In i受伤统计.Values
						If b.值 >= b.键.生命 Then b.死 = True
					Next
					值 += a.值
				End If
			Next
		End Sub

		Private Function I界面回合_Get受伤统计() As IReadOnlyList(Of I界面统计) Implements I界面回合.Get受伤统计
			Dim a As New List(Of I界面统计)
			For Each b As I界面统计 In i受伤统计.Values
				a.Add(b)
			Next
			Return a
		End Function

		Private Function I战场回合_Get受伤统计() As IReadOnlyCollection(Of I战场统计) Implements I战场回合.Get受伤统计
			Dim a As New Collection(Of I战场统计)
			For Each b As I战场统计 In i受伤统计.Values
				a.Add(b)
			Next
			Return a
		End Function

		Public Function Get输出统计() As IReadOnlyList(Of I界面统计) Implements I界面回合.Get输出统计
			Dim a As New List(Of I界面统计)
			For Each b As I界面统计 In i输出统计.Values
				a.Add(b)
			Next
			Return a
		End Function
	End Class
	Class 战斗回合
		Inherits 战斗回合(Of 伤害统计, 文件伤害)
	End Class
End Namespace