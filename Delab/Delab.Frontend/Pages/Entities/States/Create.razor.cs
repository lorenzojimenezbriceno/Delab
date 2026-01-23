using CurrieTechnologies.Razor.SweetAlert2;
using Delab.Shared.Entities;
using Delab.Frontend.Repositories;
using Microsoft.AspNetCore.Components;

namespace Delab.Frontend.Pages.Entities.States;

public partial class Create
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private State State = new();

    private Form? Form { get; set; }

    [Parameter]
    public int CountryId { get; set; }

    private async Task _Create()
    {
        State.Id = CountryId;
        var responseHttp = await _repository.PostAsync<State>($"/api/States", State);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"/countries/details/{CountryId}");
    }

    private void ReturnAction()
    {
        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"/countries/details/{CountryId}");
    }
}