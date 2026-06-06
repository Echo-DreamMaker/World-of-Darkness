using Content.Shared._Arcane.ERP.Organs;
using Content.Shared.Body.Organ;
using Robust.Shared.Serialization;

namespace Content.Shared._Arcane.ErpPanel.Requirements;

[Serializable, NetSerializable]
public sealed partial class EroticOrganRequirement : ErpRequirement
{
    [DataField(required: true)]
    public string Organ = string.Empty;

    [DataField]
    public bool RequireVisible = false;

    private static readonly IReadOnlyDictionary<string, Type> OrganTypes = new Dictionary<string, Type>
    {
        ["anus"] = typeof(AnusOrganComponent),
        ["penis"] = typeof(PenisOrganComponent),
        ["testicles"] = typeof(TesticlesOrganComponent),
        ["vagina"] = typeof(VaginaOrganComponent),
        ["uterus"] = typeof(UterusOrganComponent),
        ["breasts"] = typeof(BreastsOrganComponent),
    };

    public override bool IsAvailable(EntityUid uid, IEntityManager entityManager)
    {
        if (string.IsNullOrWhiteSpace(Organ))
            return false;

        if (!OrganTypes.TryGetValue(Organ, out var organType))
            return false;

        // Check all erogenous organs that belong to this entity
        // Uses direct EntityQuery instead of BodyComponent traversal, since
        // container replication may not be reliable on the client.
        var query = entityManager.EntityQueryEnumerator<EroticOrganComponent>();
        while (query.MoveNext(out var organUid, out var eroticComp))
        {
            if (!entityManager.HasComponent(organUid, organType))
                continue;

            // Check if this organ belongs to the target entity by walking up the transform hierarchy
            // (organs are children of body parts, which are children of the body entity)
            if (!IsDescendantOf(organUid, uid, entityManager))
                continue;

            if (RequireVisible)
                return eroticComp.Visible;
            else
                return true;
        }

        return false;
    }

    private static bool IsDescendantOf(EntityUid child, EntityUid potentialParent, IEntityManager entManager)
    {
        var current = child;
        while (entManager.EntityExists(current))
        {
            if (current == potentialParent)
                return true;

            var xform = entManager.GetComponent<TransformComponent>(current);
            if (xform.ParentUid == xform.Owner || xform.ParentUid == current)
                return false;

            current = xform.ParentUid;
        }

        return false;
    }
}
