using UnityEngine;

using Leap.UI.Elements;

public class ScrolledText : MonoBehaviour
{
    private RectTransform trf;
    private Text text;

    private void Awake()
    {
        trf = GetComponent<RectTransform>();
        text = trf.GetChild(0).GetComponent<Text>();
    }

    public void Scale()
    {
        trf.sizeDelta = new Vector2(trf.sizeDelta.x, text.LinesHeight);
    }
}
