using Naninovel;

[CommandAlias("questUpdate")]
public class QuestUpdateCommand : Command
{
    [ParameterAlias("text"), RequiredParameter] public StringParameter Text;

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var uiMgr = Engine.GetService<IUIManager>();
        var ui = uiMgr != null ? uiMgr.GetUI<QuestUI>() : null;

        if (ui != null)
        {
            ui.AddLine(Text);
            if (!ui.Visible)
                _ = ui.ShowAsync();
        }
        return UniTask.CompletedTask;
    }
}
