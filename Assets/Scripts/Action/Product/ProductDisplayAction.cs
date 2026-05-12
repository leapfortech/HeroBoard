using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Leap.UI.Elements;
using Leap.UI.Page;
using Leap.UI.Dialog;
using Leap.Core.Tools;

using Sirenix.OdinInspector;


public class ProductDisplayAction : MonoBehaviour
{
    [Serializable]
    public class ImagesEvent : UnityEvent<List<Sprite>> { }
    [Space]
    [Title("Details")]
    [SerializeField]
    Text txtAlias = null;
    [SerializeField]
    Text txtDateTime = null;
    [SerializeField]
    Text txtTitle = null;
    [SerializeField]
    Text txtSummary = null;
    [SerializeField]
    Text txtDescription = null;

    [Space]
    [Title("Panel")]
    [SerializeField]
    PanelController pnlCtr = null;
    [SerializeField]
    Panel pnlDetail = null;

    [Title("Event")]
    [SerializeField]
    ImagesEvent onImagesDisplay = null;
    [SerializeField]
    UnityLongsEvent onDisplayed = null;

    ProductService productService;

    long postId = -1, productId = -1;

    private void Awake()
    {
        productService = GetComponent<ProductService>();
    }

    public void Display(long postId)
    {
        this.postId = postId;

        ProductFull productFull = StateManager.Instance.GetProductFullByPostId(postId);
        if (productFull != null)
        {
            productId = productFull.Id;
            Display(productFull);
            return;
        }

        ScreenDialog.Instance.Display();
        productService.GetFullByPostId(postId);
    }

    public void ApplyFull(ProductFull productFull)
    {
        productId = productFull.Id;
        StateManager.Instance.AddProductFull(productFull);
        StateManager.Instance.AddProductImages(productFull.Id, productFull.Images);
        Display(productFull);
    }

    private void Display(ProductFull productFull)
    {       
        if (productFull == null)
            return;

        // Post
        txtAlias.TextValue = $"Publicado por: <b>@{productFull.AppUserAlias}</b>";
        txtTitle.TextValue = String.IsNullOrWhiteSpace(productFull.Title) ? "-" : productFull.Title;
        txtDateTime.TextValue = productFull.PublicationDateTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        txtSummary.TextValue = String.IsNullOrWhiteSpace(productFull.Summary) ? "-" : productFull.Summary;
        txtDescription.TextValue = String.IsNullOrWhiteSpace(productFull.Description) ? "-" : productFull.Description;

        List<Sprite> images = StateManager.Instance.GetProductImagesById(productId);
        
        onImagesDisplay.Invoke(images);
        onDisplayed.Invoke(new long[2] {productFull.PostId, productFull.Id});

        pnlCtr.ChangePanel(pnlDetail);

        StateManager.Instance.BoardLoadHide();
    }
}