Imports 小说助手.数据类型
Namespace 战斗模拟
	Interface I团队成员
		Inherits I可复活, I有战力, I改名提醒
		Sub 死亡(Optional 提醒战力变化 As Boolean = True)
		Function 安排战斗计划(敌人 As IReadOnlyCollection(Of I团队成员)) As Object
		ReadOnly Property 存活 As Boolean
	End Interface
	Interface I有成员列表
		ReadOnly Property 成员列表 As 实时更新列表(Of I团队成员)
	End Interface
	Class 团队
		Inherits 战斗单位
		Implements I界面团队, I战场团队, I成员团队
		Sub New(名称 As String)
			Me.名称 = 名称
		End Sub
		Private WithEvents i成员列表 As New 实时更新列表(Of I团队成员)

		Public Overrides ReadOnly Property 战力 As Single
			Get
				战力 = 0
				For Each a As I团队成员 In i成员列表
					战力 += a.战力
				Next
			End Get
		End Property

		Public ReadOnly Property 存活 As Boolean Implements I战场团队.存活
			Get
				For Each a As I团队成员 In 成员列表
					If a.存活 Then Return True
				Next
				Return False
			End Get
		End Property

		Public ReadOnly Property 成员列表 As 实时更新列表(Of I团队成员) Implements I有成员列表.成员列表
			Get
				Return i成员列表
			End Get
		End Property

		Public Sub 全灭(Optional 提醒战力改变 As Boolean = True) Implements I界面团队.全灭
			For Each a As I团队成员 In 成员列表
				a.死亡(False)
			Next
			If 提醒战力改变 Then 战力改变()
		End Sub

		Public Overrides Sub 复活(复活血量 As Byte, Optional 提醒战力变化 As Boolean = True)
			For Each a As I团队成员 In 成员列表
				a.复活(复活血量, False)
			Next
			If 提醒战力变化 Then 战力改变()
		End Sub
		Public Function 安排战斗计划(敌队 As IReadOnlyCollection(Of I战场团队)) As IReadOnlyCollection(Of I战场统计) Implements I战场团队.安排战斗计划
			Dim a As New List(Of I团队成员)
			For Each b As 团队 In 敌队
				a.AddRange(b.i成员列表)
			Next
			Dim d As New Collection(Of I战场统计)
			For Each c As I团队成员 In i成员列表
				If c.存活 Then d.Add(c.安排战斗计划(a))
			Next
			Return d
		End Function
	End Class
End Namespace