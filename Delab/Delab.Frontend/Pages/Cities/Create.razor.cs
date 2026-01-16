using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Delab.Frontend.Repositories;
using Delab.Shared.Entities;

namespace Delab.Frontend.Pages.Cities;

public partial class Create
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private City City = new();

    private Form? Form { get; set; }

    [Parameter]
    public int StateId { get; set; }

    private async Task _Create()
    {
        City.StateId = StateId;
        var responseHttp = await _repository.PostAsync<City>($"/api/Cities", City);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"/states/details/{City.StateId}");
    }

    private void ReturnAction()
    {
        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"/states/details/{City.StateId}");
    }
}