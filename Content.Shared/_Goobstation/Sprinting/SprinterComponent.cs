using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Sprinting;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SprinterComponent : Component
{
    [ViewVariables, AutoNetworkedField]
    public bool IsSprinting = false;

    [DataField, AutoNetworkedField]
    public bool ScaleWithStamina = true;

    [DataField, AutoNetworkedField]
    public bool CanSprint = true;

    [DataField, AutoNetworkedField]
    public float StaminaDrainRate = 10f;

    [DataField, AutoNetworkedField]
    public float StaminaRegenMultiplier = 0.75f;

    [DataField, AutoNetworkedField]
    public float StaminaDrainMultiplier = 1.3f;

    [DataField, AutoNetworkedField]
    public float SprintSpeedMultiplier = 1.6f;

    [DataField, AutoNetworkedField]
    public TimeSpan TimeBetweenSprints = TimeSpan.FromSeconds(3);

    [ViewVariables, AutoNetworkedField]
    public TimeSpan LastSprint = TimeSpan.Zero;

    [DataField]
    public string StaminaDrainKey = "sprint";

    [ViewVariables]
    public TimeSpan LastStep = TimeSpan.Zero;

    [DataField]
    public SoundSpecifier SprintStartupSound = new SoundPathSpecifier("/Audio/_Goobstation/Effects/Sprinting/sprint_puff.ogg");

    [DataField, AutoNetworkedField]
    public TimeSpan TimeBetweenSteps = TimeSpan.FromSeconds(0.6);
}
