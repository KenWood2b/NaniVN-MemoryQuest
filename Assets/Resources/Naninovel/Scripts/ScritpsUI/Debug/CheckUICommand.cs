using Naninovel;
using UnityEngine;

[CommandAlias("checkUI")]
public sealed class CheckUICommand : Command
{
    public override UniTask ExecuteAsync(AsyncToken token = default)
    {
        Debug.Log("=== UI CHECK ===");

        var uiMan = Engine.GetService<IUIManager>();
        var pairsUI = uiMan.GetUI<PairsMinigameUI>();

        Debug.Log($"UI Manager: {uiMan != null}");
        Debug.Log($"PairsMinigameUI: {pairsUI != null}");

        if (pairsUI == null)
        {
            Debug.LogError("Critical: PairsMinigameUI not registered in UI Manager!");
            Debug.Log("Make sure it's added to UI Configuration and prefab is in Resources.");
        }
        else
        {
            Debug.Log($"Success: UI found!");
            Debug.Log($"GameObject: {pairsUI.gameObject.name}");
            Debug.Log($"Visible: {pairsUI.Visible}");
        }

        return UniTask.CompletedTask;
    }
}