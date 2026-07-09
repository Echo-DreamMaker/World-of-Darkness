using Robust.Shared.Prototypes;

namespace Content.Shared.Humanoid.Prototypes;

[Prototype("speciesBaseSprites")]
public sealed partial class SpeciesSpritePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField("sprites", required: true)]
    public Dictionary<string, ProtoId<HumanoidSpeciesSpriteLayer>> Sprites { get; private set; } = default!;
}
