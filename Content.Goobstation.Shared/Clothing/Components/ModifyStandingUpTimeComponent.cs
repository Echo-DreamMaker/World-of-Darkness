using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Clothing.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class ModifyStandingUpTimeComponent : Component
{
    [DataField]
    public float Multiplier = 1f;
}
