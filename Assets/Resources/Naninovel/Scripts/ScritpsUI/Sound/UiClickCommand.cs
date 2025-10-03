using Naninovel;

[CommandAlias("uiclick")]
public sealed class UiClickCommand : Command
{
    [ParameterAlias("id")] public StringParameter Id;
    [ParameterAlias("vol")] public DecimalParameter Volume;

    public override UniTask ExecuteAsync(AsyncToken _ = default)
    {
        AudioHub.I?.PlayUi(Assigned(Id) ? Id : "ui-click", Assigned(Volume) ? (float?)Volume.Value : null);
        return UniTask.CompletedTask;
    }
}
