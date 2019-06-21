<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/Default.aspx) (VB: [Default.aspx](./VB/Default.aspx))
* [Default.aspx.cs](./CS/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/Default.aspx.vb))
<!-- default file list end -->
# How to implement batch deleting images in ASPxImageGallery
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/t499279/)**
<!-- run online end -->


<p>1. Create a dictionary of index-file names and populate this dictionary in the ASPxImageGallery.ItemDataBound event handler.</p>


```cs
Dictionary<int, string> imageIndicies = new Dictionary<int, string>();
protected void ASPxImageGallery1_ItemDataBound(object source, ImageGalleryItemEventArgs e) {
    e.Item.Text = "Text " + e.Item.Index;
    imageIndicies.Add(e.Item.Index, GetName(e.Item.ImageUrl));
}
 

```


<p><br>2. Pass the dictionary to the client side in the ASPxImageGallery.DataBound event handler, use ASPxImageGallery.JSProperties for this.</p>


```cs
protected void ASPxImageGallery1_DataBound(object sender, EventArgs e) {
    var gallery = sender as ASPxImageGallery;
    gallery.JSProperties["cp_items"] = imageIndicies;
}
 

```


<p>Your property name should start with the cp prefix. You will be able to address this property on the client side via the ASPxImageGallery ClientInstanceName.<br><br>3. Assign a custom style to every gallery item using the Styles property. This is necessary to find a gallery item node further.</p>


```aspx
<Styles>
    <Item CssClass="galleryItem">
    </Item>
</Styles>
 

```


<p><br>4. Implement ASPxImageGallery.ItemTextTemplate. In this template, create a div element that will be responsible for marking an image as ready for deletion.</p>


```aspx
<ItemTextTemplate>
    <div class="alignLeft">
        <%# Container.Item.Text %>
    </div>
    <div class="alignRight deleteButton" id="div<%#Container.Item.Index %>" onclick="setDeletedItem(<%# Container.Item.Index %>,this)"></div>
</ItemTextTemplate>
 

```


<p><br>5. Handle the div onclick event. In this event handler, find the gallery item root node (by searching for the galleryItem class up the tree), place a new transparent red div over the gallery item to show a user that the item state has been changed and add a file name to a new collection of images' names ready for deletion. In the sample, ASPxHiddenField is used to save images' names that are ready for deletion.</p>


```js
function setDeletedItem(index, el) {
    var itemContainer = findAncestor(el, "galleryItem");
    if (!itemContainer) return;
    addOrDeleteImageInDictionary(index);
    addDeletedOverlay(itemContainer, index);
}
 

```


<p><br>6. Place the Delete button on the Page and handle its client Click event. In this event, call the ASPxClientImageGallery.PerfromCallback method. On the server, handle the ASPxImageGallery.CustomCallback event and remove appropriate files both from ImageCacheFolder and ImageSourceFolder. To update ASPxImageGallery, call the UpdateImageCacheFolder method:</p>


```cs
protected void ASPxImageGallery1_CustomCallback(object sender, CallbackEventArgsBase e) {
    imageIndicies.Clear();
    string fileName;        
    foreach(var item in ASPxHiddenField1) {
        string cacheFolderPath = Server.MapPath(ASPxImageGallery1.SettingsFolder.ImageCacheFolder);
        string[] cachedImages = Directory.GetFiles(cacheFolderPath, "*" + item.Key + "*", SearchOption.AllDirectories);
        for(int i = 0; i < cachedImages.Length; i++) {
            File.Delete(cachedImages[i]);
        }
        fileName = Server.MapPath(Path.Combine(ASPxImageGallery1.SettingsFolder.ImageSourceFolder, item.Key));
        if(File.Exists(fileName)) {
            File.Delete(fileName);
        }
    }
    ASPxImageGallery1.UpdateImageCacheFolder();
}

```


<p> </p>

<br/>


