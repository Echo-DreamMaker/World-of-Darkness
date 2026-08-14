using Content.Shared._Arcane.ERP.Organs;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared.Clothing.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Content.Shared.Inventory;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared._Arcane.ERP;

public sealed class CreampieSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _speed = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedBodySystem _body = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solution = default!;

    private const SlotFlags GroinCovering = SlotFlags.INNERCLOTHING | SlotFlags.UNDERWEAR;
    private const string VaginaSolutionName = "vagina";

    private static readonly EntProtoId SemenPuddleProto = "PuddleSemen";

    /// <summary>
    /// Сколько семени содержит одна лужица на полу.
    /// </summary>
    private const int PuddleReagentAmount = 10;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CreampieInsideComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<CreampieInsideComponent, RefreshMovementSpeedModifiersEvent>(OnRefreshSpeed);
        SubscribeLocalEvent<CreampieInsideComponent, ComponentShutdown>(OnShutdown);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_net.IsServer)
            return;

        var now = _timing.CurTime;
        var query = EntityQueryEnumerator<CreampieInsideComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (now < comp.NextLeakAt)
                continue;

            // Чем больше кремпаев, тем чаще вытекание.
            var nextDelay = TimeSpan.FromSeconds(MathF.Max(1.5f, 4 - comp.Count));

            // Проверяем, есть ли одежда на нижней части тела
            if (IsGroinCovered(uid))
            {
                comp.NextLeakAt = now + TimeSpan.FromSeconds(5);
                Dirty(uid, comp);
                continue;
            }

            // Пытаемся вытянуть семя из вагины
            if (TryDrainVagina(uid, comp.LeakThreshold, out var drained) && drained > 0)
                SpawnLeakPuddles(uid, drained);

            // Если семя в вагине закончилось — кремпай вытек, убираем эффект.
            if (!HasLeakableSemen(uid, comp.LeakThreshold))
            {
                RemCompDeferred<CreampieInsideComponent>(uid);
                RemCompDeferred<CumOverlayComponent>(uid);
                continue;
            }

            comp.NextLeakAt = now + nextDelay;
            Dirty(uid, comp);
        }
    }

    /// <summary>
    /// Пытается вытянуть порцию семени из вагины цели.
    /// </summary>
    private bool TryDrainVagina(EntityUid uid, FixedPoint2 threshold, out FixedPoint2 drained)
    {
        drained = FixedPoint2.Zero;

        if (!TryComp<BodyComponent>(uid, out var body))
            return false;

        var vaginas = _body.GetBodyOrganEntityComps<VaginaOrganComponent>((uid, body));
        foreach (var (organUid, _, _) in vaginas)
        {
            Entity<SolutionComponent>? solRef = null;
            if (!_solution.ResolveSolution(organUid, VaginaSolutionName, ref solRef, out var solution))
                continue;

            var semenId = new ReagentId("Semen", null);
            var semenAmount = solution.GetReagentQuantity(semenId);
            if (semenAmount < threshold)
                continue;

            // Забираем порцию и создаём на полу
            var toRemove = FixedPoint2.Min(semenAmount, threshold);
            _solution.RemoveReagent(solRef.Value, semenId, toRemove);
            drained = toRemove;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Осталось ли в вагине достаточно семени для новой лужицы.
    /// </summary>
    private bool HasLeakableSemen(EntityUid uid, FixedPoint2 threshold)
    {
        if (!TryComp<BodyComponent>(uid, out var body))
            return false;

        var vaginas = _body.GetBodyOrganEntityComps<VaginaOrganComponent>((uid, body));
        foreach (var (organUid, _, _) in vaginas)
        {
            Entity<SolutionComponent>? solRef = null;
            if (!_solution.ResolveSolution(organUid, VaginaSolutionName, ref solRef, out var solution))
                continue;

            if (solution.GetReagentQuantity(new ReagentId("Semen", null)) >= threshold)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Создаёт лужицы на полу пропорционально вытекшему количеству семени.
    /// </summary>
    private void SpawnLeakPuddles(EntityUid uid, FixedPoint2 drained)
    {
        var count = (int)Math.Ceiling(drained.Double() / PuddleReagentAmount);
        var coords = Transform(uid).Coordinates;
        for (var i = 0; i < count; i++)
        {
            var offset = new System.Numerics.Vector2(
                _random.NextFloat(-0.3f, 0.3f),
                _random.NextFloat(-0.3f, 0.3f));
            Spawn(SemenPuddleProto, coords.Offset(offset));
        }
    }

    private void OnInit(Entity<CreampieInsideComponent> ent, ref ComponentInit args)
    {
        _speed.RefreshMovementSpeedModifiers(ent);
    }

    private void OnRefreshSpeed(Entity<CreampieInsideComponent> ent, ref RefreshMovementSpeedModifiersEvent args)
    {
        args.ModifySpeed(ent.Comp.SpeedModifier, ent.Comp.SpeedModifier);
    }

    private void OnShutdown(Entity<CreampieInsideComponent> ent, ref ComponentShutdown args)
    {
        _speed.RefreshMovementSpeedModifiers(ent);
    }

    /// <summary>
    /// Проверяет, закрыта ли нижняя часть тела одеждой.
    /// </summary>
    private bool IsGroinCovered(EntityUid uid)
    {
        var coverage = SlotFlags.NONE;
        var enumerator = _inventory.GetSlotEnumerator(uid, GroinCovering);
        while (enumerator.NextItem(out var item))
        {
            if (TryComp<ClothingComponent>(item, out var clothing))
                coverage |= clothing.Slots;
        }

        return (coverage & GroinCovering) != SlotFlags.NONE;
    }
}
