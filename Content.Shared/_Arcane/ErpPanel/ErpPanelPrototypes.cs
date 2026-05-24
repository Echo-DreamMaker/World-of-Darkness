using Content.Shared._Arcane.ErpPanel.Requirements;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared._Arcane.ErpPanel;

[Prototype]
public sealed partial class PanelInteractionPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public ProtoId<PanelInteractionCategoryPrototype> Category;

    [DataField(required: true)]
    public string Name = string.Empty;

    [DataField(required: true)]
    public List<string> Messages = new();

    [DataField(required: true)]
    public SpriteSpecifier Icon = SpriteSpecifier.Invalid;

    [DataField]
    public TimeSpan Cooldown = TimeSpan.FromSeconds(3);

    [DataField]
    public float Range = 2f;

    [DataField]
    public int UserArouse = 0;

    [DataField]
    public int TargetArouse = 0;


    [DataField]
    public List<ErpRequirement>? UserRequirements;

    [DataField]
    public List<ErpRequirement>? TargetRequirements;
}

[Prototype]
public sealed partial class PanelInteractionCategoryPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public string Name = string.Empty;
}
