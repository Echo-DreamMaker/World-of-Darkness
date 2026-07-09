using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Wraith.Components;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class WraithAbsorbableComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Absorbed;
}
