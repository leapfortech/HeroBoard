using UnityEngine;

using Leap.Core.Tools;
using Leap.UI.Elements;
using Leap.UI.Page;

using Sirenix.OdinInspector;

public class ZoomDialog : SingletonBehaviour<ZoomDialog>
{
    [Space]
    [SerializeField]
    Style[] stlOverlays = null;
    [SerializeField]
    Style[] stlButtons = null;

    [Space]
    [SerializeField]
    ZoomImage zoomImage = null;

    Image imgOverlay = null;
    GameObject panel = null;

    int buttonCount = 3;
    Button[] buttons = null;

    private void Awake()
    {
        panel = transform.GetChild(0).gameObject;
        imgOverlay = panel.GetComponent<Image>();

        buttons = new Button[buttonCount];
        for (int i = 0; i < buttonCount; i++)
            buttons[i] = transform.GetChild(0).GetChild(2 + i).GetComponent<Button>();
    }

    public void Display(Sprite sprite = null)
    {
        Display(0, sprite);
    }

    public void Display(int styleIdx, Sprite sprite = null)
    {
        if (panel.activeSelf)
            return;

        zoomImage.SetSprite(sprite);

        PageManager.Instance.OnPageChanged += Hide;

        imgOverlay.SetStyle(stlOverlays[styleIdx]);
        for (int i = 0; i < buttonCount; i++)
            buttons[i].SetStyle(stlButtons[styleIdx]);
        panel.SetActive(true);
    }

    public void Hide()
    {
        if (!panel.activeSelf)
            return;

        zoomImage.SetSprite(null);

        PageManager.Instance.OnPageChanged -= Hide;
        panel.SetActive(false);
    }
}
