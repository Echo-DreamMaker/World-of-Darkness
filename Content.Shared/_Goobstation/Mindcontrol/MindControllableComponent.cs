using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Mindcontrol;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MindControllableComponent : Component
{
    [ViewVariables, AutoNetworkedField]
    public bool ControlledBySomeone = false;
}
