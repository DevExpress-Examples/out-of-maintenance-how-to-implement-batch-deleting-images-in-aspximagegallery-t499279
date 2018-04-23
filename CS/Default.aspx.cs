using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevExpress.Web;

public partial class _Default : System.Web.UI.Page {
    Dictionary<int, string> imageIndicies = new Dictionary<int, string>();
    protected void ASPxImageGallery1_ItemDataBound(object source, ImageGalleryItemEventArgs e) {
        e.Item.Text = "Text " + e.Item.Index;
        imageIndicies.Add(e.Item.Index, GetName(e.Item.ImageUrl));
    }

    protected void ASPxImageGallery1_DataBound(object sender, EventArgs e) {
        var gallery = sender as ASPxImageGallery;
        gallery.JSProperties["cp_items"] = imageIndicies;
    }
    string GetName(string imageUrl) {
        var nameWithPostfix = imageUrl.Split(new string[] { "\\" }, StringSplitOptions.None).Last();
        return nameWithPostfix.Split(new string[] { "?" }, StringSplitOptions.None).First();
    }

    protected void ASPxImageGallery1_CustomCallback(object sender, CallbackEventArgsBase e) {
        imageIndicies.Clear();
        string fileName;        
        foreach(var item in ASPxHiddenField1) {
            string cacheFolderPath = Server.MapPath(ASPxImageGallery1.SettingsFolder.ImageCacheFolder);
            string[] cachedImages = Directory.GetFiles(cacheFolderPath, "*" + item.Key + "*", SearchOption.AllDirectories);
            for(int i = 0; i < cachedImages.Length; i++) {
                //Deleting image files in a running online sample is not allowed, uncomment this line to test deleting on your local machine
                //File.Delete(cachedImages[i]);
            }
            fileName = Server.MapPath(Path.Combine(ASPxImageGallery1.SettingsFolder.ImageSourceFolder, item.Key));
            if(File.Exists(fileName)) {
                //Deleting image files in a running online sample is not allowed, uncomment this line to test deleting on your local machine
                //File.Delete(fileName);
            }
        }
        ASPxImageGallery1.UpdateImageCacheFolder();
    }
}