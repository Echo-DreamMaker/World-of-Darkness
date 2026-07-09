using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Dash;

[RegisterComponent]
public sealed partial class DashActionComponent : Component
{
    [DataField("actionProto")]
    public EntProtoId ActionProto = string.Empty;
}
