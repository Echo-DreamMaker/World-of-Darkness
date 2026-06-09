// World-of-Darkness (WoD) EDIT START
// Добавлено: требование работы (Job) для ERP-интеракций
using Content.Shared._Arcane.ErpPanel.Requirements; // Базовый класс InvertableErpRequirement
using Content.Shared.Mind;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._WoD.ErpPanel.Requirements;

/// <summary>
///     Требование, проверяющее что у игрока есть указанная работа (Job).
///     Например, чтобы использовать интеракцию "Отдать приказ", нужно быть Captain.
///     Используется в <c>userRequirements</c>/<c>targetRequirements</c> панели взаимодействий.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class JobRequirement : InvertableErpRequirement
{
    /// <summary>
    ///     Список ID работ (JobPrototype), которые допустимы. Хотя бы одна должна совпасть.
    ///     Пример: <c>- Captain</c>, <c>- HeadOfSecurity</c>, <c>- ChiefMedicalOfficer</c>.
    /// </summary>
    [DataField(required: true)]
    public HashSet<ProtoId<JobPrototype>> Jobs = new();

    public override bool IsAvailable(EntityUid uid, IEntityManager entityManager)
    {
        // Получаем mind игрока
        var mindSystem = entityManager.System<SharedMindSystem>();
        if (!mindSystem.TryGetMind(uid, out var mindId, out _))
            return Inverted; // Если у entity нет mind (например, NPC) - возвращаем инвертированное значение

        // Получаем систему ролей
        var roleSystem = entityManager.System<SharedRoleSystem>();

        // Проверяем, есть ли у игрока одна из указанных работ
        var hasRequiredJob = false;
        foreach (var jobId in Jobs)
        {
            if (roleSystem.MindHasRole<JobRoleComponent>(mindId, out var jobRole) &&
                jobRole.Value.Comp1.JobPrototype == jobId)
            {
                hasRequiredJob = true;
                break;
            }
        }

        return Inverted ? !hasRequiredJob : hasRequiredJob;
    }
}
// World-of-Darkness (WoD) EDIT END
