using Naninovel;
using UnityEngine;

[CommandAlias("debugPairs")]
public sealed class DebugPairsCommand : Command
{
    public override async UniTask ExecuteAsync(AsyncToken token = default)
    {
        Debug.Log("=== DEBUG PAIRS COMMAND ===");

        var uiMan = Engine.GetService<IUIManager>();
        var pairsUI = uiMan.GetUI<PairsMinigameUI>();

        Debug.Log($"PairsMinigameUI found: {pairsUI != null}");

        if (pairsUI == null)
        {
            Debug.LogError("PairsMinigameUI NOT found in UI Manager!");
            return;
        }

        Debug.Log($"UI GameObject: {pairsUI.gameObject.name}");
        Debug.Log("Showing UI for 3 seconds...");

        pairsUI.Show();
        await UniTask.Delay(3000);
        pairsUI.Hide();

        Debug.Log("Test completed!");
    }
}