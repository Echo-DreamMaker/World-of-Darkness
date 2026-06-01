using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;

namespace Content.Shared._Arcane.ERP;

public sealed class OrgasmWeaknessSystem : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _speed = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<OrgasmWeaknessComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<OrgasmWeaknessComponent, RefreshMovementSpeedModifiersEvent>(OnRefreshSpeed);
        SubscribeLocalEvent<OrgasmWeaknessComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnInit(Entity<OrgasmWeaknessComponent> ent, ref ComponentInit args)
    {
        _speed.RefreshMovementSpeedModifiers(ent);
    }

    private void OnRefreshSpeed(Entity<OrgasmWeaknessComponent> ent, ref RefreshMovementSpeedModifiersEvent args)
    {
        args.ModifySpeed(ent.Comp.SpeedModifier, ent.Comp.SpeedModifier);
    }

    private void OnShutdown(Entity<OrgasmWeaknessComponent> ent, ref ComponentShutdown args)
    {
        ent.Comp.SpeedModifier = 1f;
        _speed.RefreshMovementSpeedModifiers(ent);
    }
}
