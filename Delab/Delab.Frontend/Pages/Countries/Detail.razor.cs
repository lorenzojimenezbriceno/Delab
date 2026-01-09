using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Delab.Frontend.Repositories;
using Daleb.Shared.Entities;

namespace Delab.Frontend.Pages.Countries;

public partial class Detail
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    // Variables privadas
    private int CurrentPage = 1;
    private int TotalPages;

    public Country? Country { get; set; }
    public List<State>? States { get; set; }

    [Parameter]
    public int Id { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await Cargar();
    }

    private async Task SelectedPage(int page)
    {
        CurrentPage = page;
        await Cargar(page);
    }

    private async Task Cargar(int page = 1)
    {
        // Cargar el pais 
        var responseHttpCountry = await _repository.GetAsync<Country>($"/api/countries/{Id}");
        if (responseHttpCountry.Error)
        {
            if (responseHttpCountry.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await _sweetAlert.FireAsync("Error", "Registro No Encontrado", SweetAlertIcon.Error);
            }
            else
            {
                var messageError = await responseHttpCountry.GetErrorMessageAsync();
                await _sweetAlert.FireAsync("Error", messageError, SweetAlertIcon.Error);
            }
            await SelectedPage(CurrentPage);
        }
        else
        {
            Country = responseHttpCountry.Response;
        }

        // Cargar los estados del pais
        var responseHttpStates = await _repository.GetAsync<List<State>>($"/api/states?Id={Id}&page={page}");
        if (responseHttpStates.Error)
        {
            if (responseHttpStates.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await _sweetAlert.FireAsync("Error", "Registro No Encontrado", SweetAlertIcon.Error);
            }
            else
            {
                var messageError = await responseHttpCountry.GetErrorMessageAsync();
                await _sweetAlert.FireAsync("Error", messageError, SweetAlertIcon.Error);
            }
            await SelectedPage(CurrentPage);
        }
        else
        {
            States = responseHttpStates.Response;
            TotalPages = int.Parse(responseHttpStates.HttpResponseMessage.Headers.GetValues("TotalPages").FirstOrDefault()!);
        }
    }

    public async Task Borrar(int id)
    {
        var result = await _sweetAlert.FireAsync(new SweetAlertOptions
        {
            Title = "Confirmacion",
            Text = "Estas Seguro de Borrar el Registro",
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true
        });

        var confirmation = string.IsNullOrEmpty(result.Value);
        if (confirmation)
        {
            return;
        }

        var responseHTTP = await _repository.DeleteAsync($"/api/states/{id}");
        if (responseHTTP.Error)
        {
            if (responseHTTP.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await _sweetAlert.FireAsync("Error", "Registro No Encontrado", SweetAlertIcon.Error);
            }
            else
            {
                var messageError = await responseHTTP.GetErrorMessageAsync();
                await _sweetAlert.FireAsync("Error", messageError, SweetAlertIcon.Error);
            }
            await SelectedPage(CurrentPage);
        }
        else
        {
            await SelectedPage(CurrentPage);
        }
    }
}