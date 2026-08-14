using Content.Shared._Arcane.ERP.Organs;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared._Arcane.ERP;

public sealed partial class WashCumOverlayReaction : EntityEffect
{
    private const string VaginaSolutionName = "vagina";

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => null;

    public override void Effect(EntityEffectBaseArgs args)
    {
        var uid = args.TargetEntity;

        args.EntityManager.RemoveComponent<CumOverlayComponent>(uid);
        args.EntityManager.RemoveComponent<CreampieInsideComponent>(uid);

        DrainVagina(args);
    }

    private void DrainVagina(EntityEffectBaseArgs args)
    {
        var uid = args.TargetEntity;

        if (!args.EntityManager.TryGetComponent(uid, out BodyComponent? body))
            return;

        var bodySystem = args.EntityManager.System<SharedBodySystem>();
        var solutionSystem = args.EntityManager.System<SharedSolutionContainerSystem>();

        var vaginas = bodySystem.GetBodyOrganEntityComps<VaginaOrganComponent>((uid, body));
        foreach (var (organUid, _, _) in vaginas)
        {
            Entity<SolutionComponent>? solRef = null;
            if (!solutionSystem.ResolveSolution(organUid, VaginaSolutionName, ref solRef, out var solution))
                continue;

            var semenId = new ReagentId("Semen", null);
            var amount = solution.GetReagentQuantity(semenId);
            if (amount <= FixedPoint2.Zero)
                continue;

            var solnComp = args.EntityManager.GetComponent<SolutionComponent>(organUid);
            solutionSystem.RemoveReagent((organUid, solnComp), semenId, amount);
        }
    }
}
