using Content.Server.Chat.Systems;
using Content.Shared._Arcane.ERP;
using Content.Shared._Arcane.ERP.Organs;
using Content.Shared.Body.Components;
using Content.Shared.Body.Organ;
using Content.Shared.Body.Systems;
using Content.Shared.Chat;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Content.Shared.Humanoid;
using Content.Shared.Interaction;
using Content.Shared.Inventory;
using Content.Shared.Popups;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server._Arcane.ERP;

public sealed class ServerCreampieSystem : EntitySystem
{
    [Dependency] private readonly SharedUserInterfaceSystem _ui = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedBodySystem _body = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solution = default!;

    public override void Initialize()
    {
        base.Initialize();

        // Подписываемся на сообщения от BUI
        Subs.BuiEvents<CreampiePendingComponent>(CreampieConfirmKey.Key, subs =>
        {
            subs.Event<CreampieConfirmMessage>(OnCreampieConfirmMessage);
        });
    }

    /// <summary>
    /// Открывает окно подтверждения "кончить внутрь?" для мужского персонажа.
    /// Добавляет временный компонент для отслеживания цели.
    /// </summary>
    public void RequestCreampieConfirm(EntityUid user, EntityUid target)
    {
        if (!HasComp<ActorComponent>(user))
            return;

        // Добавляем компонент для отслеживания
        var comp = EnsureComp<CreampiePendingComponent>(user);
        comp.Target = GetNetEntity(target);
        Dirty(user, comp);

        // Регистрируем BUI для этого компонента
        var interfaceData = new InterfaceData(
            clientType: "Content.Client._Arcane.ERP.CreampieConfirmBUI"
        );
        _ui.SetUi(user, CreampieConfirmKey.Key, interfaceData);

        // Открываем UI
        _ui.TryOpenUi(user, CreampieConfirmKey.Key, user);

        var state = new CreampieConfirmBuiState(GetNetEntity(user), GetNetEntity(target));
        _ui.SetUiState(user, CreampieConfirmKey.Key, state);
    }

    /// <summary>
    /// Обрабатывает ответ от клиента (выбор "да"/"нет").
    /// </summary>
    private void OnCreampieConfirmMessage(Entity<CreampiePendingComponent> ent, ref CreampieConfirmMessage args)
    {
        var user = ent.Owner;
        var targetNet = ent.Comp.Target;

        // Удаляем временный компонент
        RemComp<CreampiePendingComponent>(user);

        if (targetNet == null)
            return;

        var target = GetEntity(targetNet.Value);

        if (!Exists(target))
            return;

        if (args.Inside)
        {
            // Кончил внутрь — добавляем компонент кремпая женщине
            if (TryComp<HumanoidAppearanceComponent>(target, out var humanoid) && humanoid.Sex == Sex.Female)
            {
                ApplyCreampie(target);
            }
        }
    }

    /// <summary>
    /// Добавляет/обновляет компонент кремпая на женщине.
    /// Семя добавляется в раствор вагины для отслеживания через дебаг-редактор жидкостей.
    /// </summary>
    public void ApplyCreampie(EntityUid target)
    {
        // Добавляем/обновляем CreampieInsideComponent
        var creampie = EnsureComp<CreampieInsideComponent>(target);
        creampie.Count++;
        if (creampie.NextLeakAt == TimeSpan.Zero)
            creampie.NextLeakAt = _timing.CurTime + TimeSpan.FromSeconds(10);
        Dirty(target, creampie);

        // Добавляем CumOverlay для визуального эффекта
        var overlay = EnsureComp<CumOverlayComponent>(target);
        overlay.Count++;
        Dirty(target, overlay);

        // Добавляем семя в раствор вагины (если орган есть)
        TryFillVagina(target);

        // Отправляем приватный popup-алерт (видит только сама женщина)
        _popup.PopupEntity(Loc.GetString("creampie-target-message"), target, target, PopupType.MediumCaution);
    }

    /// <summary>
    /// Находит орган вагины у цели и добавляет туда Semen.
    /// </summary>
    private void TryFillVagina(EntityUid target)
    {
        if (!TryComp<BodyComponent>(target, out var body))
            return;

        var vaginas = _body.GetBodyOrganEntityComps<VaginaOrganComponent>((target, body));
        foreach (var (organUid, _, _) in vaginas)
        {
            _solution.EnsureSolution(organUid, "vagina", out var solution, FixedPoint2.New(100));
            if (solution != null)
            {
                var solnComp = Comp<SolutionComponent>(organUid);
                _solution.TryAddReagent((organUid, solnComp), "Semen", FixedPoint2.New(20));
            }
        }
    }
}
