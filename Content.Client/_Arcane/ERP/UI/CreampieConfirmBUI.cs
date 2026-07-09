using Content.Client._Arcane.ERP.UI;
using Content.Shared._Arcane.ERP;
using Robust.Client.UserInterface;

namespace Content.Client._Arcane.ERP;

public sealed class CreampieConfirmBUI(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables]
    private CreampieConfirmWindow? _menu;

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<CreampieConfirmWindow>();
        _menu.OnConfirm += inside =>
        {
            SendMessage(new CreampieConfirmMessage(inside));
            Close(); // закрываем окно после выбора
        };
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (_menu is null)
            return;

        if (state is CreampieConfirmBuiState newState)
        {
            _menu.User = newState.User;
            _menu.Target = newState.Target;
        }
    }
}
