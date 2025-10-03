using System.Threading;
using Naninovel;
using Naninovel.UI;

public class MapService : IEngineService
{
    IUIManager UI => Engine.GetService<IUIManager>();

    public UniTask InitializeServiceAsync() => UniTask.CompletedTask;

    public async UniTask<string> OpenAndWaitAsync(CancellationToken ct = default)
    {
        var map = UI.GetUI<MapUI>();
        if (map == null)
        {
            UnityEngine.Debug.LogError("MapUI is not registered (Project Settings → Naninovel → UI → Custom UIs).");
            return "Loc1";
        }

        await map.ChangeVisibilityAsync(true);
        map.ApplyLocksFromVars();

        string choice = null;
        try { choice = await map.WaitForChoiceAsync(ct); }
        finally { await map.ChangeVisibilityAsync(false); }

        return string.IsNullOrEmpty(choice) ? "Loc1" : choice;
    }

    public void ResetService() { }
    public void DestroyService() { }
}
