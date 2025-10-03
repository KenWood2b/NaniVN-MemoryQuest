using Naninovel;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmuletPopupUI : CustomUI
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameText;

    public async UniTask ShowAmuletAsync(string itemName, Sprite icon, AsyncToken token = default)
    {
        if (nameText) nameText.text = itemName ?? "Amulet";
        if (image) image.sprite = icon;

        await ChangeVisibilityAsync(true, null, token);
        await UniTask.Delay(1500, cancellationToken: token.CancellationToken);
        await ChangeVisibilityAsync(false, null, token); 


    }
}
