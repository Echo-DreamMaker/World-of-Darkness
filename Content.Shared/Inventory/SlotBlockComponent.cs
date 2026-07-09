using Robust.Shared.GameStates;

namespace Content.Shared.Inventory;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SlotBlockComponent : Component
{
    [DataField, AutoNetworkedField]
    public HashSet<SlotFlags> BlockList = new();

    [DataField, AutoNetworkedField]
    public HashSet<SlotFlags> HideList = new();

    [DataField, AutoNetworkedField]
    public SlotFlags Slots = SlotFlags.NONE;
}
