using Naninovel;

[CommandAlias("startPairs")]
public sealed class StartPairsCommand : Command
{
    [ParameterAlias("out")] public StringParameter OutVar;
    private const string UiId = "PairsMinigameUI";

    public override async UniTask ExecuteAsync(AsyncToken token = default)
    {
        var uiMan = Engine.GetService<IUIManager>();
        var vars = Engine.GetService<ICustomVariableManager>();

        var managed = uiMan.GetUI(UiId);
        var ui = managed as PairsMinigameUI
                 ?? (managed as UnityEngine.MonoBehaviour)?.GetComponent<PairsMinigameUI>();

        if (ui == null)
        {
            UnityEngine.Debug.LogWarning("[startPairs] PairsMinigameUI not found.");
            vars.SetVariableValue("pairsResult", "lose");
            vars.SetVariableValue("pairsRoute", ".onLose");
            vars.SetVariableValue("pairsWin", "false");
            vars.SetVariableValue("pairsLose", "true");
            vars.SetVariableValue("pairsCancel", "false");
            if (Assigned(OutVar)) vars.SetVariableValue(OutVar, "lose");
            return;
        }

        if (!ui.Visible) ui.Show();
        ui.Begin();

        var result = await ui.WaitForResultAsync() ?? "lose";

        ui.Hide();


        vars.SetVariableValue("pairsResult", result);
        vars.SetVariableValue("pairsWin", result == "win" ? "true" : "false");
        vars.SetVariableValue("pairsLose", result == "lose" ? "true" : "false");
        vars.SetVariableValue("pairsCancel", result == "cancel" ? "true" : "false");


        var route = result == "win" ? ".onWin"
                  : result == "lose" ? ".onLose"
                  : ".onCancel";
        vars.SetVariableValue("pairsRoute", route);

        if (Assigned(OutVar))
            vars.SetVariableValue(OutVar, result);
    }
}
