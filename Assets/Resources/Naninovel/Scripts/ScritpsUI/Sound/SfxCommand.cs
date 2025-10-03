using Naninovel;

[CommandAlias("sfx")]
public sealed class SfxCommand : Command
{
    [ParameterAlias("id")] public StringParameter Id;
    [ParameterAlias("vol")] public DecimalParameter Volume;

    public override UniTask ExecuteAsync(AsyncToken _ = default)
    {
        if (Assigned(Id)) AudioHub.I?.PlaySfx(Id, Assigned(Volume) ? (float?)Volume.Value : null);
        return UniTask.CompletedTask;
    }
}
