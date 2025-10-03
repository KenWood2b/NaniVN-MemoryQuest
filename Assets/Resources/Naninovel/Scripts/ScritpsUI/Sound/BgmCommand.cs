using Naninovel;

[CommandAlias("bgm")]
public sealed class BgmCommand : Command
{
    [ParameterAlias("id")] public StringParameter Id;
    [ParameterAlias("vol")] public DecimalParameter Volume;
    [ParameterAlias("fade")] public DecimalParameter Fade;
    [ParameterAlias("stop")] public BooleanParameter Stop;

    public override UniTask ExecuteAsync(AsyncToken _ = default)
    {
        var fade = Assigned(Fade) ? (float)Fade.Value : 0.25f;
        if (Assigned(Stop) && Stop.Value) AudioHub.I?.StopMusic(fade);
        else if (Assigned(Id)) AudioHub.I?.PlayMusic(Id, Assigned(Volume) ? (float?)Volume.Value : null, fade);
        return UniTask.CompletedTask;
    }
}
