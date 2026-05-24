using Robust.Shared.GameStates;

namespace Content.Shared._Arcane.ErpPanel;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ErpPanelOwnerComponent : Component
{
    public EntityUid? Target = null;

    [AutoNetworkedField]
    public Dictionary<string, TimeSpan> Cooldowns = new();
}
