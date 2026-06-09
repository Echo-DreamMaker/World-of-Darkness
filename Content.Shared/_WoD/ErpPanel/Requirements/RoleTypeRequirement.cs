// World-of-Darkness (WoD) EDIT START
// Добавлено: требование типа роли (RoleType) для ERP-интеракций
using Content.Shared._Arcane.ErpPanel.Requirements; // Базовый класс InvertableErpRequirement
using Content.Shared.Mind;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._WoD.ErpPanel.Requirements;

/// <summary>
///     Требование, проверяющее, что у игрока есть MindRole с указанным RoleType.
///     <para>
///     RoleType — это категория, к которой относится Mind Role игрока (например,
///     "SoloAntagonist", "TeamAntagonist", "Neutral", "Silicon", "FreeAgent").
///     Используется для более общих проверок, чем конкретный <see cref="JobPrototype"/>
///     или <see cref="AntagPrototype"/>.
///     </para>
///     <para>
///     Примеры:
///     - <c>RoleTypes: [Neutral]</c> — игрок должен быть мирной ролью (без антагонистских ролей).
///     - <c>RoleTypes: [SoloAntagonist, TeamAntagonist]</c> — игрок должен быть антагонистом (любым).
///     - <c>Inverted: true, RoleTypes: [SoloAntagonist, TeamAntagonist]</c> — игрок НЕ должен быть антагонистом.
///     </para>
/// </summary>
[Serializable, NetSerializable]
public sealed partial class RoleTypeRequirement : InvertableErpRequirement
{
    /// <summary>
    ///     Список RoleType, которыми должен (или не должен) быть игрок. Хотя бы один должен совпасть.
    /// </summary>
    [DataField(required: true)]
    public HashSet<ProtoId<RoleTypePrototype>> RoleTypes = new();

    public override bool IsAvailable(EntityUid uid, IEntityManager entityManager)
    {
        // Получаем mind игрока
        var mindSystem = entityManager.System<SharedMindSystem>();
        if (!mindSystem.TryGetMind(uid, out var mindId, out _))
            return Inverted; // Если у entity нет mind (например, NPC) - возвращаем инвертированное значение

        // Получаем все MindRole компоненты на mind entity
        // Каждый MindRole имеет свой RoleType (см. MindRoleComponent.RoleType)
        var mindRoleQuery = entityManager.EntityQueryEnumerator<MindRoleComponent>();
        var hasRequiredRoleType = false;

        while (mindRoleQuery.MoveNext(out _, out var mindRoleComp))
        {
            // Проверяем, что этот MindRole принадлежит нашему mind
            // MindRole хранит ссылку на Mind через Mind-свойство
            if (mindRoleComp.Mind.Owner != mindId)
                continue;

            if (mindRoleComp.RoleType is { } roleType && RoleTypes.Contains(roleType))
            {
                hasRequiredRoleType = true;
                break;
            }
        }

        return Inverted ? !hasRequiredRoleType : hasRequiredRoleType;
    }
}
// World-of-Darkness (WoD) EDIT END
