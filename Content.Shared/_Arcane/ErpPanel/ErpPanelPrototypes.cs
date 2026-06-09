// World-of-Darkness (WoD) EDIT START
// Добавлен импорт EmotePrototype для привязки ERP-интеракций к эмоутам
using Content.Shared._Arcane.ErpPanel.Requirements;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
// World-of-Darkness (WoD) EDIT END

namespace Content.Shared._Arcane.ErpPanel;

[Prototype("panelInteraction")]
public sealed partial class PanelInteractionPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public ProtoId<PanelInteractionCategoryPrototype> Category;

    [DataField(required: true)]
    public string Name = string.Empty;

    [DataField]
    public List<string> Messages = new();

    [DataField]
    public List<string> SelfMessages = new();

    [DataField(required: true)]
    public SpriteSpecifier Icon = SpriteSpecifier.Invalid;

    [DataField]
    public List<ResPath> Sounds = new();

    [DataField]
    public TimeSpan Cooldown = TimeSpan.FromSeconds(3);

    [DataField]
    public float Range = 1.5f;

    [DataField]
    public int UserArouse = 0;

    [DataField]
    public int TargetArouse = 0;

    [DataField]
    public List<ErpRequirement>? UserRequirements;

    [DataField]
    public List<ErpRequirement>? TargetRequirements;

    // World-of-Darkness (WoD) EDIT START
    // Добавлены поля для привязки ERP-интеракций к эмоутам
    /// <summary>
    ///     Эмоут, который проигрывается у user (того, кто нажал кнопку).
    ///     Если указан, эмоут будет вызван через ChatSystem и отправит в чат сообщение из EmotePrototype.ChatMessages,
    ///     а также проиграет анимацию, указанную в EmotePrototype.Animation (если есть).
    ///     Если не указан, используется стандартное текстовое сообщение из <see cref="Messages"/>.
    /// </summary>
    [DataField]
    public ProtoId<EmotePrototype>? UserEmote;

    /// <summary>
    ///     Эмоут, который проигрывается у target.
    ///     Например: при интеракции "Поцеловать" user делает эмоут "Поцелуй", а target - "Краснеет".
    /// </summary>
    [DataField]
    public ProtoId<EmotePrototype>? TargetEmote;

    /// <summary>
    ///     Если true, то эмоут target принудительно отправится в чат (даже если он скрыт по умолчанию).
    ///     Если <see cref="TargetEmote"/> не указан, поле игнорируется.
    /// </summary>
    [DataField]
    public bool TargetEmoteForceChat = false;
    // World-of-Darkness (WoD) EDIT END
}

[Prototype("panelInteractionCategory")]
public sealed partial class PanelInteractionCategoryPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public string Name = string.Empty;
}
