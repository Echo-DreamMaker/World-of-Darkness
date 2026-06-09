using Robust.Shared.GameStates;

namespace Content.Shared._Arcane.ERP.Organs;

// Marker components — each is a tag for interaction condition checks.
// All sexes
[RegisterComponent, NetworkedComponent]
public sealed partial class AnusOrganComponent : Component;

// Male
[RegisterComponent, NetworkedComponent]
public sealed partial class PenisOrganComponent : Component;

[RegisterComponent, NetworkedComponent]
public sealed partial class TesticlesOrganComponent : Component;

// Female
[RegisterComponent, NetworkedComponent]
public sealed partial class VaginaOrganComponent : Component;

[RegisterComponent, NetworkedComponent]
public sealed partial class UterusOrganComponent : Component;

// Female
[RegisterComponent, NetworkedComponent]
public sealed partial class BreastsOrganComponent : Component;
