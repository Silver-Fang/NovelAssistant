Imports 小说助手.数据类型
Namespace 战斗模拟
	Interface I成员团队
		Inherits I有成员列表, I有战力
	End Interface
	Interface I成员统计
		Sub 添加条目(目标 As 成员, 伤害 As UShort)
	End Interface
	Interface I有生命
		Property 生命 As ULong
	End Interface
	Interface I攻防精闪等谋
		Property 攻击 As UShort
		Property 防御 As UShort
		Property 精准 As UShort
		Property 闪避 As UShort
		Property 等级 As Byte
		Property 谋略 As Byte
	End Interface
	Class 成员
		Inherits 战斗单位
		Implements I界面成员, I战场成员, I团队成员, I回合成员, I文件成员
		Private i所属团队 As I成员团队, i攻击 As UShort, i防御 As UShort, i精准 As UShort, i闪避 As UShort, i生命 As ULong, i等级 As Byte, i谋略 As Byte
		Sub New()
		End Sub
		Public Property 所属团队 As I界面团队 Implements I界面成员.所属团队
			Get
				Return i所属团队
			End Get
			Set(value As I界面团队)
				If i所属团队 IsNot Nothing Then i所属团队.成员列表.Remove(Me)
				i所属团队 = value
				If value IsNot Nothing Then value.成员列表.Add(Me)
			End Set
		End Property

		Public ReadOnly Property 所属团队Binding As New 转换Binding(Me, "所属团队") Implements I界面成员.所属团队Binding
		Property 攻击 As UShort Implements I文件成员.攻击
			Get
				Return i攻击
			End Get
			Set(value As UShort)
				i攻击 = value
				OnPropertyChanged("攻击")
				战力改变()
			End Set
		End Property

		Public ReadOnly Property 攻击Binding As New 转换Binding(Me, "攻击", GetType(UShort)) Implements I界面成员.攻击Binding
		Property 防御 As UShort Implements I文件成员.防御
			Get
				Return i防御
			End Get
			Set(value As UShort)
				i防御 = value
				OnPropertyChanged("防御")
			End Set
		End Property

		Public ReadOnly Property 防御Binding As New 转换Binding(Me, "防御", GetType(UShort)) Implements I界面成员.防御Binding
		Property 精准 As UShort Implements I文件成员.精准
			Get
				Return i精准
			End Get
			Set(value As UShort)
				i精准 = value
				OnPropertyChanged("精准")
				战力改变()
			End Set
		End Property

		Public ReadOnly Property 精准Binding As New 转换Binding(Me, "精准", GetType(UShort)) Implements I界面成员.精准Binding
		Property 闪避 As UShort Implements I文件成员.闪避
			Get
				Return i闪避
			End Get
			Set(value As UShort)
				i闪避 = value
				OnPropertyChanged("闪避")
				战力改变()
			End Set
		End Property

		Public ReadOnly Property 闪避Binding As New 转换Binding(Me, "闪避", GetType(UShort)) Implements I界面成员.闪避Binding

		Public ReadOnly Property 生命Binding As New 转换Binding(Me, "生命", GetType(ULong)) Implements I界面成员.生命Binding

		Public Property 等级 As Byte Implements I攻防精闪等谋.等级
			Get
				Return i等级
			End Get
			Set(value As Byte)
				i等级 = value
				OnPropertyChanged("等级")
			End Set
		End Property

		Public ReadOnly Property 等级Binding As New 转换Binding(Me, "等级", GetType(Byte)) Implements I界面成员.等级Binding

		Public ReadOnly Property 存活 As Boolean Implements I团队成员.存活
			Get
				Return i生命 > 0
			End Get
		End Property

		Public Overrides ReadOnly Property 战力 As Single Implements I团队成员.战力
			Get
				If 生命 = 0 Then
					Return 0
				Else
					Return Math.Sqrt((CType(攻击, Single) * 精准 + 1) * (闪避 * 生命 + 1))
				End If
			End Get
		End Property
		Property 谋略 As Byte Implements I文件成员.谋略
			Get
				Return i谋略
			End Get
			Set(value As Byte)
				i谋略 = value
				OnPropertyChanged("谋略")
			End Set
		End Property

		Public ReadOnly Property 谋略Binding As New 转换Binding(Me, "谋略", GetType(Byte)) Implements I界面成员.谋略Binding

		Public Property 生命 As ULong Implements I有生命.生命
			Get
				Return i生命
			End Get
			Set(value As ULong)
				i生命 = value
				OnPropertyChanged("生命")
				战力改变()
			End Set
		End Property

		Private Property I文件成员_所属团队 As I有名称 Implements I文件成员.所属团队
			Get
				Return i所属团队
			End Get
			Set(value As I有名称)
				If i所属团队 IsNot Nothing Then i所属团队.成员列表.Remove(Me)
				i所属团队 = value
				If value IsNot Nothing Then i所属团队.成员列表.Add(Me)
			End Set
		End Property

		Public Overrides Sub 复活(复活血量 As Byte, Optional 提醒战力变化 As Boolean = True)
			If 提醒战力变化 Then
				生命 = CULng(防御) * 复活血量
			Else
				i生命 = CULng(防御) * 复活血量
			End If
		End Sub

		Public Sub 结算伤害(伤害 As UShort) Implements I战场成员.结算伤害
			If 生命 > 伤害 Then
				生命 -= 伤害
			Else
				生命 = 0
			End If
		End Sub

		Public Sub 死亡(Optional 提醒战力变化 As Boolean = True) Implements I团队成员.死亡
			If 提醒战力变化 Then
				生命 = 0
			Else
				i生命 = 0
			End If
		End Sub

		Public Function 总能力点() As UShort Implements I界面成员.总能力点
			Return i攻击 + i防御 + i精准 + i闪避
		End Function
		Protected Overridable Function 新统计() As I成员统计
			Return New 伤害统计(Me)
		End Function
		Private Class 成员快照
			ReadOnly Property 闪避 As UShort
				Get
					Return 原成员.i闪避
				End Get
			End Property
			Private 生命 As ULong
			ReadOnly Property 原成员 As 成员
			Function 嘲讽(对方谋略 As Byte, 对方团队战力 As Single) As Single
				Static 固定嘲讽 As Single = 原成员.攻击 * 原成员.精准 * 原成员.i所属团队.战力 + 1
				If 生命 = 0 Then Return 0
				Dim a As Single = 固定嘲讽 / (闪避 * 生命 * 对方团队战力 + 1)
				Return (a + 1 / a) / 2 + (a - 1 / a) * (CShort(对方谋略) - 原成员.谋略) / Byte.MaxValue / 2
			End Function
			Sub 受伤(伤害 As UShort)
				If 伤害 > 生命 Then
					生命 = 0
				Else
					生命 -= 伤害
				End If
			End Sub
			Sub New(原成员 As 成员)
				Me.原成员 = 原成员
				生命 = 原成员.生命
			End Sub
		End Class
		Public Function 安排战斗计划(敌人 As IReadOnlyCollection(Of I团队成员)) As Object Implements I团队成员.安排战斗计划
			Dim a As I成员统计 = 新统计(), c As New 随机抽取列表(Of 成员快照), f = i所属团队.战力
			Static 样本成员 As 成员快照
			For Each b As 成员 In 敌人
				样本成员 = New 成员快照(b)
				c.添加(样本成员, 样本成员.嘲讽(i谋略, f))
			Next
			Dim e As New 随机抽取列表(Of Boolean)
			e(True) = i精准
			Do Until c.总权重 = 0
				样本成员 = c.抽取
				e(False) = 样本成员.闪避
				If e.抽取 Then
					样本成员.受伤(i攻击)
					a.添加条目(样本成员.原成员, i攻击)
					c(样本成员) = 样本成员.嘲讽(i谋略, f)
				Else
					Return a
				End If
			Loop
			Return a
		End Function
	End Class
End Namespace