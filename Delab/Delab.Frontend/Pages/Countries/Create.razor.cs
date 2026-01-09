using CurrieTechnologies.Razor.SweetAlert2;
using Daleb.Shared.Entities;
using Delab.Frontend.Repositories;
using Microsoft.AspNetCore.Components;

namespace Delab.Frontend.Pages.Countries;

public partial class Create
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private Country Country = new();

    private Form? Form { get; set; }

    private async Task _Create()
    {
        var responseHttp = await _repository.PostAsync<Country>($"/api/countries", Country);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo("/countries");
    }

    private void ReturnAction()
    {
        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo("/countries");
    }
}