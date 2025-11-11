using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

using Sirenix.OdinInspector;

public class ZoomImage : MonoBehaviour, IScrollHandler
{
    [Space]
    [SerializeField]
    float zoomSpeed = 0.1f;

    [SerializeField]
    float zoomMax = 10f;

    private RectTransform trf;
    private Image image = null;

    private Vector3 initialScale;
    private float initialHeight;
    private Vector3 maxScale;

    Quaternion qRight = Quaternion.AngleAxis(-90f, Vector3.forward);
    Quaternion qLeft = Quaternion.AngleAxis(90f, Vector3.forward);

    private void Awake()
    {
        trf = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        initialScale = trf.localScale;
        initialHeight = trf.sizeDelta.y;
        maxScale = initialScale * zoomMax;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;

        trf.anchoredPosition = Vector2.zero;
        trf.localScale = initialScale;
        trf.rotation = Quaternion.identity;

        if (sprite == null)
            return;

        trf.sizeDelta = new Vector2(sprite.rect.width * initialHeight / sprite.rect.height, initialHeight);
    }

    public void OnScroll(PointerEventData eventData)
    {
        Vector3 delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        Vector3 desiredScale = transform.localScale + delta;

        trf.localScale = ClampDesiredScale(desiredScale);
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(initialScale, desiredScale);
        return Vector3.Min(maxScale, desiredScale);
    }

    public void RotateRight()
    {
        trf.rotation = qRight * trf.rotation;
    }

    public void RotateLeft()
    {
        trf.rotation = qLeft * trf.rotation;
    }
}
