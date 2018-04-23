<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>How to implement batch deleting images in ASPxImageGallery</title>
    <style type="text/css">
        .alignLeft {
            float: left;
        }

        .alignRight {
            float: right;
        }

        .deleteButton {
            background-color: red;
            width: 10px;
            height: 10px;
        }

        .deleted {
            z-index: 20000;
            width: 100%;
            height: 100%;
            background-color: red;
            opacity: 0.5;
        }
    </style>

    <script type="text/javascript">
        function galleryEndCallback(s, e) {
            Object.keys(imgGallery.cp_items).forEach(function (index) {
                if (hf.Get(imgGallery.cp_items[index])) {
                    var divElement = document.getElementById("div" + index);
                    if (divElement) {
                        var itemContainer = findAncestor(divElement, "galleryItem");
                        addDeletedOverlay(itemContainer, index);
                    }
                }
            });
        }
        function findAncestor(el, cls) {
            do {
                el = el.parentElement;
            }
            while (!el.classList.contains(cls));
            return el;
        }
        function setDeletedItem(index, el) {
            var itemContainer = findAncestor(el, "galleryItem");
            if (!itemContainer) return;
            addOrDeleteImageInDictionary(index);
            addDeletedOverlay(itemContainer, index);
        }
        function addOrDeleteImageInDictionary(index, shouldDelete) {
            var name = imgGallery.cp_items[index];
            if (!name) return;
            if (shouldDelete)
                hf.Remove(name);
            else
                hf.Set(name, true);
            refreshButtonState();
        }
        function addDeletedOverlay(container, index) {
            var overlay = document.createElement("div");
            overlay.className = "deleted"
            overlay.onclick = function () {
                overlay.onclick = null;
                overlay.parentNode.removeChild(overlay);
                addOrDeleteImageInDictionary(index, true);
            }

            container.appendChild(overlay);
            container.children[0].style.float = "left";
        }

        function refreshButtonState() {
            var newState = Object.keys(hf.properties).length > 0;
            deleteButton.SetEnabled(newState)
        }

        function onDeleteClick(s, e) {
            imgGallery.PerformCallback();
            hf.Clear();
            refreshButtonState();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="hf" />
            <dx:ASPxImageGallery ID="ASPxImageGallery1" runat="server"
                OnCustomCallback="ASPxImageGallery1_CustomCallback"
                OnItemDataBound="ASPxImageGallery1_ItemDataBound"
                OnDataBound="ASPxImageGallery1_DataBound"
                ClientInstanceName="imgGallery">
                <ItemTextTemplate>
                    <div class="alignLeft">
                        <%#Container.Item.Text%>
                    </div>
                    <div class="alignRight deleteButton" id="div<%#Container.Item.Index%>" onclick="setDeletedItem(<%#Container.Item.Index%>,this)"></div>
                </ItemTextTemplate>
                <Styles>
                    <Item CssClass="galleryItem">
                    </Item>
                </Styles>
                <SettingsFolder ImageCacheFolder="~/Thumb/" ImageSourceFolder="~/Images" />
                <ClientSideEvents EndCallback="galleryEndCallback" />
            </dx:ASPxImageGallery>
            <dx:ASPxButton ID="ASPxButton1" runat="server" ClientEnabled="false" AutoPostBack="false" Text="Delete images" ClientInstanceName="deleteButton">
                <ClientSideEvents Click="onDeleteClick" />
            </dx:ASPxButton>
        </div>
    </form>
</body>
</html>