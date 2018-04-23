Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports DevExpress.Web

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Private imageIndicies As New Dictionary(Of Integer, String)()
    Protected Sub ASPxImageGallery1_ItemDataBound(ByVal source As Object, ByVal e As ImageGalleryItemEventArgs)
        e.Item.Text = "Text " & e.Item.Index
        imageIndicies.Add(e.Item.Index, GetName(e.Item.ImageUrl))
    End Sub

    Protected Sub ASPxImageGallery1_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gallery = TryCast(sender, ASPxImageGallery)
        gallery.JSProperties("cp_items") = imageIndicies
    End Sub
    Private Function GetName(ByVal imageUrl As String) As String
        Dim nameWithPostfix = imageUrl.Split(New String() { "\" }, StringSplitOptions.None).Last()
        Return nameWithPostfix.Split(New String() { "?" }, StringSplitOptions.None).First()
    End Function

    Protected Sub ASPxImageGallery1_CustomCallback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        imageIndicies.Clear()
        Dim fileName As String
        For Each item In ASPxHiddenField1
            Dim cacheFolderPath As String = Server.MapPath(ASPxImageGallery1.SettingsFolder.ImageCacheFolder)
            Dim cachedImages() As String = Directory.GetFiles(cacheFolderPath, "*" & item.Key & "*", SearchOption.AllDirectories)
            For i As Integer = 0 To cachedImages.Length - 1
                'Deleting image files in a running online sample is not allowed, uncomment this line to test deleting on your local machine
                'File.Delete(cachedImages[i]);
            Next i
            fileName = Server.MapPath(Path.Combine(ASPxImageGallery1.SettingsFolder.ImageSourceFolder, item.Key))
            If File.Exists(fileName) Then
                'Deleting image files in a running online sample is not allowed, uncomment this line to test deleting on your local machine
                'File.Delete(fileName);
            End If
        Next item
        ASPxImageGallery1.UpdateImageCacheFolder()
    End Sub
End Class