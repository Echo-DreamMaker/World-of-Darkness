using Content.Shared.Whitelist;

namespace Content.Shared.StepTrigger.Components;

[RegisterComponent]
public sealed partial class StepTriggerImmuneComponent : Component
{
    [DataField]
    public EntityWhitelist? Whitelist;
}
