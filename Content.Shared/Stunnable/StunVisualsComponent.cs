using Robust.Shared.Utility;

namespace Content.Shared.Stunnable;

[RegisterComponent]
public sealed partial class StunVisualsComponent : Component
{
    [DataField]
    public ResPath StarsPath = new("Mobs/Effects/stunned.rsi");

    [DataField]
    public string State = "stunned";
}
