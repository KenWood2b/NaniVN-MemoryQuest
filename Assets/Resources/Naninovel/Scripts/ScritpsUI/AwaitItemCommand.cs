using Naninovel;

[CommandAlias("awaitItem")]
public class AwaitItemCommand : Command
{
    [ParameterAlias("id"), RequiredParameter] public StringParameter ItemId;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var ct = asyncToken.CancellationToken;
        string target = ItemId;
        bool done = false;

        void Handler(string id) { if (id == target) done = true; }

        InteractiveItem.Clicked += Handler;
        while (!done && !ct.IsCancellationRequested)
            await UniTask.DelayFrame(1, cancellationToken: ct);
        InteractiveItem.Clicked -= Handler;

        if (target == "amulet")
        {
            var vars = Engine.GetService<ICustomVariableManager>();
            vars.SetVariableValue("hasAmulet", "true");
            vars.SetVariableValue("loc3Locked", "true");
        }
    }
}
