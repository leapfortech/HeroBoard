using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Leap.UI.Elements;
using Leap.UI.Dialog;
using Leap.UI.Dialog.Gallery;
using Leap.Graphics.Tools;

using Sirenix.OdinInspector;

public class ProjectImageChangeAction : MonoBehaviour
{
    [Serializable]
    public class StringsListEvent : UnityEvent<List<String>> { }

    [Title("Parameters")]
    [SerializeField]
    int imageCountMax = 10;
    [SerializeField]
    Vector2Int gallerySize = new Vector2Int(512, 512);

    [Title("Lists")]
    [SerializeField]
    ListScroller lstImage = null;

    [Title("Action")]
    [SerializeField]
    Button btnAdd = null;

    [Title("Event")]
    [SerializeField]
    private StringsListEvent onGetImages = null;

    readonly List<Sprite> sprites = new List<Sprite>();

    private void Start()
    {
        btnAdd.AddAction(SearchPhoto);
    }

    public void Clear()
    {
        lstImage.Clear();
        for (int i = 0; i < sprites.Count; i++)
            sprites[i].Destroy();
        sprites.Clear();
    }

    public void GetImages()
    {
        List<String> strImages = new List<String>(sprites.Count);
        for (int i = 0; i < sprites.Count; i++)
            strImages.Add(sprites[i].ToStrBase64(ImageType.JPG));

        onGetImages.Invoke(strImages);
    }

    public void SearchPhoto()
    {
        if (sprites.Count == imageCountMax)
        {
            ChoiceDialog.Instance.Info("Agregar una imágen", $"Solo se pueden cargar {imageCountMax} imágenes.");
            return;
        }

        GalleryDialog.Instance.Search(gallerySize, false, Add);
    }

    public void ApplyInfo(ProjectInfo projectInfo)
    {
        sprites.Clear();
        for (int i = 0; i < StateManager.Instance.ProjectFull.Sprites.Count; i++)
            sprites.Add(StateManager.Instance.ProjectFull.Sprites[i].ToBytes(ImageType.JPG).CreateSprite($"ImageUpdated_{i:D02}"));

        Refresh();
    }

    public void Add(Texture2D image)
    {
        image.name = $"ImageAdded_{sprites.Count:D02}";
        sprites.Add(image.CreateSprite(true));
        
        Refresh();
    }

    public void Remove(int idx)
    {
        sprites[idx].Destroy();
        sprites.RemoveAt(idx);

        Refresh();
    }

    public void Refresh()
    {
        lstImage.Clear();

        for (int i = 0; i < sprites.Count; i++)
        {
            ListScrollerValue scrollerValue = new ListScrollerValue(1, true);
            scrollerValue.SetSprite(0, sprites[i]);
            lstImage.AddValue(scrollerValue);
        }

        lstImage.ApplyValues();
    }
}
