' https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板
''' <summary>
''' 可用于自身或导航至 Frame 内部的空白页。
''' </summary>
Public NotInheritable Class MainPage
	Inherits Page
	Private 战斗模拟界面 As 战斗模拟.用户界面, 人物设定界面 As 人物设定.用户界面
	Private Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
		导航框架.SelectedItem = 战斗模拟NavigationViewItem
		战斗模拟界面 = New 战斗模拟.用户界面(团队列表, 所属团队, 复活血量, 当前回合, 创建团队, 团队名, 删除团队, 删除成员, 成员列表, 添加成员, 成员名, 攻击, 防御, 精准, 闪避, 生命, 团队战力, 个人战力, 成员复活, 团队复活, 全体复活, 清空记录, 战一回合, 不死不休, 成员死亡, 团队全灭, 载入人物, 战斗等级, 战斗谋略, 回合数, 回合总伤害, 输出人, 个人总输出, 受伤人, 个人总受伤, 输出目标排行, 伤害来源排行, 回合记录, 输出排行, 受伤排行)
		人物设定界面 = New 人物设定.用户界面(路径, 文件夹中的人物, 新建人物, 选择文件夹, 新建人物_确定, 名字, 攻击系数, 防御系数, 精准系数, 闪避系数, 随机上限, 设定等级, 设定谋略, 设定原型, 装备列表, 保存人物, 删除人物, 随机生成, 新装备名, 新建装备, 删除装备)

	End Sub
	Private Sub 导航框架_SelectionChanged(sender As NavigationView, args As NavigationViewSelectionChangedEventArgs) Handles 导航框架.SelectionChanged
		Static 功能集 As New Dictionary(Of NavigationViewItem, FrameworkElement) From {{战斗模拟NavigationViewItem, 战斗模拟Pivot}, {人物设定NavigationViewItem, 人物设定Grid}}, 当前功能 As FrameworkElement = 战斗模拟Pivot
		导航框架.Header = DirectCast(args.SelectedItem, NavigationViewItem).Content
		当前功能.Visibility = Visibility.Collapsed
		当前功能 = 功能集(args.SelectedItem)
		当前功能.Visibility = Visibility.Visible
	End Sub
	Private Sub MainPage_KeyUp(sender As Object, e As KeyRoutedEventArgs) Handles Me.KeyUp
		If e.Key = Windows.System.VirtualKey.Delete Then
			Static 界面字典 As New Dictionary(Of NavigationViewItem, I响应删除键) From {{战斗模拟NavigationViewItem, 战斗模拟界面}, {人物设定NavigationViewItem, 人物设定界面}}
			界面字典(导航框架.SelectedItem).删除键()
		End If
	End Sub
	Sub 打开文件(文件列表 As IReadOnlyList(Of Windows.Storage.IStorageFile))
		人物设定界面.打开文件(文件列表)
		导航框架.SelectedItem = 人物设定NavigationViewItem
	End Sub
End Class
