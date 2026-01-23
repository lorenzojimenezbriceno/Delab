using Delab.Frontend.Repositories;
using Delab.Shared.ResponsesSec;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Delab.Frontend.Shared;

public partial class RecoverPassword
{
    private EmailDTO EmailDTO = new();

    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IRepository _repository { get; set; } = null!;

    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private async Task SendRecoverPasswordEmailTokenAsync()
    {
        var responseHttp = await _repository.PostAsync("/api/accounts/RecoverPassword", EmailDTO);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            _snackbar.Add("Error en la Recuperacion de la Clave", Severity.Error);
            return;
        }

        MudDialog.Cancel();
        _navigationManager.NavigateTo("/");
        _snackbar.Add("Se ha enviado un Correo con el Cambio de Clave", Severity.Success);
    }

    private void CloseModal()
    {
        MudDialog.Cancel();
    }
}