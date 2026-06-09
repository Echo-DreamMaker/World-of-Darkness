// World-of-Darkness (WoD) EDIT START
// Добавлено: требование антагониста (Antag) для ERP-интеракций
using Content.Shared._Arcane.ErpPanel.Requirements; // Базовый класс InvertableErpRequirement
using Content.Shared.Mind;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._WoD.ErpPanel.Requirements;

/// <summary>
///     Требование, проверяющее является ли игрок антагонистом (или конкретным антагонистом).
///     <para>
///     Примеры использования:
///     - <c>Inverted: false, Antags: []</c> — игрок должен быть ЛЮБЫМ антагонистом (без указания списка).
///     - <c>Inverted: false, Antags: [Nukeops, Traitor]</c> — игрок должен быть Nukeops или Traitor.
///     - <c>Inverted: true, Antags: []</c> — игрок НЕ должен быть антагонистом (только мирные).
///     </para>
/// </summary>
[Serializable, NetSerializable]
public sealed partial class AntagRequirement : InvertableErpRequirement
{
    /// <summary>
    ///     Конкретные антагонисты, которыми должен (или не должен) быть игрок.
    ///     Если список пустой — проверяется ЛЮБОЙ антагонист (с флагом Antagonist = true).
    ///     <para>
    ///     Это ID прототипов ролей, а не AntagPrototype. Например, для вампира
    ///     нужно указывать ID MindRole прототипа.
    ///     </para>
    /// </summary>
    [DataField]
    public HashSet<string> Antags = new();

    public override bool IsAvailable(EntityUid uid, IEntityManager entityManager)
    {
        // Получаем mind игрока
        var mindSystem = entityManager.System<SharedMindSystem>();
        if (!mindSystem.TryGetMind(uid, out var mindId, out _))
            return Inverted; // Если у entity нет mind (например, NPC) - возвращаем инвертированное значение

        // Получаем систему ролей
        var roleSystem = entityManager.System<SharedRoleSystem>();
        var allRoles = roleSystem.MindGetAllRoleInfo(mindId);

        // Проверяем, является ли игрок антагонистом
        var isAntag = false;
        foreach (var role in allRoles)
        {
            if (!role.Antagonist)
                continue;

            // Если список конкретных антагов пустой, то подходит любой антагонист
            if (Antags.Count == 0)
            {
                isAntag = true;
                break;
            }

            // Иначе проверяем, что Prototype роли есть в списке
            if (Antags.Contains(role.Prototype))
            {
                isAntag = true;
                break;
            }
        }

        return Inverted ? !isAntag : isAntag;
    }
}
// World-of-Darkness (WoD) EDIT END
