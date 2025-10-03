using System;
using UnityEngine;
using UnityEngine.UI;

public class PairsCard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button button;     
    [SerializeField] private Image frontImage;   
    [SerializeField] private Image backImage;  

    public event Action<PairsCard> Clicked;

    public Sprite FaceSprite { get; private set; }
    public bool IsFaceUp { get; private set; }
    public bool IsMatched { get; private set; }

    private void Awake()
    {
        if (!button) button = GetComponent<Button>();
        if (frontImage) frontImage.raycastTarget = false;
        if (backImage) backImage.raycastTarget = false;

        button.onClick.AddListener(() => Clicked?.Invoke(this));
    }

    public void Init(Sprite face, Sprite back)
    {
        FaceSprite = face;
        if (frontImage) frontImage.sprite = face;
        if (backImage) backImage.sprite = back;

        SetFaceDown();
        IsMatched = false;
        if (button) button.interactable = true;
    }

    public void SetFaceDown() { IsFaceUp = false; if (frontImage) frontImage.enabled = false; if (backImage) backImage.enabled = true; }
    public void FlipUp() { IsFaceUp = true; if (frontImage) frontImage.enabled = true; if (backImage) backImage.enabled = false; }
    public void FlipDown() { if (!IsMatched) SetFaceDown(); }
    public void LockOpen() { IsMatched = true; if (frontImage) frontImage.enabled = true; if (backImage) backImage.enabled = false; if (button) button.interactable = false; }
}
