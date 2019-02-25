﻿Imports Windows.Storage, 小说助手.数据类型, 小说助手.人物设定
Namespace 战斗模拟
	Interface I界面条目
		Inherits IComparable(Of I界面条目), I统计树节点(Of I界面成员, UShort)
	End Interface
	Interface I界面统计
		Inherits I界面条目
		ReadOnly Property 统计表() As IReadOnlyList(Of I界面条目)
	End Interface
	Interface I界面回合
		Inherits I统计树节点(Of Byte, UShort)
		Function Get输出统计() As List(Of I界面统计)
		Function Get受伤统计() As IList(Of I界面统计)
	End Interface
	''' <summary>
	''' 要求INotifyPropertyChanged保证实时数据绑定，相关方法由框架代码调用
	''' </summary>
	Interface I界面战场
		Inherits INotifyPropertyChanged
		ReadOnly Property 团队列表 As 实时更新列表(Of I界面团队)
		ReadOnly Property 复活血量Binding As 转换Binding
		ReadOnly Property 当前回合Binding As 转换Binding
		ReadOnly Property 回合记录 As ObservableCollection(Of I界面回合)
		Property 复活血量 As Byte
		Sub 全体复活()
		Property 当前回合 As Byte
		Function 战一回合(回合号 As Byte) As String
		Function 全场成员() As IReadOnlyCollection(Of I界面成员)
	End Interface
	Interface I界面战斗单位
		Inherits I改名提醒, INotifyPropertyChanged, IToString
		ReadOnly Property 名称Binding As 转换Binding
		ReadOnly Property 整数战力Binding As 转换Binding
		Sub 战力改变()
	End Interface
	Interface I界面团队
		Inherits I界面战斗单位, I可复活, I有成员列表
		Sub 全灭(Optional 提醒战力改变 As Boolean = True)
	End Interface
	Interface I界面成员
		Inherits I界面战斗单位, I有名称
		Property 所属团队 As I界面团队
		ReadOnly Property 所属团队Binding As 转换Binding
		ReadOnly Property 攻击Binding As 转换Binding
		ReadOnly Property 防御Binding As 转换Binding
		ReadOnly Property 精准Binding As 转换Binding
		ReadOnly Property 闪避Binding As 转换Binding
		ReadOnly Property 生命Binding As 转换Binding
		ReadOnly Property 等级Binding As 转换Binding
		ReadOnly Property 谋略Binding As 转换Binding
		ReadOnly Property 等级 As Byte
		Function 总能力点() As UShort
	End Interface
	Interface I界面人物
		ReadOnly Property 名字 As String
		Function 总系数() As Byte
		Property 等级 As Byte
		Property 攻击系数 As Byte
		Property 防御系数 As Byte
		Property 精准系数 As Byte
		Property 闪避系数 As Byte
		Property 谋略 As Byte
	End Interface
	''' <summary>
	''' 如果想要使用自定义的团队、战场或成员与此界面配合工作，需要继承此类并重写新战场、新成员、新团队三个函数。
	''' </summary>
	Class 用户界面
		Implements I响应删除键
		ReadOnly 战场 As I界面战场, 所属团队 As Selector, 团队名 As TextBox, 成员名 As TextBox, 攻击 As TextBox, 防御 As TextBox, 精准 As TextBox, 闪避 As TextBox, 生命 As TextBox, 团队战力 As FrameworkElement, 个人战力 As FrameworkElement, 等级 As TextBox, 谋略 As TextBox, 回合数 As TextBlock, 回合总伤害 As TextBlock, 输出人 As TextBlock, 个人总输出 As TextBlock, 受伤人 As TextBlock, 个人总受伤 As TextBlock, 输出目标排行 As ItemsControl, 伤害来源排行 As ItemsControl
		Private 删除焦点 As Selector
		WithEvents 团队列表 As Selector, 创建团队 As ButtonBase, 删除团队 As ButtonBase, 删除成员 As ButtonBase, 添加成员 As ButtonBase, 成员列表 As Selector, 成员复活 As ButtonBase, 团队复活 As ButtonBase, 全体复活 As ButtonBase, 清空记录 As ButtonBase, 战一回合 As ButtonBase， 不死不休 As ButtonBase, 成员死亡 As ButtonBase, 团队全灭 As ButtonBase, 载入人物 As ButtonBase, 回合记录 As Selector, 输出排行 As Selector, 受伤排行 As Selector
		''' <summary>
		''' 如果想要使用自定义的战场与此界面配合工作，必须重写此方法。
		''' </summary>
		''' <returns></returns>
		Protected Overridable Function 新战场() As I界面战场
			Return New 战场
		End Function
		Sub New(团队列表 As Selector, 所属团队 As Selector, 复活血量 As FrameworkElement, 当前回合 As FrameworkElement, 创建团队 As ButtonBase, 团队名 As TextBox, 删除团队 As ButtonBase, 删除成员 As ButtonBase, 成员列表 As Selector, 添加成员 As ButtonBase, 成员名 As TextBox, 攻击 As TextBox, 防御 As TextBox, 精准 As TextBox, 闪避 As TextBox, 生命 As TextBox, 团队战力 As FrameworkElement, 个人战力 As FrameworkElement, 成员复活 As ButtonBase, 团队复活 As ButtonBase, 全体复活 As ButtonBase, 清空记录 As ButtonBase, 战一回合 As ButtonBase, 不死不休 As ButtonBase, 成员死亡 As ButtonBase, 团队全灭 As ButtonBase, 载入人物 As ButtonBase, 等级 As TextBox, 谋略 As TextBox, 回合数 As TextBlock, 回合总伤害 As TextBlock, 输出人 As TextBlock, 个人总输出 As TextBlock, 受伤人 As TextBlock, 个人总受伤 As TextBlock, 输出目标排行 As ItemsControl, 伤害来源排行 As ItemsControl, 回合记录 As Selector, 输出排行 As Selector, 受伤排行 As Selector)
			Me.团队列表 = 团队列表
			Me.所属团队 = 所属团队
			Me.创建团队 = 创建团队
			Me.团队名 = 团队名
			Me.删除团队 = 删除团队
			Me.删除成员 = 删除成员
			Me.成员列表 = 成员列表
			Me.添加成员 = 添加成员
			Me.成员名 = 成员名
			Me.攻击 = 攻击
			Me.防御 = 防御
			Me.精准 = 精准
			Me.闪避 = 闪避
			Me.生命 = 生命
			Me.团队战力 = 团队战力
			Me.个人战力 = 个人战力
			Me.成员复活 = 成员复活
			Me.团队复活 = 团队复活
			Me.全体复活 = 全体复活
			Me.清空记录 = 清空记录
			Me.战一回合 = 战一回合
			Me.不死不休 = 不死不休
			Me.成员死亡 = 成员死亡
			Me.团队全灭 = 团队全灭
			Me.载入人物 = 载入人物
			Me.等级 = 等级
			Me.谋略 = 谋略
			Me.战场 = If(战场, 新战场())
			Me.回合数 = 回合数
			Me.回合总伤害 = 回合总伤害
			Me.输出人 = 输出人
			Me.个人总输出 = 个人总输出
			Me.受伤人 = 受伤人
			Me.个人总受伤 = 个人总受伤
			Me.输出目标排行 = 输出目标排行
			Me.伤害来源排行 = 伤害来源排行
			Me.输出排行 = 输出排行
			Me.受伤排行 = 受伤排行
			Me.回合记录 = 回合记录
			战场 = 新战场()
			团队列表.ItemsSource = 战场.团队列表
			所属团队.ItemsSource = 战场.团队列表
			复活血量.SetBinding(TextBox.TextProperty, 战场.复活血量Binding)
			当前回合.SetBinding(TextBox.TextProperty, 战场.当前回合Binding)
			回合记录.ItemsSource = 战场.回合记录
		End Sub
		''' <summary>
		''' 派生类可以重写此方法以将自定义的I界面团队类型载入界面
		''' </summary>
		''' <param name="团队名"></param>
		''' <returns></returns>
		Protected Overridable Function 新团队(团队名 As String) As I界面团队
			Return New 团队(团队名)
		End Function
		Private Sub 创建团队_Click(sender As Object, e As RoutedEventArgs) Handles 创建团队.Click
			战场.团队列表.Add(新团队(团队名.Text))
		End Sub
		Private Sub 删除团队_Click() Handles 删除团队.Click
			战场.团队列表.Remove(团队列表.SelectedItem)
		End Sub
		Private Sub 删除成员_Click() Handles 删除成员.Click
			Dim a As I界面成员 = 成员列表.SelectedItem
			If a IsNot Nothing Then a.所属团队.成员列表.Remove(a)
		End Sub
		''' <summary>		
		''' 派生类可以重写此方法以将自定义的I界面成员类型载入界面。
		''' </summary>
		''' <param name="成员名"></param>
		''' <param name="攻击"></param>
		''' <param name="防御"></param>
		''' <param name="精准"></param>
		''' <param name="闪避"></param>
		''' <param name="生命"></param>
		''' <param name="所属团队">注意：将成员加入团队需要由成员自己实现，界面只是单纯创建成员，不会负责将其加入团队。</param>
		''' <param name="等级"></param>
		''' <param name="谋略"></param>
		''' <returns></returns>
		Protected Overridable Function 新成员(成员名 As String, 攻击 As UShort, 防御 As UShort, 精准 As UShort, 闪避 As UShort, 生命 As ULong, 所属团队 As I界面团队, 等级 As Byte, 谋略 As Byte) As I界面成员
			Return New 成员(成员名, 攻击, 防御, 精准, 闪避, 生命, 所属团队, 等级, 谋略)
		End Function
		Private Sub 添加成员_Click(sender As Object, e As RoutedEventArgs) Handles 添加成员.Click
			Dim a As I界面团队 = 所属团队.SelectedItem
			If a Is Nothing Then
				Static 未选团队 As New Flyout With {.Content = New TextBlock With {.Text = "必须选择所属团队"}}
				未选团队.ShowAt(所属团队)
			Else
				Static 错误提示 As New Flyout With {.Content = New TextBlock With {.Text = "攻击、防御、精准、闪避必须在0~32767之间，生命必须在0~2147483647之间，等级、谋略必须在0~255之间"}}
				Try
					新成员(成员名.Text, 攻击.Text, 防御.Text, 精准.Text, 闪避.Text, 生命.Text, a, 等级.Text, 谋略.Text)
				Catch ex As OverflowException
					错误提示.ShowAt(sender)
					Exit Sub
				Catch ex As InvalidCastException
					错误提示.ShowAt(sender)
					Exit Sub
				End Try
			End If
			团队列表.SelectedItem = a
		End Sub
		Private Sub 改变当前团队(当前团队 As I界面团队)
			With 当前团队
				团队名.SetBinding(TextBox.TextProperty, .名称Binding)
				成员列表.ItemsSource = .成员列表
				团队战力.SetBinding(TextBlock.TextProperty, .整数战力Binding)
			End With
		End Sub
		Private Sub 改变当前成员(当前成员 As I界面成员)
			With 当前成员
				成员名.SetBinding(TextBox.TextProperty, .名称Binding)
				所属团队.SetBinding(Selector.SelectedItemProperty, .所属团队Binding)
				攻击.SetBinding(TextBox.TextProperty, .攻击Binding)
				防御.SetBinding(TextBox.TextProperty, .防御Binding)
				精准.SetBinding(TextBox.TextProperty, .精准Binding)
				闪避.SetBinding(TextBox.TextProperty, .闪避Binding)
				生命.SetBinding(TextBox.TextProperty, .生命Binding)
				等级.SetBinding(TextBox.TextProperty, .等级Binding)
				谋略.SetBinding(TextBox.TextProperty, .谋略Binding)
				个人战力.SetBinding(TextBlock.TextProperty, .整数战力Binding)
			End With
		End Sub
		Private Sub 团队列表_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 团队列表.SelectionChanged
			If 团队列表.SelectedItem IsNot Nothing Then 改变当前团队(团队列表.SelectedItem)
		End Sub
		Private Sub 成员列表_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 成员列表.SelectionChanged
			If 成员列表.SelectedItem IsNot Nothing Then 改变当前成员(成员列表.SelectedItem)
		End Sub
		Private Sub 成员复活_Click(sender As Object, e As RoutedEventArgs) Handles 成员复活.Click
			Static 错误提示 As New Flyout With {.Content = New TextBlock With {.Text = "防御必须在0~32767之间"}}
			Try
				生命.Text = 防御.Text * 战场.复活血量
			Catch ex As InvalidCastException
				错误提示.ShowAt(防御)
			Catch ex As OverflowException
				错误提示.ShowAt(防御)
			End Try
		End Sub
		Private Sub 团队复活_Click(sender As Object, e As RoutedEventArgs) Handles 团队复活.Click
			Dim a As I界面团队 = 团队列表.SelectedItem
			If a IsNot Nothing Then
				a.复活(战场.复活血量)
				Dim b As I界面成员 = 成员列表.SelectedItem
				If b IsNot Nothing Then b.战力改变()
			End If
		End Sub
		Private Sub 全体复活_Click(sender As Object, e As RoutedEventArgs) Handles 全体复活.Click
			战场.全体复活()
			Dim a As I界面团队 = 团队列表.SelectedItem
			If a IsNot Nothing Then a.战力改变()
			Dim b As I界面成员 = 成员列表.SelectedItem
			If b IsNot Nothing Then b.战力改变()
		End Sub
		Private Sub 清空记录_Click(sender As Object, e As RoutedEventArgs) Handles 清空记录.Click
			战场.回合记录.Clear()
			战场.当前回合 = 0
		End Sub
		Private Sub 战一回合_Click(sender As Object, e As RoutedEventArgs) Handles 战一回合.Click
			Dim a As String = 战场.战一回合(战场.当前回合 + 1)
			If a IsNot Nothing Then
				Static 战斗结果TextBlock As New TextBlock
				战斗结果TextBlock.Text = "战斗结束，" & a
				Static 战斗结果Flyout As New Flyout With {.Content = 战斗结果TextBlock}
				战斗结果Flyout.ShowAt(战一回合)
			End If
		End Sub
		Private Sub 不死不休_Click(sender As Object, e As RoutedEventArgs) Handles 不死不休.Click
			Static 战斗结果String As String
			Do
				战斗结果String = 战场.战一回合(战场.当前回合 + 1)
			Loop While 战斗结果String Is Nothing
			Static 战斗结果TextBlock As New TextBlock
			战斗结果TextBlock.Text = "战斗结束，" & 战斗结果String
			Static 战斗结果Flyout As New Flyout With {.Content = 战斗结果TextBlock}
			战斗结果Flyout.ShowAt(不死不休)
		End Sub
		Private Sub 列表_GotFocus(sender As Object, e As RoutedEventArgs) Handles 成员列表.GotFocus, 团队列表.GotFocus
			删除焦点 = sender
		End Sub
		Private Sub 成员死亡_Click(sender As Object, e As RoutedEventArgs) Handles 成员死亡.Click
			生命.Text = 0
		End Sub
		Private Sub 团队全灭_Click(sender As Object, e As RoutedEventArgs) Handles 团队全灭.Click
			Dim a As I界面团队 = 团队列表.SelectedItem
			If a IsNot Nothing Then
				a.全灭()
				Dim b As I界面成员 = 成员列表.SelectedItem
				If b IsNot Nothing Then b.战力改变()
			End If
		End Sub

		Sub 删除键() Implements I响应删除键.删除键
			If 删除焦点 Is 团队列表 Then 删除团队_Click()
			If 删除焦点 Is 成员列表 Then 删除成员_Click()
		End Sub

		Private Async Sub 载入人物_Click(sender As Object, e As RoutedEventArgs) Handles 载入人物.Click
			Dim s As I界面团队 = 所属团队.SelectedItem
			If s Is Nothing Then
				Call New Flyout With {.Content = New TextBlock With {.Text = "必须选择所属团队"}}.ShowAt(所属团队)
			Else
				Dim a As New Pickers.FileOpenPicker
				a.FileTypeFilter.Add(".人物")
				Dim b As IAsyncOperation(Of IReadOnlyList(Of StorageFile)) = a.PickMultipleFilesAsync()
				'让用户选择文件，此时计算全场成员能力点数-等级的回归方程
				Dim f As I界面成员() = 战场.全场成员.ToArray, p As I界面人物, c As IReadOnlyList(Of StorageFile) = Await b
				If c IsNot Nothing Then
					If f.Any Then
						Dim g(f.GetUpperBound(0)) As Integer, h(f.GetUpperBound(0)) As Single
						For d As Byte = 0 To f.GetUpperBound(0)
							g(d) = f(d).等级
							h(d) = Math.Log(f(d).总能力点)
						Next
						Dim i As Single = g.Average, j As Single = h.Average, k As Single = 0, l As Single = 0
						For d As Byte = 0 To f.GetUpperBound(0)
							k += (g(d) - i) * (h(d) - j)
							l += (g(d) - i) ^ 2
						Next
						Dim r As Single
						If l = 0 Then
							For Each o As StorageFile In c
								p = New BinaryReader(Await o.OpenStreamForReadAsync).Read人物
								r = f(0).总能力点 * Math.Exp((p.等级 - f(0).等级) / 2) / p.总系数
								Try
									新成员(p.名字, p.攻击系数 * r, p.防御系数 * r, p.精准系数 * r, p.闪避系数 * r, 生命.Text, 所属团队.SelectedItem, p.等级, p.谋略)
								Catch ex As InvalidCastException
									新成员(p.名字, p.攻击系数 * r, p.防御系数 * r, p.精准系数 * r, p.闪避系数 * r, p.防御系数 * 战场.复活血量, 所属团队.SelectedItem, p.等级, p.谋略)
								End Try
							Next
						Else
							Dim m As Single = k / l, n As Single = j - m * i
							'回归方程：ln(能力点数)=m*等级+n
							For Each o As StorageFile In c
								p = New BinaryReader(Await o.OpenStreamForReadAsync).Read人物
								r = Math.Exp(p.等级 * m + n) / p.总系数
								Try
									新成员(p.名字, p.攻击系数 * r, p.防御系数 * r, p.精准系数 * r, p.闪避系数 * r, 生命.Text, 所属团队.SelectedItem, p.等级, p.谋略)
								Catch ex As InvalidCastException
									新成员(p.名字, p.攻击系数 * r, p.防御系数 * r, p.精准系数 * r, p.闪避系数 * r, p.防御系数 * 战场.复活血量, 所属团队.SelectedItem, p.等级, p.谋略)
								End Try
							Next
						End If
					Else
						For Each o As StorageFile In c
							p = New BinaryReader(Await o.OpenStreamForReadAsync).Read人物
							Try
								新成员(p.名字, p.攻击系数, p.防御系数, p.精准系数, p.闪避系数, 生命.Text, 所属团队.SelectedItem, p.等级, p.谋略)
							Catch ex As InvalidCastException
								新成员(p.名字, p.攻击系数, p.防御系数, p.精准系数, p.闪避系数, p.防御系数 * 战场.复活血量, 所属团队.SelectedItem, p.等级, p.谋略)
							End Try
						Next
					End If
					团队列表.SelectedItem = 所属团队.SelectedItem
				End If
			End If
		End Sub

		Private Sub 回合记录_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 回合记录.SelectionChanged
			Dim a As I界面回合 = 回合记录.SelectedItem
			If a IsNot Nothing Then
				回合数.Text = "第" & a.键 & "回合"
				回合总伤害.Text = a.值
				Dim b As List(Of I界面统计) = a.Get输出统计
				b.Sort()
				输出排行.ItemsSource = b
				b = a.Get受伤统计
				b.Sort()
				受伤排行.ItemsSource = b
			End If
		End Sub

		Private Sub 输出排行_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 输出排行.SelectionChanged
			Dim a As I界面统计 = 输出排行.SelectedItem
			If a IsNot Nothing Then
				输出人.Text = a.键.名称
				个人总输出.Text = a.值
				Dim b As List(Of I界面条目) = a.统计表
				b.Sort()
				输出目标排行.ItemsSource = b
			End If
		End Sub

		Private Sub 受伤排行_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 受伤排行.SelectionChanged
			Dim a As I界面统计 = 受伤排行.SelectedItem
			If a IsNot Nothing Then
				受伤人.Text = a.键.名称
				个人总受伤.Text = a.值
				Dim b As List(Of I界面条目) = a.统计表
				b.Sort()
				伤害来源排行.ItemsSource = b
			End If
		End Sub
	End Class
End Namespace