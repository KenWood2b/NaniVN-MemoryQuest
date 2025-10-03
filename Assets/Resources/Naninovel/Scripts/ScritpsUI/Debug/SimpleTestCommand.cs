using Naninovel;
using UnityEngine;

[CommandAlias("simpleTest")]
public sealed class SimpleTestCommand : Command
{
    public override async UniTask ExecuteAsync(AsyncToken token = default)
    {
        Debug.Log("=== SIMPLE TEST COMMAND ===");

        var resourcePath = "PairsMinigameUI";
        var uiPrefab = Resources.Load<GameObject>(resourcePath);

        if (uiPrefab == null)
        {
            Debug.LogError($"Prefab not found at path: {resourcePath}");
            return;
        }

        Debug.Log("Prefab found! Instantiating...");
        var uiObject = Object.Instantiate(uiPrefab);
        var ui = uiObject.GetComponent<PairsMinigameUI>();

        if (ui != null)
        {
            Debug.Log("UI component found! Showing for 3 seconds...");
            ui.Show();
            await UniTask.Delay(3000);
            ui.Hide();
            Object.Destroy(uiObject);
            Debug.Log("Test completed successfully!");
        }
        else
        {
            Debug.LogError("PairsMinigameUI component not found on prefab!");
            Object.Destroy(uiObject);
        }
    }
}