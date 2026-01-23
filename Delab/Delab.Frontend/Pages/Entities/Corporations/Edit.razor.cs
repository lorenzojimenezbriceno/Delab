using CurrieTechnologies.Razor.SweetAlert2;
using Delab.Frontend.Repositories;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace Delab.Frontend.Pages.Entities.Corporations;

public partial class Edit
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private Corporation? Corporation;

    private Form? Form { get; set; }

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHTTP = await _repository.GetAsync<Corporation>($"/api/corporations/{Id}");
        if (responseHTTP.Error)
        {
            var message = await responseHTTP.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            _navigationManager.NavigateTo("/corporations");
            return;
        }
        Corporation = responseHTTP.Response;
    }

    private async Task _Edit()
    {
        var responseHTTP = await _repository.PutAsync("/api/corporations", Corporation);
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