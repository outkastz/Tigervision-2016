'Copyright 2012-2013 Orange Owl Solutions - www.orangeowlsolutions.com.   

'Oauth Twitter Example is free software: you can redistribute it and/or modify
'it under the terms of the Lesser GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.

'Oauth Twitter Example is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'Lesser GNU General Public License for more details.

'You should have received a copy of the GNU General Public License
'along with Oauth Twitter Example.  If not, see <http://www.gnu.org/licenses/>.




Imports System.Net
Imports System.Text
Imports System.Security.Cryptography

Public Class userRequest

    'You may read about "Authorizing a Twitter request" on 
    'https://dev.twitter.com/docs/auth/authorizing-request
    'and get accurate parameters description


    'You may get following data from https://dev.twitter.com/docs/auth/tokens-devtwittercom
    'activating your twitter application account
    Private oauth_consumer_key As String
    Private consumer_secret As String
    Private oauth_token As String
    Private oauth_token_secret As String


    Private oauth_timestamp As String 'The number of seconds since the Unix epoch at the point the request is generated 
    Private oauth_nonce As String 'An hash identifing the request

    'We are going to invoke the search API 
    Private requestUrl As String = "https://api.twitter.com/1.1/statuses/home_timeline.json"
    Private requestMethod As String = "GET"
    Private oauth_version As String = "1.0"
    Private oauth_signature_method As String = "HMAC-SHA1"


    ''' <summary>
    ''' Set a new request with Twitter App parameters
    ''' </summary>
    ''' <param name="oauth_consumer_key_in"></param>
    ''' <param name="consumer_secret_in"></param>
    ''' <param name="oauth_token_in"></param>
    ''' <param name="oauth_token_secret_in"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal oauth_consumer_key_in As String, ByVal consumer_secret_in As String, ByVal oauth_token_in As String, ByVal oauth_token_secret_in As String)
        oauth_consumer_key = oauth_consumer_key_in
        consumer_secret = consumer_secret_in
        oauth_token = oauth_token_in
        oauth_token_secret = oauth_token_secret_in
    End Sub


    ''' <summary>
    ''' The oauth_nonce parameter is a unique token your application should generate for each unique request.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function setNounce() As String
        Return Convert.ToBase64String(New ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()))
    End Function

    ''' <summary>
    ''' The oauth_timestamp parameter indicates when the request was created. This value should be the number of seconds since the Unix epoch at the point the request is generated
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function setTimeStamp() As String
        Dim timeSpan As TimeSpan = DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
        Return Convert.ToInt64(timeSpan.TotalSeconds).ToString()
    End Function

    ''' <summary>
    ''' Performs the Percent-encoding, also known as URL encoding
    ''' </summary>
    ''' <param name="StringToEncode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function percEncode(ByVal StringToEncode As String) As String
        Dim utf8Encoding As New System.Text.UTF8Encoding(True)
        Dim encodedString() As Byte
        encodedString = utf8Encoding.GetBytes(StringToEncode)
        StringToEncode = utf8Encoding.GetString(encodedString)
        Return System.Uri.EscapeDataString(StringToEncode)
    End Function


    ''' <summary>
    ''' The oauth_signature_method used by Twitter is HMAC-SHA1. This value should be used for any authorized request sent to Twitter's API.
    ''' </summary>
    ''' <param name="consumer_key_in"></param>
    ''' <param name="oauth_nonce_in"></param>
    ''' <param name="oauth_timestamp_in"></param>
    ''' <param name="oauth_token_in"></param>
    ''' <param name="consumer_secret_in"></param>
    ''' <param name="oauth_token_Secret_in"></param>
    ''' <param name="query">The string to search</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getSignature(ByVal consumer_key_in As String, ByVal oauth_nonce_in As String, ByVal oauth_timestamp_in As String, ByVal oauth_token_in As String, ByVal consumer_secret_in As String, ByVal oauth_token_Secret_in As String) As String
        Dim myKey As String = percEncode(consumer_secret_in) + "&" + percEncode(oauth_token_Secret_in)
        Dim percURL = percEncode(requestUrl)
        Dim ris As String = ""
        Dim signature_base As String = ""
        Dim mypar As String = createAuthParam(consumer_key_in, oauth_nonce_in, oauth_timestamp_in, oauth_token_in)
        signature_base = requestMethod & "&" & percURL & "&" & percEncode(mypar)
        ris = HashString(signature_base, myKey)
        Return ris
    End Function

    ''' <summary>
    ''' In cryptography, a keyed-hash message authentication code (HMAC) is a specific construction for calculating a message authentication code (MAC) involving a cryptographic hash function in combination with a secret cryptographic key. (from Wikipedia)
    ''' </summary>
    ''' <param name="StringToHash"></param>
    ''' <param name="Key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HashString(ByVal StringToHash As String, ByVal Key As String) As String
        Dim oauth_signature As String
        Dim hasher As HMACSHA1 = New HMACSHA1(ASCIIEncoding.ASCII.GetBytes(Key))
        Using hasher
            oauth_signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(StringToHash)))
        End Using
        Return oauth_signature
    End Function


    ''' <summary>
    ''' Create the concatenation of parameters for OAuth request.
    ''' </summary>
    ''' <param name="consumer_key_in"></param>
    ''' <param name="nonce_in"></param>
    ''' <param name="oauth_timestamp_in"></param>
    ''' <param name="oauth_token_in"></param>
    ''' <param name="query"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function createAuthParam(ByVal consumer_key_in As String, ByVal nonce_in As String, ByVal oauth_timestamp_in As String, ByVal oauth_token_in As String) As String
        Dim paramString As String = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}"
        Dim authString As String = String.Format(paramString, percEncode(consumer_key_in), percEncode(nonce_in), oauth_signature_method, oauth_timestamp_in, oauth_token_in, oauth_version)
        Return authString
    End Function

    ''' <summary>
    ''' format and set the main request parameters
    ''' </summary>
    ''' <param name="searchInput">Is the string to search</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function reqParam(ByVal searchInput As String) As String
        Return "q=" & percEncode(searchInput) & "&result_type=popular"
    End Function





    ''' <summary>
    ''' Makes the main HTTP request.
    ''' </summary>
    ''' <param name="query"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function makeRequest()
        Dim requestResult As String = ""
        oauth_nonce = setNounce()
        oauth_timestamp = setTimeStamp()
        Dim oauth_signature As String = getSignature(oauth_consumer_key, oauth_nonce, oauth_timestamp, oauth_token, consumer_secret, oauth_token_secret)


        'Set the authorization header
        Dim headerFormat As String = "OAuth oauth_nonce=""{0}"", oauth_signature_method=""{1}"", oauth_timestamp=""{2}"", oauth_consumer_key=""{3}"", oauth_token=""{4}"", oauth_signature=""{5}"", oauth_version=""{6}"""
        Dim authHeader As String = String.Format(headerFormat, Uri.EscapeDataString(oauth_nonce), Uri.EscapeDataString(oauth_signature_method), Uri.EscapeDataString(oauth_timestamp), Uri.EscapeDataString(oauth_consumer_key), Uri.EscapeDataString(oauth_token), Uri.EscapeDataString(oauth_signature), Uri.EscapeDataString(oauth_version))


        'set the request
        ServicePointManager.Expect100Continue = False
        Dim request As WebRequest
        Dim response As HttpWebResponse
        Dim streamReader As System.IO.StreamReader
        Dim encode As Encoding = System.Text.Encoding.GetEncoding("utf-8")
        request = WebRequest.Create(requestUrl)
        request.Timeout = -1
        request.Headers.Add("Authorization", authHeader)

        Try
            response = DirectCast(request.GetResponse(), HttpWebResponse)
            streamReader = New System.IO.StreamReader(response.GetResponseStream(), encode)
            requestResult = streamReader.ReadLine()
            request.Abort()
            streamReader.Close()
            streamReader = Nothing
            response.Close()
            response = Nothing
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

        Return requestResult
    End Function










End Class
