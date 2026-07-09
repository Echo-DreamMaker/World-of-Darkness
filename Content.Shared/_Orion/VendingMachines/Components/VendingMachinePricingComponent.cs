using Robust.Shared.GameStates;

namespace Content.Shared._Orion.VendingMachines.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class VendingMachinePricingComponent : Component
{
    [DataField]
    public bool AllProductsFree;

    [DataField]
    public bool? AllProductsFreeOverride;

    [DataField]
    public string? DepartmentAccount;

    [DataField]
    public int DefaultPrice;

    [DataField]
    public int ExtraPrice;
}
