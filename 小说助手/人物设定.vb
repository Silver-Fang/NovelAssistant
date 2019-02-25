Imports Windows.Storage, Windows.Storage.AccessCache.StorageApplicationPermissions
Imports 小说助手.战斗模拟

Namespace 人物设定
	Friend Class 人物
		Implements I界面人物
		Property 随机上限 As Byte = 0
		Property 设定原型 As String = ""
		Property 装备列表 As New ObservableCollection(Of String)

		Public ReadOnly Property 名字 As String = "" Implements I界面人物.名字

		Public Property 等级 As Byte = 0 Implements I界面人物.等级

		Public Property 攻击系数 As Byte = 0 Implements I界面人物.攻击系数

		Public Property 防御系数 As Byte = 0 Implements I界面人物.防御系数

		Public Property 精准系数 As Byte = 0 Implements I界面人物.精准系数

		Public Property 闪避系数 As Byte = 0 Implements I界面人物.闪避系数

		Public Property 谋略 As Byte = 0 Implements I界面人物.谋略

		Sub New(名字 As String)
			Me.名字 = 名字
		End Sub

		Public Function 总系数() As Byte Implements I界面人物.总系数
			Return 攻击系数 + 防御系数 + 精准系数 + 闪避系数
		End Function
	End Class
	Friend Module 人物与字节
		<Extension> Sub Write(source As BinaryWriter, value As String())
			If value Is Nothing Then
				source.Write(0)
			Else
				source.Write(CByte(value.Count))
				For Each a As String In value
					source.Write(a)
				Next
			End If
		End Sub
		<Extension> Sub Write(source As BinaryWriter, value As 人物)
			With source
				.Write(value.名字)
				.Write(value.攻击系数)
				.Write(value.防御系数)
				.Write(value.精准系数)
				.Write(value.闪避系数)
				.Write(value.随机上限)
				.Write(value.等级)
				.Write(value.谋略)
				.Write(value.设定原型)
				.Write(value.装备列表.ToArray)
			End With
		End Sub
		<Extension> Function ReadStringArray(source As BinaryReader) As String()
			Static 下标上界 As Byte
			Try
				下标上界 = source.ReadByte - 1
			Catch ex As OverflowException
				Dim 空数组 As String() = {}
				Return 空数组
			Catch ex As EndOfStreamException
				Dim 空数组 As String() = {}
				Return 空数组
			End Try
			Dim a(下标上界) As String
			Try
				For c As Byte = 0 To 下标上界
					a(c) = source.ReadString
				Next
			Catch ex As EndOfStreamException
			End Try
			Return a
		End Function
		<Extension> Function Read人物(source As BinaryReader) As 人物
			Try
				Read人物 = New 人物(source.ReadString)
			Catch ex As EndOfStreamException
				Return Nothing
			End Try
			Try
				With Read人物
					.攻击系数 = source.ReadByte
					.防御系数 = source.ReadByte
					.精准系数 = source.ReadByte
					.闪避系数 = source.ReadByte
					.随机上限 = source.ReadByte
					.等级 = source.ReadByte
					.谋略 = source.ReadByte
					.设定原型 = source.ReadString
					.装备列表 = New ObservableCollection(Of String)(source.ReadStringArray)
				End With
			Catch ex As EndOfStreamException
			End Try
		End Function
	End Module
	Class 用户界面
		Private 人物文件夹 As StorageFolder
		ReadOnly 路径 As TextBlock, 新建人物 As Control, 名字 As TextBox, 新装备名 As TextBox
		WithEvents 选择文件夹 As ButtonBase, 新建人物_确定 As ButtonBase, 文件夹中的人物 As Selector, 保存人物 As ButtonBase, 删除人物 As ButtonBase, 随机生成 As ButtonBase, 装备列表 As Selector, 新建装备 As ButtonBase, 删除装备 As ButtonBase, 攻击系数 As TextBox, 防御系数 As TextBox, 精准系数 As TextBox, 闪避系数 As TextBox, 随机上限 As TextBox, 等级 As TextBox, 谋略 As TextBox, 设定原型 As TextBox
		Private Function 获取文件夹中的人物文件() As IAsyncOperation(Of IReadOnlyList(Of StorageFile))
			Return 人物文件夹.CreateFileQueryWithOptions(New Search.QueryOptions(Search.CommonFileQuery.DefaultQuery, {".人物"})).GetFilesAsync
		End Function
		Private Sub 刷新人物列表(文件列表 As IReadOnlyList(Of StorageFile))
			文件夹中的人物.Items.Clear()
			For Each a As StorageFile In 文件列表
				文件夹中的人物.Items.Add(New 人物文件(a))
			Next
		End Sub
		Private Async Sub 初始化()
			If MostRecentlyUsedList.ContainsItem("人物文件夹") Then
				人物文件夹 = Await MostRecentlyUsedList.GetItemAsync("人物文件夹")
				路径.Text = 人物文件夹.Path
				刷新人物列表(Await 获取文件夹中的人物文件())
				新建人物.IsEnabled = True
			End If
		End Sub
		Sub New(路径 As TextBlock, 文件夹中的人物 As Selector, 新建人物 As Control, 选择文件夹 As ButtonBase, 新建人物_确定 As ButtonBase, 名字 As TextBox, 攻击系数 As TextBox, 防御系数 As TextBox, 精准系数 As TextBox, 闪避系数 As TextBox, 随机上限 As TextBox, 等级 As TextBox, 谋略 As TextBox, 设定原型 As TextBox, 装备列表 As ItemsControl, 保存人物 As ButtonBase, 删除人物 As ButtonBase, 随机生成 As ButtonBase, 新装备名 As TextBox, 新建装备 As ButtonBase, 删除装备 As ButtonBase)
			Me.路径 = 路径
			Me.文件夹中的人物 = 文件夹中的人物
			Me.新建人物 = 新建人物
			Me.选择文件夹 = 选择文件夹
			Me.新建人物_确定 = 新建人物_确定
			Me.名字 = 名字
			Me.攻击系数 = 攻击系数
			Me.防御系数 = 防御系数
			Me.精准系数 = 精准系数
			Me.闪避系数 = 闪避系数
			Me.随机上限 = 随机上限
			Me.等级 = 等级
			Me.谋略 = 谋略
			Me.设定原型 = 设定原型
			Me.装备列表 = 装备列表
			Me.保存人物 = 保存人物
			Me.删除人物 = 删除人物
			Me.随机生成 = 随机生成
			Me.新装备名 = 新装备名
			Me.新建装备 = 新建装备
			Me.删除装备 = 删除装备
			初始化()
		End Sub
		Private Async Sub 选择文件夹_Click(sender As Object, e As RoutedEventArgs) Handles 选择文件夹.Click
			Dim b As New Pickers.FolderPicker
			b.FileTypeFilter.Add("*")
			人物文件夹 = Await b.PickSingleFolderAsync
			If 人物文件夹 IsNot Nothing Then
				刷新人物列表(Await 获取文件夹中的人物文件())
				路径.Text = 人物文件夹.Path
				MostRecentlyUsedList.AddOrReplace("人物文件夹", 人物文件夹)
				新建人物.IsEnabled = True
			End If
		End Sub
		Private Class 人物文件
			Private i人物 As 人物
			Property 未保存 As Boolean = False
			ReadOnly Property 人物 As 人物
				Get
					If i人物 Is Nothing Then i人物 = New BinaryReader(文件.OpenStreamForReadAsync.Result).Read人物
					If i人物 Is Nothing Then i人物 = New 人物(ToString)
					Return i人物
				End Get
			End Property
			Property 文件 As StorageFile
			Sub New(文件 As StorageFile)
				Me.文件 = 文件
			End Sub
			Public Overrides Function ToString() As String
				Return 文件.DisplayName
			End Function
		End Class
		Private Async Sub 新建人物_确定_Click(sender As Object, e As RoutedEventArgs) Handles 新建人物_确定.Click
			Dim b As New BinaryWriter(Await (Await 人物文件夹.CreateFileAsync(名字.Text & ".人物", CreationCollisionOption.OpenIfExists)).OpenStreamForWriteAsync)
			b.Write(New 人物(名字.Text))
			b.Close()
			刷新人物列表(Await 获取文件夹中的人物文件())
			For Each a As 人物文件 In 文件夹中的人物.Items
				If a.ToString = 名字.Text & ".人物" Then
					文件夹中的人物.SelectedItem = a
					Exit For
				End If
			Next
		End Sub

		Private Sub 文件夹中的人物_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles 文件夹中的人物.SelectionChanged
			Dim a As 人物文件 = 文件夹中的人物.SelectedItem
			If a Is Nothing Then
				攻击系数.Text = ""
				防御系数.Text = ""
				精准系数.Text = ""
				闪避系数.Text = ""
				随机上限.Text = ""
				等级.Text = ""
				谋略.Text = ""
				设定原型.Text = ""
				保存人物.IsEnabled = False
				删除人物.IsEnabled = False
			Else
				RemoveHandler 攻击系数.TextChanged, AddressOf TextChanged
				RemoveHandler 防御系数.TextChanged, AddressOf TextChanged
				RemoveHandler 精准系数.TextChanged, AddressOf TextChanged
				RemoveHandler 闪避系数.TextChanged, AddressOf TextChanged
				RemoveHandler 随机上限.TextChanged, AddressOf TextChanged
				RemoveHandler 等级.TextChanged, AddressOf TextChanged
				RemoveHandler 谋略.TextChanged, AddressOf TextChanged
				RemoveHandler 设定原型.TextChanged, AddressOf TextChanged
				With a.人物
					攻击系数.Text = .攻击系数
					防御系数.Text = .防御系数
					精准系数.Text = .精准系数
					闪避系数.Text = .闪避系数
					随机上限.Text = .随机上限
					等级.Text = .等级
					谋略.Text = .谋略
					设定原型.Text = .设定原型
					装备列表.ItemsSource = .装备列表
				End With
				AddHandler 攻击系数.TextChanged, AddressOf TextChanged
				AddHandler 防御系数.TextChanged, AddressOf TextChanged
				AddHandler 精准系数.TextChanged, AddressOf TextChanged
				AddHandler 闪避系数.TextChanged, AddressOf TextChanged
				AddHandler 随机上限.TextChanged, AddressOf TextChanged
				AddHandler 等级.TextChanged, AddressOf TextChanged
				AddHandler 谋略.TextChanged, AddressOf TextChanged
				AddHandler 设定原型.TextChanged, AddressOf TextChanged
				删除人物.IsEnabled = True
				保存人物.IsEnabled = a.未保存
			End If
		End Sub

		Private Async Sub 保存人物_Click(sender As Object, e As RoutedEventArgs) Handles 保存人物.Click
			Dim a As 人物文件 = 文件夹中的人物.SelectedItem
			Dim b As Task(Of Stream) = a.文件.OpenStreamForWriteAsync
			With a.人物
				.攻击系数 = 攻击系数.Text
				.防御系数 = 防御系数.Text
				.精准系数 = 精准系数.Text
				.闪避系数 = 闪避系数.Text
				.随机上限 = 随机上限.Text
				.等级 = 等级.Text
				.谋略 = 谋略.Text
				.设定原型 = 设定原型.Text
				.装备列表 = 装备列表.ItemsSource
			End With
			Dim c As New BinaryWriter(Await b)
			c.Write(a.人物)
			c.Close()
			a.未保存 = False
			保存人物.IsEnabled = False
		End Sub

		Private Sub 删除人物_Click() Handles 删除人物.Click
			With 文件夹中的人物
				Call DirectCast(.SelectedItem, 人物文件).文件.DeleteAsync()
				.Items.Remove(.SelectedItem)
			End With
		End Sub

		Private Sub 随机生成_Click(sender As Object, e As RoutedEventArgs) Handles 随机生成.Click
			Static 随机生成器 As New Random, 随机上限Byte As Byte, 错误提示 As New Flyout With {.Content = New TextBlock With {.Text = "必须输入1~255的正整数"}}
			Try
				随机上限Byte = 随机上限.Text
			Catch ex As InvalidCastException
				错误提示.ShowAt(随机上限)
				Exit Sub
			Catch ex As OverflowException
				错误提示.ShowAt(随机上限)
				Exit Sub
			End Try
			If 随机上限Byte = 0 Then
				错误提示.ShowAt(随机上限)
				Exit Sub
			End If
			攻击系数.Text = 随机生成器.Next(1, 随机上限Byte)
			防御系数.Text = 随机生成器.Next(1, 随机上限Byte)
			精准系数.Text = 随机生成器.Next(1, 随机上限Byte)
			闪避系数.Text = 随机生成器.Next(1, 随机上限Byte)
			谋略.Text = 随机生成器.Next(256)
		End Sub
		Sub 打开文件(文件列表 As IReadOnlyList(Of StorageFile))
			新建人物.IsEnabled = False
			刷新人物列表(文件列表)
			路径.Text = "打开的文件"
		End Sub
		Private Sub 要求保存()
			Dim a As 人物文件 = 文件夹中的人物.SelectedItem
			If a IsNot Nothing Then
				a.未保存 = True
				保存人物.IsEnabled = True
			End If
		End Sub

		Private Sub 删除装备_Click() Handles 删除装备.Click
			If 装备列表.ItemsSource Is Nothing Then
				装备列表.Items.RemoveAt(装备列表.SelectedIndex)
			Else
				DirectCast(装备列表.ItemsSource, IList(Of String)).RemoveAt(装备列表.SelectedIndex)
			End If
			要求保存()
		End Sub

		Private Sub 新建装备_Click(sender As Object, e As RoutedEventArgs) Handles 新建装备.Click
			If 装备列表.ItemsSource Is Nothing Then
				装备列表.Items.Add(新装备名.Text)
			Else
				DirectCast(装备列表.ItemsSource, ICollection(Of String)).Add(新装备名.Text)
			End If
			要求保存()
		End Sub

		Private Sub TextChanged(sender As Object, e As TextChangedEventArgs) Handles 攻击系数.TextChanged, 防御系数.TextChanged, 精准系数.TextChanged, 闪避系数.TextChanged, 随机上限.TextChanged, 等级.TextChanged, 谋略.TextChanged, 设定原型.TextChanged
			要求保存()
		End Sub
	End Class
End Namespace