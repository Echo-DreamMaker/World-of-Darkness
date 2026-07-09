using Robust.Shared.GameStates;

namespace Content.Shared.Damage.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SlowOnDamageSystem))]
public sealed partial class ClothingSlowOnDamageModifierComponent : Component
{
    [DataField]
    public float Modifier = 1;
}
