Namespace 战斗模拟
	Interface I文件成员
		Inherits I有名称, I有生命, I攻防精闪等谋
		Property 所属团队 As I有名称
	End Interface
	Interface I文件伤害
		Property 攻方 As I文件成员
		Property 守方 As I文件成员
		Property 伤害 As UShort
	End Interface
	Interface I文件回合
		Property 伤害条目 As IReadOnlyCollection(Of I文件伤害)
		Property 战死者 As IEnumerable(Of I文件成员)
		WriteOnly Property 回合号 As Byte
	End Interface
	Interface I文件战场
		Inherits I血量回合
		Property 团队列表 As IReadOnlyList(Of I有名称)
		Property 全场成员 As IReadOnlyList(Of I文件成员)
		Property 回合记录 As IEnumerable(Of I文件回合)
	End Interface
	Structure 文件伤害
		Implements I文件伤害
		Public Property 攻方 As I文件成员 Implements I文件伤害.攻方

		Public Property 守方 As I文件成员 Implements I文件伤害.守方

		Public Property 伤害 As UShort Implements I文件伤害.伤害
	End Structure
	Module 读写扩展
		<Extension> Function Write(source As BinaryWriter, value As I文件战场) As BinaryWriter
			With source
				Dim c As IList(Of I有名称) = value.团队列表
				Dim e As IList(Of I文件成员) = value.全场成员
				Dim f As IEnumerable(Of I文件回合) = value.回合记录
				For Each a As I文件回合 In f
					For Each g As I文件伤害 In a.伤害条目
						If e.Contains(g.攻方) = False Then e.Add(g.攻方)
						If e.Contains(g.守方) = False Then e.Add(g.守方)
					Next
				Next
				For Each a As I文件成员 In e
					If c.Contains(a.所属团队) = False Then c.Add(a.所属团队)
				Next
				.Write(CByte(c.Count))
				For Each a As I有名称 In c
					.Write(a.名称)
				Next
				.Write(CByte(e.Count))
				For Each a As I文件成员 In e
					.Write(a.名称)
					.Write(a.攻击)
					.Write(a.防御)
					.Write(a.精准)
					.Write(a.闪避)
					.Write(a.生命)
					.Write(a.等级)
					.Write(a.谋略)
					.Write(CByte(c.IndexOf(a.所属团队)))
				Next
				.Write(CByte(f.Count))
				For Each a As I文件回合 In f
					Dim h As IReadOnlyCollection(Of I文件伤害) = a.伤害条目
					.Write(CByte(h.Count))
					For Each g As I文件伤害 In h
						.Write(CByte(e.IndexOf(g.攻方)))
						.Write(CByte(e.IndexOf(g.守方)))
						.Write(g.伤害)
					Next
					.Write(CByte(a.战死者.Count))
					For Each g As I文件成员 In a.战死者
						.Write(CByte(e.IndexOf(g)))
					Next
				Next
				.Write(value.复活血量)
				.Write(value.当前回合)
			End With
			Return source
		End Function
		<Extension> Function Read文件战场(Of T成员 As {New, I文件成员}, T团队 As {New, I有名称}, T战场 As {New, I文件战场}, T回合 As {New, I文件回合})(source As BinaryReader) As I文件战场
			Dim a As New List(Of I有名称), c As New List(Of I文件成员), d As New List(Of I文件回合)
			Try
				With source
					For b As Byte = 1 To .ReadByte
						a.Add(New T团队 With {.名称 = source.ReadString})
					Next
					For b As Byte = 1 To .ReadByte
						c.Add(New T成员 With {.名称 = source.ReadString, .攻击 = source.ReadUInt16, .防御 = source.ReadUInt16, .精准 = source.ReadUInt16, .闪避 = source.ReadUInt16, .生命 = source.ReadUInt64, .等级 = source.ReadByte, .谋略 = source.ReadByte, .所属团队 = a(source.ReadByte)})
					Next
					For b As Byte = 1 To .ReadByte
						Dim e As New Collection(Of I文件伤害)
						For f As Byte = 1 To .ReadByte
							e.Add(New 文件伤害 With {.攻方 = c(source.ReadByte), .守方 = c(source.ReadByte), .伤害 = source.ReadUInt16})
						Next
						Dim g As New Collection(Of I文件成员)
						For f As Byte = 1 To .ReadByte
							g.Add(c(.ReadByte))
						Next
						d.Add(New T回合 With {.伤害条目 = e, .战死者 = g, .回合号 = b})
					Next
				End With
			Catch ex As EndOfStreamException
			Catch ex As ArgumentOutOfRangeException
			End Try
			Return New T战场 With {.团队列表 = a, .全场成员 = c, .回合记录 = d, .复活血量 = source.ReadByte, .当前回合 = source.ReadByte}
		End Function
	End Module
End Namespace