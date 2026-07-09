using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Wraith.Components;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class WraithAbsorbableComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Absorbed;
}
