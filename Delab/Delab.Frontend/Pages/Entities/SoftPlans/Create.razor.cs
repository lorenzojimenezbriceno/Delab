using CurrieTechnologies.Razor.SweetAlert2;
using Delab.Frontend.Repositories;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace Delab.Frontend.Pages.Entities.SoftPlans;

public partial class Create
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private SoftPlan _softplan = new();

    private string BaseUrl = "/api/softplans";
    private string BaseView = "/softplans";

    private async Task _Create()
    {
        var responseHttp = await _repository.PostAsync($"{BaseUrl}", _softplan);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }

        _navigationManager.NavigateTo($"{BaseView}");
    }

    private void Return()
    {
        _navigationManager.NavigateTo($"{BaseView}");
    }
}