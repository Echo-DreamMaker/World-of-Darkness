namespace Content.Server._Goobstation.HeightAdjust;

[RegisterComponent]
public sealed partial class FlightAffectedByScaleComponent : Component
{
    [DataField]
    public float MinSpeedFactor = 0.75f, MaxSpeedFactor = 1.05f;

    [DataField]
    public float MinStaminaDrainFactor = 0.95f, MaxStaminaDrainFactor = 1.25f;
}
