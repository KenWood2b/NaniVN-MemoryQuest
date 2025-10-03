using Naninovel;
using Naninovel.UI;
using UnityEngine;

[CommandAlias("dumpUI")]
public sealed class DumpUICommand : Command
{
    public override UniTask ExecuteAsync(AsyncToken token = default)
    {
        var ui = Engine.GetService<IUIManager>();
        if (ui == null) { Debug.LogError("IUIManager == null"); return UniTask.CompletedTask; }

        string[] namesToCheck = { "MapUI", "QuestUI", "PairsMinigameUI", "AmuletItem", "TitleUI", "SettingsUI" };

        foreach (var name in namesToCheck)
        {
            var m = ui.GetUI(name);
            if (m == null) { Debug.Log($"[dumpUI] '{name}' -> NULL"); continue; }

            var go = (m as Component)?.gameObject;
            var hasMapUI = go && go.GetComponent<MapUI>() != null;
            Debug.Log($"[dumpUI] '{name}': type={m.GetType().Name}, go={go?.name}, has MapUI comp: {hasMapUI}");
        }

        return UniTask.CompletedTask;
    }
}
