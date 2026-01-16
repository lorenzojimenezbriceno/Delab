using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Delab.Frontend.Repositories;
using Delab.Shared.Entities;

namespace Delab.Frontend.Pages.Cities;

public partial class Edit
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private City? City;

    private Form? Form { get; set; }

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHTTP = await _repository.GetAsync<City>($"api/cities/{Id}");

        if (responseHTTP.Error)
        {
            var messageError = "";
            if (responseHTTP.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                messageError = "Registro No Encontrado";
            }
            else
            {
                messageError = await responseHTTP.GetErrorMessageAsync();
            }
            await _sweetAlert.FireAsync("Error", messageError, SweetAlertIcon.Error);

            _navigationManager.NavigateTo("/countries");
        }
        else
        {
            City = responseHTTP.Response;
        }
    }

    private async Task _Update()
    {

        var responseHttp = await _repository.PutAsync<City>($"/api/cities", City!);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"/states/details/{City!.StateId}");
    }

    private void ReturnAction()
    {
        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"/states/details/{City!.StateId}");
    }
}