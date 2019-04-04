Imports System.IO
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Linq
Imports System.Configuration
Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices
Imports System.Drawing

Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D


Public Class Form1
    Dim righty As Integer = Placement.TextBox5.Text
    Dim righty2 As Integer = Placement.TextBox5.Text

    'twitter api is broken, this is an old app lol
    Dim oauth_consumer_key As String = ""
    Dim consumer_secret As String = ""
    Dim oauth_token As String = ""
    Dim oauth_token_secret As String = ""
    Dim path As String
    Dim fullpath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
    Dim pathOnly As String = My.Computer.FileSystem.GetParentPath(fullpath)
    Dim livepath = pathOnly & "\livestats\"
    Const DS As Integer = 21
    Const SP As Integer = &H2
    <DllImport("urlmon.dll")> _
    <PreserveSig> _
    Private Shared Function CoInternetSetFeatureEnabled(FeatureEntry As Integer, <MarshalAs(UnmanagedType.U4)> dSFlags As Integer, eEnable As Boolean) As <MarshalAs(UnmanagedType.[Error])> Integer
    End Function
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ListView1.Items.Add(New ListViewItem(New String() {TextBox5.Text, "Marquee"}))
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ListView1.Items.Add(New ListViewItem(New String() {TextBox5.Text, "Breaking"}))
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ListView1.Items.Add(New ListViewItem(New String() {TextBox5.Text, "Twitter"}))
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        ListView1.Items.Add(New ListViewItem(New String() {TextBox5.Text, "Score Update"}))
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        ListView1.Items.Clear()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Try
            For Each i As ListViewItem In ListView1.SelectedItems
                ListView1.Items.Remove(i)
            Next
        Catch ex As Exception
            MessageBox.Show("Select an item first...")
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        CoInternetSetFeatureEnabled(DS, SP, True)
        path = System.IO.Path.GetDirectoryName( _
           System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)

        Try
            RichTextBox1.LoadFile(pathOnly & "\ads\ads1.rtf")
            RichTextBox2.LoadFile(pathOnly & "\ads\ads2.rtf")
            RichTextBox3.LoadFile(pathOnly & "\ads\ads3.rtf")
            RichTextBox4.LoadFile(pathOnly & "\ads\ads4.rtf")
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString)
        End Try
        WebBrowser1.Navigate(pathOnly & "/marquee/marquee.html")
        Try
            LoadDataToListview()
        Catch ex As Exception

        End Try
        TextBox7.Text = livepath
    End Sub
    Private Sub generate()
        System.IO.File.WriteAllText("marquee\config.js", "$(function(){$('.marquee').marquee({showSpeed:1000, scrollSpeed: 10, yScroll:  'bottom', direction:  'left', pauseSpeed: " & TextBox6.Text & "000, duplicated: true});});")



        Dim marqueebody As String = ""

        For Each item As ListViewItem In ListView1.Items
            If item.SubItems(1).Text = "Breaking" Then
                marqueebody = marqueebody & "<li><font color='red'>BREAKING:</font> " & item.SubItems(0).Text & "</li>" & vbCrLf
            End If
            If item.SubItems(1).Text = "Twitter" Then
                marqueebody = marqueebody & "<li><font color='#00aced'>Twitter:</font> " & item.SubItems(0).Text & "</li>" & vbCrLf
            End If
            If item.SubItems(1).Text = "Score Update" Then
                marqueebody = marqueebody & "<li><font color='orange'>Score Update:</font> " & item.SubItems(0).Text & "</li>" & vbCrLf
            End If
            If item.SubItems(1).Text = "Marquee" Then
                marqueebody = marqueebody & "<li>" & item.SubItems(0).Text & "</li>" & vbCrLf
            End If
        Next
        If System.IO.Directory.Exists("marquee") = False Then
            System.IO.Directory.CreateDirectory("marquee")
        End If

        System.IO.File.WriteAllText("marquee\marquee.html", "<!DOCTYPE HTML><html><head><title>Marquee</title><meta http-equiv='Content-type' content='text/html;charset=UTF-8' /><link rel='stylesheet' type='text/css' href='style.css'></head><script type='text/javascript' src='jquery.min.js'></script><script type='text/javascript' src='jquery.marquee.js'></script><script type='text/javascript' src='config.js'></script><body><ul class='marquee'>" & marqueebody & "</ul></body></html>")

        WebBrowser1.Navigate(pathOnly & "/marquee/marquee.html")
    End Sub
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        generate()
    End Sub
    Private Sub playsound(filename As String)
        Dim a As New MediaPlayer.MediaPlayer
        a.FileName = filename
        a.Play()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        playsound("sounds/sound1.mp3")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        playsound("sounds/sound2.mp3")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        playsound("sounds/sound3.mp3")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        playsound("sounds/sound4.mp3")
    End Sub

    Private Sub SaveAdsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAdsToolStripMenuItem.Click

        Try
            RichTextBox1.SaveFile(pathOnly & "\ads\ads1.rtf")
            RichTextBox2.SaveFile(pathOnly & "\ads\ads2.rtf")
            RichTextBox3.SaveFile(pathOnly & "\ads\ads3.rtf")
            RichTextBox4.SaveFile(pathOnly & "\ads\ads4.rtf")
        Catch ex As Exception
            MessageBox.Show(ex.Message.ToString)
        End Try
    End Sub

    Private Sub SaveConfigToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveConfigToolStripMenuItem.Click
        ExportListview2Excel(ListView1)
    End Sub
    Private Sub ExportListview2Excel(ByVal lstview As ListView)
        Dim csvFileContents As New System.Text.StringBuilder
        Dim CurrLine As String = String.Empty
        For columnIndex As Int32 = 0 To lstview.Columns.Count - 1
            CurrLine &= (String.Format("{0};", lstview.Columns(columnIndex).Text))
        Next
        csvFileContents.AppendLine(CurrLine.Substring(0, CurrLine.Length - 1))
        CurrLine = String.Empty
        For Each item As ListViewItem In lstview.Items
            For Each subItem As ListViewItem.ListViewSubItem In item.SubItems
                CurrLine &= (String.Format("{0};", subItem.Text))
            Next
            csvFileContents.AppendLine(CurrLine.Substring(0, CurrLine.Length - 1))
            CurrLine = String.Empty
        Next
        Dim Sys As New System.IO.StreamWriter(pathOnly & "\config\marquee.csv")
        Sys.WriteLine(csvFileContents.ToString)
        Sys.Flush()
        Sys.Dispose()
    End Sub
    Private Sub LoadDataToListview()
        Dim filePath As String = pathOnly & "\config\marquee.csv"
        Dim streamReader As New IO.StreamReader(filePath)
        Dim streamText As String
        Dim listViewItem As ListViewItem
        While Not streamReader.EndOfStream
            streamText = streamReader.ReadLine()
            If Not String.IsNullOrEmpty(streamText) Then
                If Not ListView1.Columns.Count = 0 Then
                    listViewItem = New ListViewItem(streamText.Split(";"))
                    If streamText.ToString = "Message;Type" Then
                    Else
                        ListView1.Items.Add(listViewItem)
                    End If

                Else
                    For Each value As String In streamText.Split(";")
                        If value.ToString = "Message;Type" Then
                        Else
                            ListView1.Columns.Add(value)
                        End If

                    Next
                End If
            End If
        End While
        streamReader.Dispose()
        generate()
    End Sub




    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ListBox1.Items.Clear()
        Dim myReq As New userRequest(oauth_consumer_key, consumer_secret, oauth_token, oauth_token_secret)
        Dim response As String = myReq.makeRequest()


        Dim json As String = response
        Dim ser As JArray = JArray.Parse(json)
        Dim data As List(Of JToken) = ser.Children().ToList
        Dim output As String = ""
        For i = 0 To ser.Count - 1
            Dim text = ser(i)("text").ToString()

            Dim user = ser(i)("user")("screen_name").ToString()

            ListBox1.Items.Add("@" & user & ":" & text)

        Next
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Try
            TextBox5.Text = ListBox1.SelectedItem.ToString
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        ListView1.Items.Add(New ListViewItem(New String() {ListBox1.SelectedItem.ToString, "Twitter"}))
    End Sub
    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Dim Text As String = ""
        Dim FontColor As Color = Color.Blue
        Dim BackColor As Color = Color.Yellow
        Dim FontName As String = "Times New Roman"
        Dim FontSize As Integer = 14
        Dim Height As Integer = 1020
        Dim Width As Integer = 1980
        Dim FileName As String = pathOnly & "\img"
        Dim objBitmap As New Bitmap(Width, Height)
        Dim objGraphics As Graphics = Graphics.FromImage(objBitmap)
        'Dim objColor As Color
        Dim objFont As New Font(FontName, FontSize)
        'Following PointF object defines where the text will be displayed in the
        'specified area of the image
        Dim objPoint As New PointF(5.0F, 5.0F)
        Dim objBrushForeColor As New SolidBrush(FontColor)
        Dim objBrushBackColor As New SolidBrush(Color.Transparent)
        objGraphics.FillRectangle(objBrushBackColor, 0, 0, Width, Height)
        objGraphics.DrawString(Text, objFont, objBrushForeColor, objPoint)
        objBitmap.Save(FileName & ".png", ImageFormat.Png)
        Dim fs As System.IO.FileStream
        ' Specify a valid picture file path on your computer.
        fs = New System.IO.FileStream(FileName & ".png",
             IO.FileMode.Open, IO.FileAccess.Read)
        PictureBox1.Image = System.Drawing.Image.FromStream(fs)
        fs.Close()
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        'boosters logo
        genadvert("boosters")
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        'garrettrichard logo
        genadvert("gr")
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        'clickingspree logo
        genadvert("clickingspree")
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        'keetons logo
        genadvert("keetons")
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        'devitos logo
        genadvert("devitos")
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        'jaxx playbook logo
        genadvert("jaxx")
    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        'ugh logo
        genadvert("ugh")
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        'speed kings logo
        genadvert("speedkings")
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        'pregame show logo
        genadvert("pregame")
    End Sub

    Private Sub genadvert(ByVal adlogo As String)
        Dim FileName As String = pathOnly & "\img"
        Dim ms As New MemoryStream
        Dim objBitmap As New Bitmap(pathOnly & "\adlogos\" & adlogo & ".png")
        Dim e2 As Graphics = Graphics.FromImage(objBitmap)
        Try
            objBitmap.Save(FileName & ".png", ImageFormat.Png)
        Finally
            objBitmap.Dispose()
            e2.Dispose()
        End Try
        Dim fs As System.IO.FileStream
        ' Specify a valid picture file path on your computer.
        fs = New System.IO.FileStream(FileName & ".png",
             IO.FileMode.Open, IO.FileAccess.Read)
        PictureBox1.Image = System.Drawing.Image.FromStream(fs)
        fs.Close()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ListBox2.Items.Clear()
        ListBox3.Items.Clear()
        Try
            Dim sport = ComboBox2.SelectedItem.ToString
            Dim playername As String = ComboBox1.SelectedItem.ToString.Replace(" ", "").ToLower
            playername = playername.Replace(",", "")
            playername = playername.Replace(".", "")
            playername = playername.Replace("'", "")
            Dim newpath = pathOnly & "\stats\" & sport & "\" & playername & ".csv"
            Dim livefile = livepath & sport & "\" & playername & ".csv"
            If File.Exists(newpath) Then
                ListBox2.Items.AddRange(IO.File.ReadAllLines(newpath))
            Else
                File.WriteAllText(newpath, "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf)
                ListBox2.Items.AddRange(IO.File.ReadAllLines(newpath))
            End If
            If File.Exists(livefile) Then
                ListBox3.Items.AddRange(IO.File.ReadAllLines(livefile))
            Else
                File.WriteAllText(livefile, "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf & "Dummy:Data" & vbCrLf)
                ListBox3.Items.AddRange(IO.File.ReadAllLines(livefile))
            End If
            Dim fs As System.IO.FileStream
            ' Specify a valid picture file path on your computer.
            fs = New System.IO.FileStream(pathOnly & "\players\" & sport & "\" & playername & ".png",
                 IO.FileMode.Open, IO.FileAccess.Read)
            PictureBox2.Image = System.Drawing.Image.FromStream(fs)
            fs.Close()
            For i As Integer = 0 To ListBox2.Items.Count - 1
                ListBox2.SetSelected(i, True)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button15_Click_1(sender As Object, e As EventArgs) Handles Button15.Click
        Try
            Dim sport = ComboBox2.SelectedItem.ToString
            Dim playername As String = ComboBox1.SelectedItem.ToString.Replace(" ", "").ToLower
            playername = playername.Replace(",", "")
            playername = playername.Replace(".", "")
            playername = playername.Replace("'", "")
            Dim FileName As String = pathOnly & "\img"
            Dim ms As New MemoryStream
            Dim objBitmap As New Bitmap(pathOnly & "\img-bg.png")
            Dim e2 As Graphics = Graphics.FromImage(objBitmap)
            e2.DrawImage(Bitmap.FromFile(pathOnly & "\players\" & sport & "\" & playername & ".png"), New Rectangle(480, 200, 308, 450))
            ' e2.DrawImage(Bitmap.FromFile(pathOnly & "\scratch.png"), New Rectangle(300, 520, 400, 400))
            Dim text1 As String = ""
            For Each selecteditem In ListBox2.SelectedItems
                text1 = text1 & "•  " & selecteditem.ToString & vbCrLf & vbCrLf
            Next
            Dim playerfont As New Font("Agency FB", 36, FontStyle.Bold, GraphicsUnit.Point)
            Dim font1 As New Font("Agency FB", 24, FontStyle.Bold, GraphicsUnit.Point)
            Try
                '487 is far left of box
                Dim rectF1 As New RectangleF(500, 120, 950, 700)
                Dim rectF2 As New RectangleF(900, 200, 950, 700)
                e2.DrawString(ComboBox1.SelectedItem.ToString.ToUpper, playerfont, Brushes.Red, rectF1)
                e2.DrawString(text1, font1, Brushes.White, rectF2)
                objBitmap.Save(FileName & ".png", ImageFormat.Png)


            Finally

                font1.Dispose()
                objBitmap.Dispose()
                e2.Dispose()

            End Try
            Dim fs As System.IO.FileStream
            ' Specify a valid picture file path on your computer.
            fs = New System.IO.FileStream(FileName & ".png",
                 IO.FileMode.Open, IO.FileAccess.Read)
            PictureBox1.Image = System.Drawing.Image.FromStream(fs)
            fs.Close()
        Catch
        End Try

    End Sub

    Private Sub gensingleline()

        Try
            Dim FileName As String = pathOnly & "\single-line"
            Dim ms As New MemoryStream
            Dim objBitmap As New Bitmap(1920, 1080)
            Dim e2 As Graphics = Graphics.FromImage(objBitmap)
            Dim baseimg As String = "empty"
            e2.DrawImage(Bitmap.FromFile(pathOnly & "\single-line-src.png"), New Rectangle(Placement.TextBox5.Text, Placement.TextBox6.Text, Placement.TextBox7.Text, Placement.TextBox8.Text))
            Try
                objBitmap.Save(FileName & ".png", ImageFormat.Png)
            Finally
                objBitmap.Dispose()
                e2.Dispose()
            End Try
        Catch
        End Try
    End Sub
    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        gensingleline()
        Dim basex As String = Placement.TextBox5.Text
        Dim basey As String = Placement.TextBox6.Text
        Try
            Dim sport = ComboBox2.SelectedItem.ToString
            Dim playername As String = ComboBox1.SelectedItem.ToString.Replace(" ", "").ToLower
            playername = playername.Replace(",", "")
            playername = playername.Replace(".", "")
            playername = playername.Replace("'", "")
            Dim FileName As String = pathOnly & "\img"
            Dim ms As New MemoryStream
            Dim objBitmap As New Bitmap(pathOnly & "\single-line.png")
            Dim e2 As Graphics = Graphics.FromImage(objBitmap)
            e2.DrawImage(Bitmap.FromFile(pathOnly & "\players\" & sport & "\head\" & playername & ".png"), New Rectangle(basex + 209, basey - 60, 217, 223))
            Dim font1 As New Font("Agency FB", 20, FontStyle.Bold, GraphicsUnit.Point)
            Dim text1 As String = ""

            righty = basex + 440
            For Each selecteditem In ListBox2.SelectedItems
                Dim rectF2 = New RectangleF(righty, basey + 80, 950, 700)
                Dim header As String = selecteditem.ToString.Substring(0, selecteditem.ToString.IndexOf(":"))
                Dim stat As String = selecteditem.ToString.Substring(selecteditem.ToString.IndexOf(":") + 1, selecteditem.ToString.Length - selecteditem.ToString.IndexOf(":") - 1)
                e2.DrawString(header, font1, Brushes.Red, rectF2)
                For i As Integer = 0 To header.Length
                    righty += 12
                Next

                rectF2 = New RectangleF(righty, basey + 80, 950, 700)
                e2.DrawString(stat, font1, Brushes.White, rectF2)
                For i As Integer = 0 To stat.Length
                    righty += 10
                Next
                righty += 20
            Next
            righty2 = basex + 440
            For Each selecteditem In ListBox3.SelectedItems
                Dim rectF2 = New RectangleF(righty2, basey + 120, 950, 700)
                Dim header As String = selecteditem.ToString.Substring(0, selecteditem.ToString.IndexOf(":"))
                Dim stat As String = selecteditem.ToString.Substring(selecteditem.ToString.IndexOf(":") + 1, selecteditem.ToString.Length - selecteditem.ToString.IndexOf(":") - 1)
                e2.DrawString(header, font1, Brushes.Black, rectF2)
                For i As Integer = 0 To header.Length
                    righty2 += 12
                Next

                rectF2 = New RectangleF(righty2, basey + 120, 950, 700)
                e2.DrawString(stat, font1, Brushes.White, rectF2)
                For i As Integer = 0 To stat.Length
                    righty2 += 10
                Next
                righty2 += 20
            Next

            Dim playerfont As New Font("Agency FB", 36, FontStyle.Bold, GraphicsUnit.Point)

            Try
                '487 is far left of box
                Dim rectF1 As New RectangleF(basex + 450, basey + 20, 950, 700)
                e2.DrawString(ComboBox1.SelectedItem.ToString.ToUpper, playerfont, Brushes.White, rectF1)

                objBitmap.Save(FileName & ".png", ImageFormat.Png)


            Finally

                font1.Dispose()
                objBitmap.Dispose()
                e2.Dispose()

            End Try
            Dim fs As System.IO.FileStream
            ' Specify a valid picture file path on your computer.
            fs = New System.IO.FileStream(FileName & ".png",
                 IO.FileMode.Open, IO.FileAccess.Read)
            PictureBox1.Image = System.Drawing.Image.FromStream(fs)
            fs.Close()
        Catch
        End Try
        righty = 690
        righty2 = 690
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        livepath = TextBox7.Text
    End Sub

    Private Sub Button31_Click(sender As Object, e As EventArgs) Handles Button31.Click
        gendiamond()
    End Sub

    Private Sub gendiamond()
        Dim top = "▲"
        Dim bottom = "▼"
        Dim basex = Placement.TextBox1.Text '1580
        Dim basey = Placement.TextBox2.Text '25
        Dim basew = Placement.TextBox3.Text
        Dim baseh = Placement.TextBox4.Text
        Try
            Dim homefont = Placement.RichTextBox1.Font
            Dim visitorfont = Placement.RichTextBox2.Font
            Dim inningfont = Placement.RichTextBox3.Font
            Dim outfont = Placement.RichTextBox4.Font
            Dim topbottomfont = Placement.RichTextBox5.Font
            Dim FileName As String = pathOnly & "\img"
            Dim ms As New MemoryStream
            Dim objBitmap As New Bitmap(1920, 1080)
            Dim e2 As Graphics = Graphics.FromImage(objBitmap)
            Dim baseimg As String = "empty"
            Dim Hbrushcolor As New SolidBrush(Placement.RichTextBox1.ForeColor)
            Dim Vbrushcolor As New SolidBrush(Placement.RichTextBox2.ForeColor)
            Dim Ibrushcolor As New SolidBrush(Placement.RichTextBox3.ForeColor)
            Dim Obrushcolor As New SolidBrush(Placement.RichTextBox4.ForeColor)
            Dim TBbrushcolor As New SolidBrush(Placement.RichTextBox5.ForeColor)
            If CheckBox1.Checked = True Then
                baseimg = "1"
                If CheckBox2.Checked = True Then
                    baseimg = "12"
                    If CheckBox3.Checked = True Then
                        baseimg = "123"
                    End If
                Else
                    If CheckBox3.Checked = True Then
                        baseimg = "13"
                    End If
                End If
            Else
                If CheckBox2.Checked = True Then
                    baseimg = "2"
                    If CheckBox3.Checked = True Then
                        baseimg = "23"
                    End If
                Else
                    If CheckBox3.Checked = True Then
                        baseimg = "3"
                    End If
                End If
            End If
            e2.DrawImage(Bitmap.FromFile(pathOnly & "\baseball\" & baseimg & ".png"), New Rectangle(basex, basey, basew, baseh))
            Using GradientGraphic = e2
                Dim visitorrectangle As New RectangleF(basex + 15, basey + 64, 166, 49)
                Using GradientBrush As New LinearGradientBrush(visitorrectangle, Placement.Button7.BackColor, Placement.Button8.BackColor, LinearGradientMode.BackwardDiagonal)
                    GradientGraphic.FillRectangle(GradientBrush, visitorrectangle)
                End Using
                Dim HomeRectangle As New RectangleF(basex + 15, basey + 15, 166, 49)
                Using GradientBrush As New LinearGradientBrush(HomeRectangle, Placement.Button9.BackColor, Placement.Button10.BackColor, LinearGradientMode.BackwardDiagonal)
                    GradientGraphic.FillRectangle(GradientBrush, HomeRectangle)
                End Using

                Dim visitorrec = New RectangleF(basex + 10, basey + 20, 150, 50)
                Dim homerec = New RectangleF(basex + 10, basey + 70, 150, 50)
                Dim visitorscorerec = New RectangleF(basex + 150, basey + 20, 300, 50)
                Dim homescorerec = New RectangleF(basex + 150, basey + 70, 300, 50)
                Dim outsrec = New RectangleF(basex + 260, basey + 83, 300, 50)
                Dim inningrec = New RectangleF(basex + 203, basey + 41, 300, 50)
                Dim toprec = New RectangleF(basex + 199, basey + 12, 300, 50)
                Dim bottomrec = New RectangleF(basex + 199, basey + 81, 300, 50)
                Dim visitor = TextBox8.Text
                Dim home = TextBox9.Text
                Dim visitorscore = TextBox10.Text
                Dim homescore = TextBox11.Text
                If TextBox10.Text.Length > 1 Then
                    homescorerec = New RectangleF(basex + 140, basey + 70, 300, 50)
                    visitorscorerec = New RectangleF(basex + 140, basey + 20, 300, 50)
                End If
                If TextBox11.Text.Length > 1 Then
                    homescorerec = New RectangleF(basex + 140, basey + 70, 300, 50)
                    visitorscorerec = New RectangleF(basex + 140, basey + 20, 300, 50)
                End If
                Dim outs = TextBox15.Text & "-" & TextBox14.Text & "      Outs:" & TextBox13.Text
                Dim inning = TextBox12.Text
                If TextBox12.Text.Count > 1 Then
                    inningrec = New RectangleF(basex + 193, basey + 41, 300, 50)
                End If
                e2.DrawString(home, homefont, Hbrushcolor, homerec)
                e2.DrawString(visitor, visitorfont, Vbrushcolor, visitorrec)
                e2.DrawString(homescore, homefont, Hbrushcolor, homescorerec)
                e2.DrawString(visitorscore, visitorfont, Vbrushcolor, visitorscorerec)
                e2.DrawString(outs, outfont, Obrushcolor, outsrec)
                e2.DrawString(inning, inningfont, Ibrushcolor, inningrec)
                If RadioButton1.Checked = True Then
                    e2.DrawString(top, topbottomfont, TBbrushcolor, toprec)
                    e2.DrawString(bottom, topbottomfont, Brushes.Gray, bottomrec)
                Else
                    e2.DrawString(bottom, topbottomfont, TBbrushcolor, bottomrec)
                    e2.DrawString(top, topbottomfont, Brushes.Gray, toprec)
                End If

            End Using
            Try
                objBitmap.Save(FileName & ".png", ImageFormat.Png)
            Finally
                objBitmap.Dispose()
                e2.Dispose()
            End Try
            Dim fs As System.IO.FileStream
            ' Specify a valid picture file path on your computer.
            fs = New System.IO.FileStream(FileName & ".png",
                 IO.FileMode.Open, IO.FileAccess.Read)
            PictureBox1.Image = System.Drawing.Image.FromStream(fs)
            fs.Close()
        Catch ex As Exception
            Application.DoEvents()
        End Try
    End Sub



    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            Timer1.Enabled = True
            Timer1.Interval = 30000
            Timer1.Start()
        Else
            Timer1.Stop()
            Timer1.Enabled = False
            Timer1.Dispose()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Button12.PerformClick()
    End Sub

    Private Sub Button32_Click(sender As Object, e As EventArgs) Handles Button32.Click
        Placement.ShowDialog()
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        TextBox14.Text += 1
        If TextBox14.Text = 3 Then
            TextBox14.Text = 0
            TextBox15.Text = 0
            TextBox13.Text += 1
        End If
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If

    End Sub

    Private Sub TextBox13_TextChanged(sender As Object, e As EventArgs) Handles TextBox13.TextChanged
        If TextBox13.Text = 3 Then
            If RadioButton2.Checked = True Then
                TextBox12.Text += 1
                RadioButton1.Checked = True
                TextBox13.Text = 0
                TextBox14.Text = 0
                TextBox15.Text = 0
            Else
                RadioButton2.Checked = True
                TextBox13.Text = 0
            End If
        End If
    End Sub

    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Button29.Click
        TextBox15.Text += 1
        If TextBox15.Text = 4 Then
            TextBox15.Text = 0
            TextBox14.Text = 0
        End If
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If
    End Sub

    Private Sub Button33_Click(sender As Object, e As EventArgs) Handles Button33.Click
        TextBox13.Text += 1
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs) Handles Button30.Click
        TextBox15.Text = "0"
        TextBox14.Text = "0"
    End Sub
    Private Sub genscoreboard()
        Try
            Dim basex = Placement.TextBox12.Text '1580
            Dim basey = Placement.TextBox11.Text '25
            Dim basew = Placement.TextBox10.Text
            Dim baseh = Placement.TextBox9.Text
            Dim homefont = Placement.RichTextBox1.Font
            Dim visitorfont = Placement.RichTextBox2.Font
            Dim inningfont = Placement.RichTextBox3.Font
            Dim Hbrushcolor As New SolidBrush(Placement.RichTextBox1.ForeColor)
            Dim Vbrushcolor As New SolidBrush(Placement.RichTextBox2.ForeColor)
            Dim Ibrushcolor As New SolidBrush(Placement.RichTextBox3.ForeColor)
            Dim FileName As String = pathOnly & "\img"
            Dim visitor = TextBox8.Text
            Dim home = TextBox9.Text
            Dim visitorscore = TextBox10.Text
            Dim homescore = TextBox11.Text
            Dim visitorrec = New RectangleF(basex + 13, basey + 20, 150, 50)
            Dim homerec = New RectangleF(basex + 13, basey + 70, 150, 50)
            Dim visitorscorerec = New RectangleF(basex + 550, basey + 20, 300, 50)
            Dim homescorerec = New RectangleF(basex + 550, basey + 70, 300, 50)
            Dim herrorrec = New RectangleF(basex + 650, basey + 70, 300, 50)
            Dim herror = TextBox39.Text
            Dim verrorrec = New RectangleF(basex + 650, basey + 20, 300, 50)
            Dim verror = TextBox38.Text
            Dim vhits = TextBox36.Text
            Dim vhitsrec = New RectangleF(basex + 605, basey + 20, 150, 50)
            Dim hhits = TextBox37.Text
            Dim hhitsrec = New RectangleF(basex + 605, basey + 70, 150, 50)
            Dim h1s = TextBox16.Text
            Dim h1srec = New RectangleF(basex + 205, basey + 70, 150, 50)
            Dim h2s = TextBox18.Text
            Dim h2srec = New RectangleF(basex + 255, basey + 70, 150, 50)
            Dim h3s = TextBox20.Text
            Dim h3srec = New RectangleF(basex + 305, basey + 70, 150, 50)
            Dim h4s = TextBox22.Text
            Dim h4srec = New RectangleF(basex + 355, basey + 70, 150, 50)
            Dim h5s = TextBox24.Text
            Dim h5srec = New RectangleF(basex + 405, basey + 70, 150, 50)
            Dim h6s = TextBox26.Text
            Dim h6srec = New RectangleF(basex + 455, basey + 70, 150, 50)
            Dim h7s = TextBox28.Text
            Dim h7srec = New RectangleF(basex + 505, basey + 70, 150, 50)
            Dim h8s = TextBox30.Text
            Dim h8srec = New RectangleF(basex + 555, basey + 70, 150, 50)
            Dim h9s = TextBox32.Text
            Dim h9srec = New RectangleF(basex + 605, basey + 70, 150, 50)
            Dim v1s = TextBox17.Text
            Dim v1srec = New RectangleF(basex + 205, basey + 20, 150, 50)
            Dim v2s = TextBox19.Text
            Dim v2srec = New RectangleF(basex + 255, basey + 20, 150, 50)
            Dim v3s = TextBox21.Text
            Dim v3srec = New RectangleF(basex + 305, basey + 20, 150, 50)
            Dim v4s = TextBox23.Text
            Dim v4srec = New RectangleF(basex + 355, basey + 20, 150, 50)
            Dim v5s = TextBox25.Text
            Dim v5srec = New RectangleF(basex + 405, basey + 20, 150, 50)
            Dim v6s = TextBox27.Text
            Dim v6srec = New RectangleF(basex + 455, basey + 20, 150, 50)
            Dim v7s = TextBox29.Text
            Dim v7srec = New RectangleF(basex + 505, basey + 20, 150, 50)
            Dim v8s = TextBox31.Text
            Dim v8srec = New RectangleF(basex + 555, basey + 20, 150, 50)
            Dim v9s = TextBox33.Text
            Dim v9srec = New RectangleF(basex + 605, basey + 20, 150, 50)
            Dim inning = TextBox12.Text


            Dim ms As New MemoryStream
            Dim objBitmap As New Bitmap(1920, 1080)
            Dim e2 As Graphics = Graphics.FromImage(objBitmap)
            If inning < 8 Then
                e2.DrawImage(Bitmap.FromFile(pathOnly & "\baseball\scoreboard7.png"), New Rectangle(basex, basey, basew, baseh))
            End If
            If inning = 8 Then
                basex = basex - 24
                basew = basew + 49
                homerec = New RectangleF(basex + 13, basey + 70, 150, 50)
                visitorrec = New RectangleF(basex + 13, basey + 20, 150, 50)
                visitorscorerec = New RectangleF(basex + 600, basey + 20, 300, 50)
                homescorerec = New RectangleF(basex + 600, basey + 70, 300, 50)
                If TextBox10.Text.Length > 1 Then
                    homescorerec = New RectangleF(basex + 540, basey + 20, 300, 50)
                End If
                If TextBox11.Text.Length > 1 Then
                    visitorscorerec = New RectangleF(basex + 540, basey + 70, 300, 50)
                End If
                e2.DrawImage(Bitmap.FromFile(pathOnly & "\baseball\scoreboard8.png"), New Rectangle(basex, basey, basew, baseh))
            End If
            If inning = 9 Then
                basex = basex - 24 - 21
                basew = basew + 49 + 42
                homerec = New RectangleF(basex + 13, basey + 70, 150, 50)
                visitorrec = New RectangleF(basex + 13, basey + 20, 150, 50)
                visitorscorerec = New RectangleF(basex + 650, basey + 20, 300, 50)
                homescorerec = New RectangleF(basex + 650, basey + 70, 300, 50)

                e2.DrawImage(Bitmap.FromFile(pathOnly & "\baseball\scoreboard9.png"), New Rectangle(basex, basey, basew, baseh))
            End If
            If inning > 9 Then
                MessageBox.Show("Sorry, app currently doesn't support this many innings... complain to Jordan, it's his fault. 100% Jordan's fault. Call his cell 941-345-6226 and complain immediately.")
                Exit Sub
            End If
            Using GradientGraphic = e2
                Dim visitorrectangle As New RectangleF(basex + 18, basey + 64, 166, 49)
                Using GradientBrush As New LinearGradientBrush(visitorrectangle, Placement.Button7.BackColor, Placement.Button8.BackColor, LinearGradientMode.BackwardDiagonal)
                    GradientGraphic.FillRectangle(GradientBrush, visitorrectangle)
                End Using
                Dim HomeRectangle As New RectangleF(basex + 18, basey + 15, 166, 49)
                Using GradientBrush As New LinearGradientBrush(HomeRectangle, Placement.Button9.BackColor, Placement.Button10.BackColor, LinearGradientMode.BackwardDiagonal)
                    GradientGraphic.FillRectangle(GradientBrush, HomeRectangle)
                End Using

                e2.DrawString(hhits, homefont, Hbrushcolor, hhitsrec)
                e2.DrawString(vhits, visitorfont, Vbrushcolor, vhitsrec)
                e2.DrawString(h1s, homefont, Hbrushcolor, h1srec)
                e2.DrawString(h2s, homefont, Hbrushcolor, h2srec)
                e2.DrawString(h3s, homefont, Hbrushcolor, h3srec)
                e2.DrawString(h4s, homefont, Hbrushcolor, h4srec)
                e2.DrawString(h5s, homefont, Hbrushcolor, h5srec)
                e2.DrawString(h6s, homefont, Hbrushcolor, h6srec)
                e2.DrawString(h7s, homefont, Hbrushcolor, h7srec)
                e2.DrawString(v1s, visitorfont, Vbrushcolor, v1srec)
                e2.DrawString(v2s, visitorfont, Vbrushcolor, v2srec)
                e2.DrawString(v3s, visitorfont, Vbrushcolor, v3srec)
                e2.DrawString(v4s, visitorfont, Vbrushcolor, v4srec)
                e2.DrawString(v5s, visitorfont, Vbrushcolor, v5srec)
                e2.DrawString(v6s, visitorfont, Vbrushcolor, v6srec)
                e2.DrawString(v7s, visitorfont, Vbrushcolor, v7srec)
                If inning = 8 Then
                    e2.DrawString(h8s, homefont, Hbrushcolor, h8srec)
                    e2.DrawString(v8s, visitorfont, Vbrushcolor, v8srec)
                End If
                If inning = 9 Then
                    e2.DrawString(h9s, homefont, Hbrushcolor, h9srec)
                    e2.DrawString(v9s, visitorfont, Vbrushcolor, v9srec)
                End If
                e2.DrawString(verror, visitorfont, Vbrushcolor, verrorrec)
                e2.DrawString(herror, homefont, Hbrushcolor, herrorrec)
                e2.DrawString(home, homefont, Hbrushcolor, homerec)
                e2.DrawString(visitor, visitorfont, Vbrushcolor, visitorrec)
                e2.DrawString(homescore, homefont, Hbrushcolor, homescorerec)
                e2.DrawString(visitorscore, visitorfont, Vbrushcolor, visitorscorerec)
            End Using
            Try
                objBitmap.Save(FileName & ".png", ImageFormat.Png)
            Finally
                objBitmap.Dispose()
                e2.Dispose()
            End Try
            Dim fs As System.IO.FileStream
            ' Specify a valid picture file path on your computer.
            fs = New System.IO.FileStream(FileName & ".png",
                 IO.FileMode.Open, IO.FileAccess.Read)
            PictureBox1.Image = System.Drawing.Image.FromStream(fs)
            fs.Close()
        Catch ex As Exception
            Application.DoEvents()
        End Try

    End Sub
    Private Sub Button34_Click(sender As Object, e As EventArgs) Handles Button34.Click
        genscoreboard()
    End Sub

    Private Sub Button36_Click(sender As Object, e As EventArgs) Handles Button36.Click
        TextBox36.Text += 1
        If CheckBox6.Checked = True Then
            Button30.PerformClick()
        End If
    End Sub

    Private Sub Button35_Click(sender As Object, e As EventArgs) Handles Button35.Click
        TextBox38.Text += 1
    End Sub

    Private Sub Button37_Click(sender As Object, e As EventArgs) Handles Button37.Click
        TextBox37.Text += 1
        If CheckBox6.Checked = True Then
            Button30.PerformClick()
        End If
    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs) Handles Button38.Click
        TextBox39.Text += 1
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        Dim inning = TextBox12.Text
        Dim hinning1 = TextBox16.Text
        Dim hinning2 = TextBox18.Text
        Dim hinning3 = TextBox20.Text
        Dim hinning4 = TextBox22.Text
        Dim hinning5 = TextBox24.Text
        Dim hinning6 = TextBox26.Text
        Dim hinning7 = TextBox28.Text
        Dim hinning8 = TextBox30.Text
        Dim hinning9 = TextBox32.Text
        Dim vinning1 = TextBox17.Text
        Dim vinning2 = TextBox19.Text
        Dim vinning3 = TextBox21.Text
        Dim vinning4 = TextBox23.Text
        Dim vinning5 = TextBox25.Text
        Dim vinning6 = TextBox27.Text
        Dim vinning7 = TextBox29.Text
        Dim vinning8 = TextBox31.Text
        Dim vinning9 = TextBox33.Text
        If inning = 1 Then
            If RadioButton2.Checked = True Then
                If vinning1 = "" Then
                    TextBox17.Text = "0"
                End If
            End If
        End If
        If inning = 2 Then
            If RadioButton1.Checked = True Then
                If hinning1 = "" Then
                    TextBox16.Text = "0"
                End If
                If RadioButton2.Checked = True Then
                    If vinning2 = "" Then
                        TextBox19.Text = "0"
                    End If
                End If
            End If
        End If
        If inning = 3 Then
            If RadioButton1.Checked = True Then
                If hinning2 = "" Then
                    TextBox18.Text = "0"
                End If
                If RadioButton2.Checked = True Then
                    If vinning3 = "" Then
                        TextBox21.Text = "0"
                    End If
                End If
            End If
        End If
        If inning = 4 Then
            If RadioButton1.Checked = True Then
                If hinning3 = "" Then
                    TextBox20.Text = "0"
                End If
                If RadioButton2.Checked = True Then
                    If vinning4 = "" Then
                        TextBox23.Text = "0"
                    End If
                End If
            End If
        End If
        If inning = 5 Then
            If RadioButton1.Checked = True Then
                If hinning4 = "" Then
                    TextBox22.Text = "0"
                End If
                If RadioButton2.Checked = True Then
                    If vinning5 = "" Then
                        TextBox25.Text = "0"
                    End If
                End If
            End If
        End If
        If inning = 6 Then
            If RadioButton1.Checked = True Then
                If hinning5 = "" Then
                    TextBox24.Text = "0"
                End If
                If RadioButton2.Checked = True Then
                    If vinning6 = "" Then
                        TextBox27.Text = "0"
                    End If
                End If
            End If
        End If
        If inning = 7 Then
            If RadioButton1.Checked = True Then
                If hinning6 = "" Then
                    TextBox26.Text = "0"
                End If
                If RadioButton2.Checked = True Then
                    If vinning7 = "" Then
                        TextBox29.Text = "0"
                    End If
                End If
            End If
        End If
        If inning = 8 Then
            If RadioButton1.Checked = True Then
                If hinning7 = "" Then
                    TextBox28.Text = "0"
                End If
                If RadioButton2.Checked = True Then
                    If vinning8 = "" Then
                        TextBox31.Text = "0"
                    End If
                End If
            End If
        End If
        If inning = 9 Then
            If RadioButton1.Checked = True Then
                If hinning8 = "" Then
                    TextBox30.Text = "0"
                End If
                If RadioButton2.Checked = True Then
                    If vinning9 = "" Then
                        TextBox33.Text = "0"
                    End If
                End If
            End If
        End If
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If
    End Sub

    Private Sub TextBox10_TextChanged(sender As Object, e As EventArgs) Handles TextBox10.TextChanged
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If
    End Sub

    Private Sub TextBox11_TextChanged(sender As Object, e As EventArgs) Handles TextBox11.TextChanged
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If
    End Sub

    Private Sub TextBox12_TextChanged(sender As Object, e As EventArgs) Handles TextBox12.TextChanged
        If CheckBox5.Checked = True Then
            Button31.PerformClick()
        End If
    End Sub

    Private Sub Button39_Click(sender As Object, e As EventArgs) Handles Button39.Click
        Dim inning = TextBox12.Text
        If RadioButton1.Checked Then

            If inning = 1 Then
                If TextBox17.Text = "" Then
                    TextBox17.Text = 0
                End If
                TextBox17.Text += 1
            End If
            If inning = 2 Then
                If TextBox19.Text = "" Then
                    TextBox19.Text = 0
                End If
                TextBox19.Text += 1
            End If
            If inning = 3 Then
                If TextBox21.Text = "" Then
                    TextBox21.Text = 0
                End If
                TextBox21.Text += 1
            End If
            If inning = 4 Then
                If TextBox23.Text = "" Then
                    TextBox23.Text = 0
                End If
                TextBox23.Text += 1
            End If
            If inning = 5 Then
                If TextBox25.Text = "" Then
                    TextBox25.Text = 0
                End If
                TextBox25.Text += 1
            End If
            If inning = 6 Then
                If TextBox27.Text = "" Then
                    TextBox27.Text = 0
                End If
                TextBox27.Text += 1
            End If
            If inning = 7 Then
                If TextBox29.Text = "" Then
                    TextBox29.Text = 0
                End If
                TextBox29.Text += 1
            End If
            If inning = 8 Then
                If TextBox31.Text = "" Then
                    TextBox31.Text = 0
                End If
                TextBox31.Text += 1
            End If
            If inning = 9 Then
                If TextBox33.Text = "" Then
                    TextBox33.Text = 0
                End If
                TextBox33.Text += 1
            End If
            calcvisitors()
        Else

            If inning = 1 Then
                If TextBox16.Text = "" Then
                    TextBox16.Text = 0
                End If
                TextBox16.Text += 1
            End If
            If inning = 2 Then
                If TextBox18.Text = "" Then
                    TextBox18.Text = 0
                End If
                TextBox18.Text += 1
            End If
            If inning = 3 Then
                If TextBox20.Text = "" Then
                    TextBox20.Text = 0
                End If
                TextBox20.Text += 1
            End If
            If inning = 4 Then
                If TextBox22.Text = "" Then
                    TextBox22.Text = 0
                End If
                TextBox22.Text += 1
            End If
            If inning = 5 Then
                If TextBox24.Text = "" Then
                    TextBox24.Text = 0
                End If
                TextBox24.Text += 1
            End If
            If inning = 6 Then
                If TextBox26.Text = "" Then
                    TextBox26.Text = 0
                End If
                TextBox26.Text += 1
            End If
            If inning = 7 Then
                If TextBox28.Text = "" Then
                    TextBox28.Text = 0
                End If
                TextBox28.Text += 1
            End If
            If inning = 8 Then
                If TextBox30.Text = "" Then
                    TextBox30.Text = 0
                End If
                TextBox30.Text += 1
            End If
            If inning = 9 Then
                If TextBox32.Text = "" Then
                    TextBox32.Text = 0
                End If
                TextBox32.Text += 1
            End If
            calchome()
        End If
    End Sub
    Private Sub calchome()
        Dim inning1 = TextBox16.Text
        Dim inning2 = TextBox18.Text
        Dim inning3 = TextBox20.Text
        Dim inning4 = TextBox22.Text
        Dim inning5 = TextBox24.Text
        Dim inning6 = TextBox26.Text
        Dim inning7 = TextBox28.Text
        Dim inning8 = TextBox30.Text
        Dim inning9 = TextBox32.Text

        If inning1 = "" Then inning1 = 0
        If inning2 = "" Then inning2 = 0
        If inning3 = "" Then inning3 = 0
        If inning4 = "" Then inning4 = 0
        If inning5 = "" Then inning5 = 0
        If inning6 = "" Then inning6 = 0
        If inning7 = "" Then inning7 = 0
        If inning8 = "" Then inning8 = 0
        If inning9 = "" Then inning9 = 0

        Try

            TextBox11.Text = Convert.ToInt32(inning1) + Convert.ToInt32(inning2) + Convert.ToInt32(inning3) + Convert.ToInt32(inning4) + Convert.ToInt32(inning5) + Convert.ToInt32(inning6) + Convert.ToInt32(inning7) + Convert.ToInt32(inning8) + Convert.ToInt32(inning9)

        Catch ex As Exception

        End Try
    End Sub
    Private Sub calcvisitors()
        Dim inning1 = TextBox17.Text
        Dim inning2 = TextBox19.Text
        Dim inning3 = TextBox21.Text
        Dim inning4 = TextBox23.Text
        Dim inning5 = TextBox25.Text
        Dim inning6 = TextBox27.Text
        Dim inning7 = TextBox29.Text
        Dim inning8 = TextBox31.Text
        Dim inning9 = TextBox33.Text

        If inning1 = "" Then inning1 = 0
        If inning2 = "" Then inning2 = 0
        If inning3 = "" Then inning3 = 0
        If inning4 = "" Then inning4 = 0
        If inning5 = "" Then inning5 = 0
        If inning6 = "" Then inning6 = 0
        If inning7 = "" Then inning7 = 0
        If inning8 = "" Then inning8 = 0
        If inning9 = "" Then inning9 = 0

        Try

            TextBox10.Text = Convert.ToInt32(inning1) + Convert.ToInt32(inning2) + Convert.ToInt32(inning3) + Convert.ToInt32(inning4) + Convert.ToInt32(inning5) + Convert.ToInt32(inning6) + Convert.ToInt32(inning7) + Convert.ToInt32(inning8) + Convert.ToInt32(inning9)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox17_TextChanged(sender As Object, e As EventArgs) Handles TextBox17.TextChanged
        calcvisitors()
    End Sub

    Private Sub TextBox19_TextChanged(sender As Object, e As EventArgs) Handles TextBox19.TextChanged
        calcvisitors()
    End Sub

    Private Sub TextBox21_TextChanged(sender As Object, e As EventArgs) Handles TextBox21.TextChanged
        calcvisitors()
    End Sub

    Private Sub TextBox23_TextChanged(sender As Object, e As EventArgs) Handles TextBox23.TextChanged
        calcvisitors()
    End Sub

    Private Sub TextBox25_TextChanged(sender As Object, e As EventArgs) Handles TextBox25.TextChanged
        calcvisitors()
    End Sub

    Private Sub TextBox27_TextChanged(sender As Object, e As EventArgs) Handles TextBox27.TextChanged
        calcvisitors()
    End Sub

    Private Sub TextBox29_TextChanged(sender As Object, e As EventArgs) Handles TextBox29.TextChanged
        calcvisitors()
    End Sub

    Private Sub TextBox31_TextChanged(sender As Object, e As EventArgs) Handles TextBox31.TextChanged
        calcvisitors()
    End Sub

    Private Sub TextBox33_TextChanged(sender As Object, e As EventArgs) Handles TextBox33.TextChanged
        calcvisitors()
    End Sub

    Private Sub TextBox16_TextChanged(sender As Object, e As EventArgs) Handles TextBox16.TextChanged
        calchome()
    End Sub

    Private Sub TextBox18_TextChanged(sender As Object, e As EventArgs) Handles TextBox18.TextChanged
        calchome()
    End Sub

    Private Sub TextBox20_TextChanged(sender As Object, e As EventArgs) Handles TextBox20.TextChanged
        calchome()
    End Sub

    Private Sub TextBox22_TextChanged(sender As Object, e As EventArgs) Handles TextBox22.TextChanged
        calchome()
    End Sub

    Private Sub TextBox24_TextChanged(sender As Object, e As EventArgs) Handles TextBox24.TextChanged
        calchome()
    End Sub

    Private Sub TextBox26_TextChanged(sender As Object, e As EventArgs) Handles TextBox26.TextChanged
        calchome()
    End Sub

    Private Sub TextBox28_TextChanged(sender As Object, e As EventArgs) Handles TextBox28.TextChanged
        calchome()
    End Sub

    Private Sub TextBox30_TextChanged(sender As Object, e As EventArgs) Handles TextBox30.TextChanged
        calchome()
    End Sub

    Private Sub TextBox32_TextChanged(sender As Object, e As EventArgs) Handles TextBox32.TextChanged
        calchome()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        Try

 
        Dim inning = TextBox12.Text
        Dim hinning1 = TextBox16.Text
        Dim hinning2 = TextBox18.Text
        Dim hinning3 = TextBox20.Text
        Dim hinning4 = TextBox22.Text
        Dim hinning5 = TextBox24.Text
        Dim hinning6 = TextBox26.Text
        Dim hinning7 = TextBox28.Text
        Dim hinning8 = TextBox30.Text
        Dim hinning9 = TextBox32.Text
        Dim vinning1 = TextBox17.Text
        Dim vinning2 = TextBox19.Text
        Dim vinning3 = TextBox21.Text
        Dim vinning4 = TextBox23.Text
        Dim vinning5 = TextBox25.Text
        Dim vinning6 = TextBox27.Text
        Dim vinning7 = TextBox29.Text
        Dim vinning8 = TextBox31.Text
        Dim vinning9 = TextBox33.Text
        If inning = 1 Then
            If RadioButton2.Checked = True Then
                If vinning1 = "" Then
                    TextBox17.Text = "0"
                End If
            End If
        End If
        If inning = 2 Then
            If RadioButton1.Checked = True Then
                If hinning1 = "" Then
                        TextBox16.Text = "0"
                    End If
                Else
                    If vinning2 = "" Then
                        TextBox19.Text = "0"
                    End If
                End If
            End If
            If inning = 3 Then
                If RadioButton1.Checked = True Then
                    If hinning2 = "" Then
                        TextBox18.Text = "0"
                    End If
                Else
                    If vinning3 = "" Then
                        TextBox21.Text = "0"
                    End If
                End If
            End If
            If inning = 4 Then
                If RadioButton1.Checked = True Then
                    If hinning3 = "" Then
                        TextBox20.Text = "0"
                    End If
                Else
                    If vinning4 = "" Then
                        TextBox23.Text = "0"
                    End If
                End If
            End If
            If inning = 5 Then
                If RadioButton1.Checked = True Then
                    If hinning4 = "" Then
                        TextBox22.Text = "0"
                    End If
                Else
                    If vinning5 = "" Then
                        TextBox25.Text = "0"
                    End If
                End If
            End If
            If inning = 6 Then
                If RadioButton1.Checked = True Then
                    If hinning5 = "" Then
                        TextBox24.Text = "0"
                    End If
                Else
                    If vinning6 = "" Then
                        TextBox27.Text = "0"
                    End If
                End If
            End If
            If inning = 7 Then
                If RadioButton1.Checked = True Then
                    If hinning6 = "" Then
                        TextBox26.Text = "0"
                    End If
                Else
                    If vinning7 = "" Then
                        TextBox29.Text = "0"
                    End If
                End If
            End If
            If inning = 8 Then
                If RadioButton1.Checked = True Then
                    If hinning7 = "" Then
                        TextBox28.Text = "0"
                    End If
                Else
                    If vinning8 = "" Then
                        TextBox31.Text = "0"
                    End If
                End If
            End If
            If inning = 9 Then
                If RadioButton1.Checked = True Then
                    If hinning8 = "" Then
                        TextBox30.Text = "0"
                    End If
                Else
                    If vinning9 = "" Then
                        TextBox33.Text = "0"
                    End If
                End If
            End If
            If CheckBox5.Checked = True Then
                Button31.PerformClick()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
