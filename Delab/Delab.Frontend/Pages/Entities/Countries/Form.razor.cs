using CurrieTechnologies.Razor.SweetAlert2;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace Delab.Frontend.Pages.Entities.Countries;

public partial class Form
{
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private EditContext _editContext = null!;

    public bool FormPostedSuccessfully { get; set; } = false;

    [Parameter, EditorRequired]
    public Country Country { get; set; } = null!;

    [Parameter, EditorRequired]
    public EventCallback OnSubmit { get; set; }

    [Parameter, EditorRequired]
    public EventCallback ReturnAction { get; set; }

    protected override void OnInitialized()
    {
        _editContext = new(Country);
    }

    private async Task OnBeforeInternalNavigation(LocationChangingContext context)
    {
        var formWasEdited = _editContext.IsModified();

        if (!formWasEdited)
        {
            return;
        }

        if (FormPostedSuccessfully)
        {
            return;
        }

        var result = await _sweetAlert.FireAsync(new SweetAlertOptions
        {
            Title = "Confirmación",
            Text = "¿Deseas abandonar la página y perder los cambios?",
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true
        });

        var confirm = !string.IsNullOrEmpty(result.Value);

        if (confirm)
        {
            return;
        }

        context.PreventNavigation();
    }
}