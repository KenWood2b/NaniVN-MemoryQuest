using System.Threading.Tasks;
using Naninovel;
using Naninovel.UI;
using UnityEngine;

[CommandAlias("openMap")]
public sealed class OpenMapCommand : Command
{
    [ParameterAlias("out")] public StringParameter OutVar;

    public override async UniTask ExecuteAsync(AsyncToken token = default)
    {
        var ui = Engine.GetService<IUIManager>();
        if (ui == null) { Debug.LogError("[openMap] IUIManager is null."); return; }

        var managed = ui.GetUI("MapUI");
        if (managed == null) { _ = ui.GetUI("MapUI"); await Task.Yield(); managed = ui.GetUI("MapUI"); }

        var map = (managed as Component)?.GetComponent<MapUI>() ?? ui.GetUI<MapUI>();
        if (map == null) { Debug.LogError("[openMap] MapUI not found (check Naninovel → UI)."); return; }

        await map.ChangeVisibilityAsync(true, 0, token);
        map.ApplyLocksFromVars();

        string choice = null;
        try { choice = await map.WaitForChoiceAsync(token.CancellationToken); }
        finally { await map.ChangeVisibilityAsync(false, 0, token); }

        if (string.IsNullOrEmpty(choice))
        {
            var vars = Engine.GetService<ICustomVariableManager>();
            bool l1 = AsBool(vars.GetVariableValue("loc1Locked"));
            bool l2 = AsBool(vars.GetVariableValue("loc2Locked"));
            bool l3 = AsBool(vars.GetVariableValue("loc3Locked"));
            if (!l2) choice = "Loc2";
            else if (!l1) choice = "Loc1";
            else if (!l3) choice = "Loc3";
            else choice = "Loc1";
        }

        choice = (choice ?? "Loc1").Trim().Trim('"');
        if (choice != "Loc1" && choice != "Loc2" && choice != "Loc3") choice = "Loc1";

        var varsMgr = Engine.GetService<ICustomVariableManager>();
        var hasAmulet = AsBool(varsMgr.GetVariableValue("hasAmulet"));

        string targetScript = choice;

        if (hasAmulet)
        {
            if (choice == "Loc2") targetScript = "AfterAmulet";
            else if (choice == "Loc1") targetScript = "Finale";
            else targetScript = "Loc3";
        }

        if (Assigned(OutVar))
            varsMgr.SetVariableValue(OutVar, targetScript);
    }

    private static bool AsBool(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        s = s.Trim().Trim('"');
        return s.Equals("true", System.StringComparison.OrdinalIgnoreCase);
    }
}
