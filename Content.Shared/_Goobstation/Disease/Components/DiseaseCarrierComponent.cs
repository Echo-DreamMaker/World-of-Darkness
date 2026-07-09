using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Disease.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DiseaseCarrierComponent : Component
{
    [ViewVariables, AutoNetworkedField]
    public List<EntityUid> Diseases = new();

    [DataField("diseases")]
    public List<string> StartingDiseases = new();

    [DataField]
    public bool EffectImmune = false;
}
