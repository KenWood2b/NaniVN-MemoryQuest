using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [Header("Refs (assign in prefab)")]
    [SerializeField] private Button button;
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;

    public void SetImages(Sprite front, Sprite back)
    {
        if (frontImage) frontImage.sprite = front;
        if (backImage) backImage.sprite = back;
    }

    public void ShowBackInstant()
    {
        if (frontImage) frontImage.enabled = false;
        if (backImage) backImage.enabled = true;
    }

    public void ShowFrontInstant()
    {
        if (frontImage) frontImage.enabled = true;
        if (backImage) backImage.enabled = false;
    }

    public void SetInteractable(bool v)
    {
        if (button) button.interactable = v;
    }
}
