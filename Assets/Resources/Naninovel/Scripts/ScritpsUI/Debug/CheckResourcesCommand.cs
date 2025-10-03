using Naninovel;
using Naninovel.UI;
using UnityEngine;

[CommandAlias("checkResources")]
public sealed class CheckResourcesCommand : Command
{
    public override UniTask ExecuteAsync(AsyncToken token = default)
    {
        Debug.Log("=== DIAGNOSTIC CHECK ===");

        // 1. Проверяем префаб в ресурсах
        var prefab = Resources.Load<GameObject>("PairsMinigameUI");
        Debug.Log($"1. Prefab in Resources: {prefab != null}");

        if (prefab != null)
        {
            Debug.Log($"   Prefab name: {prefab.name}");
            var uiComponent = prefab.GetComponent<PairsMinigameUI>();
            Debug.Log($"   Has PairsMinigameUI component: {uiComponent != null}");

            if (uiComponent != null)
            {
                Debug.Log($"   GameObject: {uiComponent.gameObject.name}");
                Debug.Log($"   GameObject active: {uiComponent.gameObject.activeInHierarchy}");
            }
        }

        // 2. Проверяем UI Manager
        var uiMan = Engine.GetService<IUIManager>();
        var ui = uiMan.GetUI<PairsMinigameUI>();
        Debug.Log($"2. UI in Manager: {ui != null}");

        if (ui != null)
        {
            Debug.Log($"   UI GameObject: {ui.gameObject.name}");
            Debug.Log($"   UI Visible: {ui.Visible}");
            Debug.Log($"   UI GameObject active: {ui.gameObject.activeInHierarchy}");
        }

        // 3. Проверяем все зарегистрированные UI
        Debug.Log("3. All CustomUI components in scene:");
        var allUIs = GameObject.FindObjectsOfType<CustomUI>();
        foreach (var customUI in allUIs)
        {
            Debug.Log($"   - {customUI.gameObject.name} (Type: {customUI.GetType().Name})");
        }

        return UniTask.CompletedTask;
    }
}