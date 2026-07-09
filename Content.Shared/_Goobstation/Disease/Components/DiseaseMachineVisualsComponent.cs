namespace Content.Shared._Goobstation.Disease.Components;

[RegisterComponent]
public sealed partial class DiseaseMachineVisualsComponent : Component
{
    [DataField]
    public string IdleState = "icon";

    [DataField]
    public string RunningState = "running";
}
