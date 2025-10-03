using Naninovel;
using UnityEngine;

[CommandAlias("amuletShow")]
public sealed class ShowAmuletCommand : Command
{
    [ParameterAlias("name")] public StringParameter ItemName = "Амулет";

    [ParameterAlias("icon")] public StringParameter IconPath;

    public override async UniTask ExecuteAsync(AsyncToken token = default)
    {
        var uiMgr = Engine.GetService<IUIManager>();
        var ui = uiMgr?.GetUI<AmuletPopupUI>();

        if (ui == null) { Debug.LogWarning("AmuletPopupUI не найден. Проверь Manage UI Resources → Custom UIs."); return; }

        Sprite icon = null;
        if (Assigned(IconPath) && !string.IsNullOrEmpty(IconPath))
            icon = Resources.Load<Sprite>(IconPath);

        await ui.ShowAmuletAsync(Assigned(ItemName) ? ItemName.Value : "Амулет", icon, token);
    }
}
