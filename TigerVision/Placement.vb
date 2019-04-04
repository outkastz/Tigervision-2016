Imports System.IO
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D

Public Class Placement
    Dim path As String
    Dim fullpath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
    Dim pathOnly As String = My.Computer.FileSystem.GetParentPath(fullpath)
    Private Sub Placement_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        gendiamond(False)
    End Sub
    Private Sub gendiamond(score As Boolean)




        Try
            Dim FileName As String = pathOnly & "\img"
            Dim ms As New MemoryStream
            Dim objBitmap As New Bitmap(1920, 1080)
            Dim e2 As Graphics = Graphics.FromImage(objBitmap)
            Dim baseimg As String = "empty"
            e2.DrawImage(Bitmap.FromFile(pathOnly & "\baseball\" & baseimg & ".png"), New Rectangle(TextBox1.Text, TextBox2.Text, TextBox3.Text, TextBox4.Text))
            Dim VisitorRectangle As New Rectangle(TextBox1.Text + 15, TextBox2.Text + 15, 166, 49)
            Dim HomeRectangle As New RectangleF(TextBox1.Text + 15, TextBox2.Text + 64, 166, 49)
            Using GradientGraphic = e2
                Using GradientBrush As New LinearGradientBrush(HomeRectangle, Button7.BackColor, Button8.BackColor, LinearGradientMode.BackwardDiagonal)
                    GradientGraphic.FillRectangle(GradientBrush, HomeRectangle)
                End Using
                Using GradientBrush As New LinearGradientBrush(VisitorRectangle, Button9.BackColor, Button10.BackColor, LinearGradientMode.BackwardDiagonal)
                    GradientGraphic.FillRectangle(GradientBrush, VisitorRectangle)
                End Using
            End Using
            Try
                objBitmap.Save(FileName & ".png", ImageFormat.Png)
            Finally
                objBitmap.Dispose()
                e2.Dispose()
            End Try
            Dim fs As System.IO.FileStream
            fs = New System.IO.FileStream(FileName & ".png",
                 IO.FileMode.Open, IO.FileAccess.Read)
            Me.PictureBox1.Image = System.Drawing.Image.FromStream(fs)
            fs.Close()
        Catch
        End Try
    End Sub
    Private Sub gensingleline()

        Try
            Dim FileName As String = pathOnly & "\img"
            Dim ms As New MemoryStream
            Dim objBitmap As New Bitmap(1920, 1080)
            Dim e2 As Graphics = Graphics.FromImage(objBitmap)
            Dim baseimg As String = "empty"
            e2.DrawImage(Bitmap.FromFile(pathOnly & "\single-line-src.png"), New Rectangle(TextBox5.Text, TextBox6.Text, TextBox7.Text, TextBox8.Text))
            Try
                objBitmap.Save(FileName & ".png", ImageFormat.Png)
            Finally
                objBitmap.Dispose()
                e2.Dispose()
            End Try
            Dim fs As System.IO.FileStream
            fs = New System.IO.FileStream(FileName & ".png",
                 IO.FileMode.Open, IO.FileAccess.Read)
            Me.PictureBox1.Image = System.Drawing.Image.FromStream(fs)
            fs.Close()
        Catch
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        gensingleline()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If FontDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            RichTextBox1.ForeColor = FontDialog1.Color
            RichTextBox1.Font = FontDialog1.Font
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If FontDialog3.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            RichTextBox3.ForeColor = FontDialog3.Color
            RichTextBox3.Font = FontDialog3.Font
        End If
    End Sub


    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If ColorDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            Button7.BackColor = ColorDialog1.Color
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If ColorDialog2.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            Button8.BackColor = ColorDialog2.Color
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If ColorDialog3.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            Button9.BackColor = ColorDialog3.Color
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If ColorDialog4.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            Button10.BackColor = ColorDialog4.Color
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If FontDialog5.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            RichTextBox5.ForeColor = FontDialog5.Color
            RichTextBox5.Font = FontDialog5.Font
        End If
    End Sub
End Class