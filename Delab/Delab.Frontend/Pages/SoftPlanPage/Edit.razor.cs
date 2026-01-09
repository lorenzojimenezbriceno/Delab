using CurrieTechnologies.Razor.SweetAlert2;
using Delab.Frontend.Repositories;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace Delab.Frontend.Pages.SoftPlanPage;

public partial class Edit
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private SoftPlan? _softplan;
    private string BaseUrl = "/api/softplans";
    private string BaseView = "/softplans";

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await _repository.GetAsync<SoftPlan>($"{BaseUrl}/{Id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }

        _softplan = responseHttp.Response;
    }

    private async Task _Edit()
    {
        var responseHttp = await _repository.PutAsync($"{BaseUrl}", _softplan);

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