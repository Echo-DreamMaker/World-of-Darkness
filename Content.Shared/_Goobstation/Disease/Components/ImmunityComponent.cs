using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Disease.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class ImmunityComponent : Component
{
    [DataField]
    public float ImmunityGainRate = 0.002f;

    [DataField]
    public float ImmunityStrength = 0.02f;

    [DataField]
    public bool InDead = false;
}
