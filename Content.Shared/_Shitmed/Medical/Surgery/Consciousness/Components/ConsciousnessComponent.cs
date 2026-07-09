using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Consciousness.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class ConsciousnessComponent : Component
{
    [DataField]
    public float Threshold = 95;

    [DataField]
    public float Cap = 190;
}
