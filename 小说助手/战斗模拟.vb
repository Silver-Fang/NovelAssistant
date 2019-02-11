Imports 小说助手.超级列表

Namespace 战斗模拟
	MustInherit Class 战斗单位
		Implements INotifyPropertyChanged, I改名提醒

		Private i名称 As String

		Property 名称 As String
			Get
				Return i名称
			End Get
			Set(value As String)
				i名称 = value
				RaiseEvent 名称更改(Me)
				OnPropertyChanged("名称")
			End Set
		End Property
		ReadOnly Property 名称Binding As New 转换Binding(Me, "名称")
		Protected 有效 As Boolean
		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
		Public Event 名称更改(sender As I改名提醒) Implements I改名提醒.名称更改
		MustOverride ReadOnly Property 战力 As Single
		ReadOnly Property 整数战力 As ULong
			Get
				Return 战力
			End Get
		End Property
		ReadOnly Property 整数战力Binding As New 转换Binding(Me, "整数战力", GetType(ULong), BindingMode.OneWay)
		Sub New()
			有效 = False
		End Sub
		Overrides Function ToString() As String
			Return 名称
		End Function
		Friend Sub OnPropertyChanged(propertyName As String)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End Sub
		Friend Sub 战力改变()
			OnPropertyChanged("战力")
			OnPropertyChanged("整数战力")
		End Sub
	End Class
	Class 成员
		Inherits 战斗单位
		Private i所属团队 As 团队
		Private i攻击 As UShort
		Private i防御 As UShort
		Private i精准 As UShort
		Private i闪避 As UShort
		Private i生命 As ULong
		Private Shadows Sub 战力改变()
			MyBase.战力改变()
			If i所属团队 IsNot Nothing Then i所属团队.战力改变()
		End Sub
		Property 攻击 As UShort
			Get
				Return i攻击
			End Get
			Set(value As UShort)
				i攻击 = value
				OnPropertyChanged("攻击")
				战力改变()
			End Set
		End Property
		ReadOnly Property 攻击Binding As New 转换Binding(Me, "攻击", GetType(UShort))
		Property 防御 As UShort
			Get
				Return i防御
			End Get
			Set(value As UShort)
				i防御 = value
				OnPropertyChanged("防御")
			End Set
		End Property
		ReadOnly Property 防御Binding As New 转换Binding(Me, "防御", GetType(UShort))
		Property 精准 As UShort
			Get
				Return i精准
			End Get
			Set(value As UShort)
				i精准 = value
				OnPropertyChanged("精准")
				战力改变()
			End Set
		End Property
		ReadOnly Property 精准Binding As New 转换Binding(Me, "精准", GetType(UShort))
		Property 闪避 As UShort
			Get
				Return i闪避
			End Get
			Set(value As UShort)
				i闪避 = value
				OnPropertyChanged("闪避")
				战力改变()
			End Set
		End Property
		ReadOnly Property 闪避Binding As New 转换Binding(Me, "闪避", GetType(UShort))
		Property 生命 As ULong
			Get
				Return i生命
			End Get
			Set(value As ULong)
				i生命 = value
				OnPropertyChanged("生命")
				战力改变()
			End Set
		End Property
		ReadOnly Property 生命Binding As New 转换Binding(Me, "生命", GetType(ULong))
		Overrides ReadOnly Property 战力 As Single
			Get
				If 生命 = 0 Then
					Return 0
				Else
					Return Math.Sqrt((CType(攻击, Single) * 精准 + 1) * (闪避 * 生命 + 1))
				End If
			End Get
		End Property
		Property 所属团队 As 团队
			Get
				Return i所属团队
			End Get
			Set(value As 团队)
				If i所属团队 IsNot Nothing Then i所属团队.成员列表.Remove(Me)
				i所属团队 = value
				If value IsNot Nothing Then value.成员列表.Add(Me)
			End Set
		End Property
		ReadOnly Property 所属团队Binding As New 转换Binding(Me, "所属团队")
		Sub New(成员名 As String, 攻击 As UShort, 防御 As UShort, 精准 As UShort, 闪避 As UShort, 生命 As ULong, 所属团队 As 团队)
			名称 = 成员名
			i攻击 = 攻击
			i防御 = 防御
			i精准 = 精准
			i闪避 = 闪避
			i生命 = 生命
			i所属团队 = 所属团队
			所属团队.成员列表.Add(Me)
			有效 = True
		End Sub
		'Sub New()

		'End Sub
		Function 嘲讽() As Single
			Return (CType(攻击, Single) * 精准 + 1) / (闪避 * 生命 + 1)
		End Function
		Sub 复活(复活血量 As Byte, Optional 提醒战力变化 As Boolean = True)
			If 提醒战力变化 Then
				生命 = CULng(防御) * 复活血量
			Else
				i生命 = CULng(防御) * 复活血量
			End If
		End Sub
		Sub 死亡(Optional 提醒战力变化 As Boolean = True)
			If 提醒战力变化 Then
				生命 = 0
			Else
				i生命 = 0
			End If
		End Sub
	End Class
	Class 团队
		Inherits 战斗单位
		Private i默契度 As Byte
		Private WithEvents I成员列表 As New 超级列表.超级列表
		ReadOnly Property 默契度Binding As New 转换Binding(Me, "默契度", GetType(Byte))
		Property 默契度 As Byte
			Get
				Return i默契度
			End Get
			Set(value As Byte)
				i默契度 = value
				OnPropertyChanged("默契度")
				战力改变()
			End Set
		End Property
		ReadOnly Property 成员列表 As 超级列表.超级列表
			Get
				Return I成员列表
			End Get
		End Property
		Overrides ReadOnly Property 战力 As Single
			Get
				Dim b As ULong = 0, c As ULong = 0, e As ULong = 0
				For Each a As 成员 In 成员列表
					If a.生命 > 0 Then
						b += CUInt(a.攻击) * a.精准
						c += a.闪避 * a.生命 + 1
						For Each d As 成员 In 成员列表
							If a Is d Then
								e += (a.闪避 * a.生命 + 1) * d.攻击 * d.精准
							Else
								If d.生命 > 0 Then
									If a.嘲讽 > d.嘲讽 Then e += (a.闪避 * a.生命 + 1) * d.攻击 * d.精准
									If a.嘲讽 = d.嘲讽 Then e += ((a.闪避 * a.生命 + 1) * d.攻击 * d.精准) / 2
								End If
							End If
						Next
					End If
				Next
				If e = 0 Then Return 0
				Dim f As ULong = b * c
				Return Math.Sqrt((f / e) ^ (默契度 / 255) * e)
			End Get
		End Property
		Sub New(团队名 As String, 默契度 As Byte)
			名称 = 团队名
			i默契度 = 默契度
			有效 = True
		End Sub
		'Sub New()

		'End Sub
		Private Sub 成员变动() Handles I成员列表.CollectionChanged
			战力改变()
		End Sub

		Sub 复活(复活血量 As Byte, Optional 提醒战力改变 As Boolean = True)
			For Each a As 成员 In 成员列表
				a.复活(复活血量, False)
			Next
			If 提醒战力改变 Then 战力改变()
		End Sub
		Sub 全灭(Optional 提醒战力改变 As Boolean = True)
			For Each a As 成员 In 成员列表
				a.死亡(False)
			Next
			If 提醒战力改变 Then 战力改变()
		End Sub
	End Class
	Class 战场
		Implements INotifyPropertyChanged
		Private Sub OnPropertyChanged(propertyName)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End Sub
		ReadOnly Property 团队列表 As New 超级列表.超级列表
		ReadOnly Property 战斗记录 As New ObservableCollection(Of String)
		Private i复活血量 As Byte = 1
		Property 复活血量 As Byte
			Get
				Return i复活血量
			End Get
			Set(value As Byte)
				i复活血量 = value
				OnPropertyChanged("复活血量")
			End Set
		End Property
		ReadOnly Property 复活血量Binding As New 转换Binding(Me, "复活血量", GetType(Byte))
		Private i当前回合 As Byte = 1
		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Property 当前回合 As Byte
			Get
				Return i当前回合
			End Get
			Set(value As Byte)
				i当前回合 = value
				OnPropertyChanged("当前回合")
			End Set
		End Property
		ReadOnly Property 当前回合Binding As New 转换Binding(Me, "当前回合", GetType(Byte))
		Sub 全体复活()
			For Each a As 团队 In 团队列表
				a.复活(i复活血量, False)
			Next
		End Sub
		Sub 全体死亡()
			For Each a As 团队 In 团队列表
				a.全灭(False)
			Next
		End Sub
		Function 战一回合() As String
			Dim c As New List(Of 团队), d As New List(Of 成员)
			Static 团队存活 As Boolean, 样本团队 As 团队, 样本成员 As 成员
			For Each 样本团队 In 团队列表
				团队存活 = False
				For Each 样本成员 In 样本团队.成员列表
					If 样本成员.生命 > 0 Then
						d.Add(样本成员)
						团队存活 = True
					End If
				Next
				If 团队存活 Then c.Add(样本团队)
			Next
			Select Case c.Count
				Case 0
					Return "所有团队全军覆没"
				Case 1
					Return c(0).名称 & "胜利"
				Case Else
					Static 攻方成员 As New List(Of 成员), 守方成员 As New List(Of (成员, Single)), 总嘲讽 As Single, 实际嘲讽 As Single, 随机生成器 As New Random, 随机值 As Single, 攻击目标 As (成员, Single) = Nothing, 守方活人数 As Byte
					战斗记录.Add("第" & 当前回合 & "回合")
					当前回合 += 1
					For Each 样本团队 In 团队列表
						攻方成员.Clear()
						For Each 样本成员 In d
							If 样本成员.所属团队 Is 样本团队 AndAlso CUInt(样本成员.攻击) * 样本成员.精准 > 0 Then 攻方成员.Add(样本成员)
						Next
						守方成员.Clear()
						总嘲讽 = 0
						守方活人数 = 0
						For Each 样本成员 In d
							If 样本成员.所属团队 IsNot 样本团队 Then
								实际嘲讽 = 样本成员.嘲讽 ^ Math.Tan(Math.PI * (CShort(样本团队.默契度) - 样本成员.所属团队.默契度) / 511)
								守方成员.Add((样本成员, 实际嘲讽))
								总嘲讽 += 实际嘲讽
								If 样本成员.生命 > 0 Then 守方活人数 += 1
							End If
						Next
						For Each 样本成员 In 攻方成员
							Do
								随机值 = 随机生成器.NextDouble * 总嘲讽
								For Each 攻击目标 In 守方成员
									随机值 -= 攻击目标.Item2
									If 随机值 <= 0 Then Exit For
								Next
								If 随机生成器.NextDouble * (CType(样本成员.精准, Double) + 攻击目标.Item1.闪避) > 样本成员.精准 Then Exit Do
								If 攻击目标.Item1.生命 > 0 Then
									If 攻击目标.Item1.生命 > 样本成员.攻击 Then
										攻击目标.Item1.生命 -= 样本成员.攻击
										战斗记录.Add(样本成员.名称 & "攻击" & 攻击目标.Item1.名称 & "造成" & 样本成员.攻击 & "伤害")
									Else
										攻击目标.Item1.生命 = 0
										守方活人数 -= 1
										战斗记录.Add(样本成员.名称 & "攻击" & 攻击目标.Item1.名称 & "造成" & 样本成员.攻击 & "伤害，将其击杀")
										If 守方活人数 = 0 Then Exit For
									End If
								End If
							Loop
						Next
					Next
					Return Nothing
			End Select
		End Function
	End Class
End Namespace