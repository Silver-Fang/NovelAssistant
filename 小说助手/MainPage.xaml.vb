' https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板
Imports 小说助手.战斗模拟

''' <summary>
''' 可用于自身或导航至 Frame 内部的空白页。
''' </summary>
Public NotInheritable Class MainPage
	Inherits Page
	Private 战场 As New 战场, 删除焦点 As ListView

	Private Sub 创建团队_Click(sender As Object, e As RoutedEventArgs) Handles 创建团队.Click
		Static a As 团队, 错误提示 As New Flyout With {.Content = New TextBlock With {.Text = "默契度必须在0~255之间"}}
		Try
			a = New 团队(团队名.Text, 默契度.Text)
		Catch ex As OverflowException
			错误提示.ShowAt(默契度)
			Exit Sub
		Catch ex As InvalidCastException
			错误提示.ShowAt(默契度)
			Exit Sub
		End Try
		战场.团队列表.Add(a)
		'团队列表.SelectedItem = a
	End Sub

	Private Sub 删除团队_Click() Handles 删除团队.Click
		战场.团队列表.Remove(团队列表.SelectedItem)
	End Sub

	Private Sub 删除成员_Click() Handles 删除成员.Click
		Dim a As 成员 = 成员列表.SelectedItem
		If a IsNot Nothing Then a.所属团队.成员列表.Remove(a)
	End Sub

	Private Sub MainPage_KeyUp(sender As Object, e As KeyRoutedEventArgs) Handles Me.KeyUp
		If e.Key = Windows.System.VirtualKey.Delete Then
			If 删除焦点 Is 团队列表 Then 删除团队_Click()
			If 删除焦点 Is 成员列表 Then 删除成员_Click()
		End If
	End Sub

	Private Sub 添加成员_Click(sender As Object, e As RoutedEventArgs) Handles 添加成员.Click
		Dim a As 团队 = 所属团队.SelectedItem
		If a Is Nothing Then
			Static 未选团队 As New Flyout With {.Content = New TextBlock With {.Text = "必须选择所属团队"}}
			未选团队.ShowAt(所属团队)
		Else
			Static 错误提示 As New Flyout With {.Content = New TextBlock With {.Text = "攻击、防御、精准、闪避必须在0~32767之间，生命必须在0~2147483647之间"}}
			Try
				Dim b As New 成员(成员名.Text, 攻击.Text, 防御.Text, 精准.Text, 闪避.Text, 生命.Text, a)
				'成员列表.SelectedItem = b
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
	Private Sub 改变当前团队(当前团队 As 团队)
		With 当前团队
			团队名.SetBinding(TextBox.TextProperty, .名称Binding)
			默契度.SetBinding(TextBox.TextProperty, .默契度Binding)
			'Dim a As New Binding With {.Source = 当前团队, .path=new propertypath("成员列表"), .Mode = BindingMode.TwoWay}
			'成员列表.SetBinding(ItemsControl.ItemsSourceProperty, a)
			成员列表.ItemsSource = .成员列表
			'成员列表.ItemsSource = 当前团队.成员列表
			'成员列表.ItemsSource = 当前团队.成员列表
			团队战力.SetBinding(TextBlock.TextProperty, .整数战力Binding)
		End With
	End Sub

	Private Sub 改变当前成员(当前成员 As 成员)
		With 当前成员
			成员名.SetBinding(TextBox.TextProperty, .名称Binding)
			所属团队.SetBinding(Selector.SelectedItemProperty, .所属团队Binding)
			攻击.SetBinding(TextBox.TextProperty, .攻击Binding)
			防御.SetBinding(TextBox.TextProperty, .防御Binding)
			精准.SetBinding(TextBox.TextProperty, .精准Binding)
			闪避.SetBinding(TextBox.TextProperty, .闪避Binding)
			生命.SetBinding(TextBox.TextProperty, .生命Binding)
			个人战力.SetBinding(TextBlock.TextProperty, .整数战力Binding)
		End With
	End Sub

	Private Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
		'Dim a As New 团队, b As New 成员
		团队列表.ItemsSource = 战场.团队列表
		所属团队.ItemsSource = 战场.团队列表
		'改变当前团队(New 团队)
		'改变当前成员(New 成员)
		导航框架.SelectedItem = 导航框架.MenuItems(0)
		复活血量.SetBinding(TextBox.TextProperty, 战场.复活血量Binding)
		当前回合.SetBinding(TextBox.TextProperty, 战场.当前回合Binding)
		战斗记录.ItemsSource = 战场.战斗记录
		'成员列表.SetBinding(ItemsControl.ItemsSourceProperty, New Binding With {.Source = 团队列表, .Path = New PropertyPath("SelectedItem.成员列表")})
	End Sub

	Private Sub 团队列表_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 团队列表.SelectionChanged
		If 团队列表.SelectedItem IsNot Nothing Then 改变当前团队(团队列表.SelectedItem)
	End Sub

	Private Sub 成员列表_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 成员列表.SelectionChanged
		If 成员列表.SelectedItem IsNot Nothing Then 改变当前成员(成员列表.SelectedItem)
	End Sub

	Private Sub 成员复活_Click(sender As Object, e As RoutedEventArgs) Handles 成员复活.Click
		生命.Text = 防御.Text * 战场.复活血量
	End Sub

	Private Sub 团队复活_Click(sender As Object, e As RoutedEventArgs) Handles 团队复活.Click
		Dim a As 团队 = 团队列表.SelectedItem
		If a IsNot Nothing Then
			a.复活(战场.复活血量)
			Dim b As 成员 = 成员列表.SelectedItem
			If b IsNot Nothing Then b.战力改变()
		End If
	End Sub

	Private Sub 全体复活_Click(sender As Object, e As RoutedEventArgs) Handles 全体复活.Click
		战场.全体复活()
		Dim a As 团队 = 团队列表.SelectedItem
		If a IsNot Nothing Then a.战力改变()
		Dim b As 成员 = 成员列表.SelectedItem
		If b IsNot Nothing Then b.战力改变()
		战场.当前回合 = 1
	End Sub

	Private Sub 清空记录_Click(sender As Object, e As RoutedEventArgs) Handles 清空记录.Click
		战场.战斗记录.Clear()
	End Sub
	Private Sub 战一回合_Click(sender As Object, e As RoutedEventArgs) Handles 战一回合.Click
		Dim a As String = 战场.战一回合()
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
			战斗结果String = 战场.战一回合()
		Loop While 战斗结果string Is Nothing
		Static 战斗结果TextBlock As New TextBlock
		战斗结果TextBlock.Text = "战斗结束，" & 战斗结果String
		Static 战斗结果Flyout As New Flyout With {.Content = 战斗结果TextBlock}
		战斗结果Flyout.ShowAt(不死不休)
	End Sub

	Private Sub 列表_GotFocus(sender As Object, e As RoutedEventArgs) Handles 成员列表.GotFocus, 团队列表.GotFocus
		删除焦点 = sender
	End Sub

	Private Sub 导航框架_SelectionChanged(sender As NavigationView, args As NavigationViewSelectionChangedEventArgs) Handles 导航框架.SelectionChanged
		导航框架.Header = DirectCast(args.SelectedItem, NavigationViewItem).Content
	End Sub

	Private Sub 成员死亡_Click(sender As Object, e As RoutedEventArgs) Handles 成员死亡.Click
		Dim a As 成员 = 成员列表.SelectedItem
		If a IsNot Nothing Then a.死亡()
	End Sub

	Private Sub 团队全灭_Click(sender As Object, e As RoutedEventArgs) Handles 团队全灭.Click
		Dim a As 团队 = 团队列表.SelectedItem
		If a IsNot Nothing Then
			a.全灭()
			Dim b As 成员 = 成员列表.SelectedItem
			If b IsNot Nothing Then b.战力改变()
		End If
	End Sub
End Class
