namespace Content.Shared._Goobstation.Weapons.Penetration;

[RegisterComponent]
public sealed partial class PenetratableComponent : Component
{
    [DataField]
    public float PenetrateDamage = 25f;

    [DataField("damagePenalty")]
    public float DamagePenaltyModifier = 0f;
}
