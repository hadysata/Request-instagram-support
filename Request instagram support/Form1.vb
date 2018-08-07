Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1
    Dim c1 As New CookieContainer, c2 As New CookieContainer, c_text As String, uuid As String


    Private Sub post()
        Try
            Dim request As HttpWebRequest = HttpWebRequest.Create("https://i.instagram.com/api/v1/accounts/assisted_account_recovery/")
            request.Method = "POST"
            request.Accept = "*/*"
            request.Proxy = Nothing
            request.Headers.Add("X-IG-Connection-Type", "WiFi")
            request.Headers.Add("Accept-Language", "ar-SA;q=1, en-SA;q=0.9")
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
            request.CookieContainer = c1
            request.UserAgent = "Instagram 10.3.2 Android (18/4.3; 320dpi; 720x1280; Xiaomi; HM 1SW; armani; qcom; en_US)"
            request.Headers.Add("X-IG-Capabilities: 3wI=")

            uuid = Guid.NewGuid.ToString.ToUpper
            Dim data As String = "{""username_or_email"":""" & TextBox5.Text & """,""_uuid"":""" & uuid & """,""_uid"":""4668192093"",""device_id"":""" & uuid & """,""_csrftoken"":""" & c_text & """,""qe_version"":""ae_v1""}"
            Dim sb As New StringBuilder
            Try
                Dim secretkey As String = "5ad7d6f013666cc93c88fc8af940348bd067b68f0dce3c85122a923f4f74b251"
                Dim sha As New System.Security.Cryptography.HMACSHA256(System.Text.ASCIIEncoding.ASCII.GetBytes(secretkey))
                Dim Hash() As Byte = sha.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(data))
                sb = New StringBuilder(Hash.Length * 2)
                For Each B As Byte In Hash
                    sb.Append(Hex(B).PadLeft(2, "0"))
                Next
            Catch ex As Exception : End Try
            Dim postData As String = "ig_sig_key_version=4&signed_body=" & sb.ToString.ToLower & "." & Web.HttpUtility.UrlEncode(data)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            response.Close()
            If responseFromServer.Contains("show_login_support_form") = True Then
                post2()
            Else
                MsgBox("لا تستطيع ان تراسل على هذا الحساب", MsgBoxStyle.Critical)
            End If
        Catch ex As WebException
            MsgBox("لا تستطيع ان تراسل على هذا الحساب", MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub post2()
        Try

            Dim request As HttpWebRequest = HttpWebRequest.Create("https://i.instagram.com/api/v1/users/vetted_device_login_support/")
            request.Method = "POST"
            request.Accept = "*/*"
            request.Headers.Add("X-IG-Connection-Type", "WiFi")
            request.Headers.Add("Accept-Language", "ar-SA;q=1, en-SA;q=0.9")
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
            request.CookieContainer = c2
            request.UserAgent = "Instagram 10.3.2 Android (18/4.3; 320dpi; 720x1280; Xiaomi; HM 1SW; armani; qcom; en_US)"
            request.Headers.Add("X-IG-Capabilities: 3wI=")
            If CheckBox1.Checked = True Then
                Dim myProxy As New WebProxy("" & TextBox3.Text & ":" & TextBox4.Text, True)
                request.Proxy = myProxy
            Else
                request.Proxy = Nothing
            End If
            Dim data As String = "{""signup_email"":""" & TextBox1.Text & """,""additional_info"":""" & TextBox6.Text & """,""_uuid"":""" & uuid & """,""device_id"":""" & uuid & """,""_uid"":""4668192093"",""_csrftoken"":""" & c_text & """,""username"":""" & TextBox5.Text & """,""reason_failed"":""ACCOUNT_HACKED"",""contact_email"":""" & TextBox2.Text & """,""account_type"":""personal_without_photo""}"
            If ComboBox1.SelectedIndex = 0 Then
                data = data.Replace("personal_without_photo", "company")
            ElseIf ComboBox1.SelectedIndex = 1 Then
                data = data.Replace("personal_without_photo", "personal_with_photo")
            End If
            If ComboBox2.SelectedIndex = 0 Then
                data = data.Replace("ACCOUNT_HACKED", "FORGOT_EMAIL")
            ElseIf ComboBox2.SelectedIndex = 1 Then
                data = data.Replace("ACCOUNT_HACKED", "CANNOT_LOGIN_WITH_EMAIL")
            ElseIf ComboBox2.SelectedIndex = 3 Then
                data = data.Replace("ACCOUNT_HACKED", "OTHER")
            End If
            Dim sb As New StringBuilder
            Try
                Dim secretkey As String = "5ad7d6f013666cc93c88fc8af940348bd067b68f0dce3c85122a923f4f74b251"
                Dim sha As New System.Security.Cryptography.HMACSHA256(System.Text.ASCIIEncoding.ASCII.GetBytes(secretkey))
                Dim Hash() As Byte = sha.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(data))
                sb = New StringBuilder(Hash.Length * 2)
                For Each B As Byte In Hash
                    sb.Append(Hex(B).PadLeft(2, "0"))
                Next
            Catch ex As Exception : End Try
            Dim postData As String = "ig_sig_key_version=4&signed_body=" & sb.ToString.ToLower & "." & Web.HttpUtility.UrlEncode(data)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As HttpWebResponse = request.GetResponse()
            dataStream = response.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            reader.Close()
            dataStream.Close()
            response.Close()
            MsgBox("تم الارسال بنجاح", MsgBoxStyle.Information)
        Catch ex As WebException

            Dim response As WebResponse = ex.Response
            Dim statusCode As HttpStatusCode
            Dim ResponseText As String
            Dim httpResponse As HttpWebResponse = CType(response, HttpWebResponse)
            statusCode = httpResponse.StatusCode
            Dim myStreamReader As New StreamReader(response.GetResponseStream())
            Using (myStreamReader)
                ResponseText = myStreamReader.ReadToEnd
            End Using
            MsgBox(ResponseText, MsgBoxStyle.Critical)
        End Try
    End Sub
 
  
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
    End Sub

    Private Sub TextBox1_Click(sender As Object, e As EventArgs) Handles TextBox1.Click
        TextBox1.Text = Nothing
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        MsgBox("Thanks for using my free tool" & vbCrLf & "for more free tools" & vbCrLf & "instagram : @hadysata", MsgBoxStyle.Information)
        End
    End Sub

    Private Sub TextBox2_Click(sender As Object, e As EventArgs) Handles TextBox2.Click
        TextBox2.Text = Nothing
    End Sub

    Private Sub TextBox3_Click(sender As Object, e As EventArgs) Handles TextBox3.Click
        TextBox3.Text = Nothing
    End Sub

    Private Sub TextBox4_Click(sender As Object, e As EventArgs) Handles TextBox4.Click
        TextBox4.Text = Nothing
    End Sub


    Private Sub StudioButton1_Click(sender As Object, e As EventArgs) Handles StudioButton1.Click
        TextBox6.Text = TextBox6.Text.Replace("\\n", "")
        post()
    End Sub

    Private Sub TextBox5_Click(sender As Object, e As EventArgs) Handles TextBox5.Click
        TextBox5.Text = Nothing
    End Sub

  
    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        TextBox6.Text = TextBox6.Text.Replace("\\n", "")
    End Sub

    Private Sub StudioTheme1_Click(sender As Object, e As EventArgs) Handles StudioTheme1.Click

    End Sub
End Class
