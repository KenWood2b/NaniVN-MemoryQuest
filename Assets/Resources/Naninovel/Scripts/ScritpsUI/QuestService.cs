using Naninovel;

public class QuestService : IEngineService
{
    private QuestUI ui;

    public UniTask InitializeServiceAsync()
    {
        ui = Engine.GetService<IUIManager>().GetUI<QuestUI>();
        return UniTask.CompletedTask;
    }

    public void Push(string text)
    {
        if (ui == null) return;
        ui.AddLine(text);
        if (!ui.Visible)
            _ = ui.ShowAsync();
    }

    public void ResetService() { }
    public void DestroyService() { }
}
