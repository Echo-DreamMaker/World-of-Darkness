namespace Content.Shared._Goobstation.Harpy;

[RegisterComponent]
public sealed partial class HarpySingerComponent : Component
{
    [DataField]
    public int ShutUpDamageThreshold = 5;
}
