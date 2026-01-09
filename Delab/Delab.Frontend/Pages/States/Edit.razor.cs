using CurrieTechnologies.Razor.SweetAlert2;
using Daleb.Shared.Entities;
using Delab.Frontend.Repositories;
using Microsoft.AspNetCore.Components;

namespace Delab.Frontend.Pages.States;

public partial class Edit
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private State? State;

    private Form? Form { get; set; }

    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHTTP = await _repository.GetAsync<State>($"api/states/{Id}");

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
            State = responseHTTP.Response;
        }
    }

    private async Task _Update()
    {

        var responseHttp = await _repository.PutAsync<State>($"/api/states", State!);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"/countries/details/{State!.Id}");
    }

    private void ReturnAction()
    {
        Form!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"/countries/details/{State!.Id}");
    }
}