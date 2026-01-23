using Delab.Frontend.Repositories;
using Delab.Frontend.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Delab.Frontend.Pages.EntitiesSoftSec.Auth;

public partial class ConfirmEmail
{
    private string? message;

    [Inject] private NavigationManager _navigation { get; set; } = null!;
    [Inject] private IDialogService _dialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    [Parameter, SupplyParameterFromQuery] public string UserId { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public string Token { get; set; } = string.Empty;

    protected async Task ConfirmAccountAsync()
    {
        var responseHttp = await Repository.GetAsync($"/api/accounts/ConfirmEmail/?userId={UserId}&token={Token}");
        if (responseHttp.Error)
        {
            message = await responseHttp.GetErrorMessageAsync();
            _navigation.NavigateTo("/");
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        Snackbar.Add("Su Cuenta ha sido Confirmada", Severity.Success);
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        await _dialogService.ShowAsync<Login>("Iniciar Sesion", closeOnEscapeKey);
    }
}