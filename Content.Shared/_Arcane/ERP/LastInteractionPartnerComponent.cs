using Robust.Shared.GameStates;

namespace Content.Shared._Arcane.ERP;

/// <summary>
/// Хранит последнюю цель и ID последней ERP-интеракции для персонажа.
/// Используется при оргазме для показа окна "кончить внутрь?"
/// только если последняя интеракция была с вагиной (PenisFuck, PenisGrind).
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class LastInteractionPartnerComponent : Component
{
    /// <summary>
    /// Последняя цель ERP-взаимодействия.
    /// </summary>
    [AutoNetworkedField]
    public NetEntity? LastTarget;

    /// <summary>
    /// ID последней интеракции (например "PenisFuck", "PenisGrind").
    /// </summary>
    [AutoNetworkedField]
    public string? LastInteractionId;
}
