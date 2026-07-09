using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.SecondSkin;

[RegisterComponent, NetworkedComponent]
public sealed partial class DisgustComponent : Component
{
    [DataField]
    public float Level;

    [DataField]
    public float ReductionRate = 2f;

    [DataField]
    public float AccumulationMultiplier = 1f;

    [DataField]
    public float UpdateTime = 1f;

    [ViewVariables]
    public float Accumulator;
}
