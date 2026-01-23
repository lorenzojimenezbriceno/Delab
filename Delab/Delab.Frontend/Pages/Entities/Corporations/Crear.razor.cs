using CurrieTechnologies.Razor.SweetAlert2;
using Delab.Frontend.Repositories;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace Delab.Frontend.Pages.Entities.Corporations;

public partial class Crear
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private Corporation Corporation = new();

    private Form? Form { get; set; }

    private async Task Create()
    {
        var responseHTTP = await _repository.PostAsync("/api/corporations", Corporation);
        if (responseHTTP.Error)
        {
            var message = await responseHTTP.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            _navigationManager.NavigateTo("/corporations");
            return;
        }

       Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo("/corporations");
    }

    private void Return()
    {
        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo("/corporations");
    }
}