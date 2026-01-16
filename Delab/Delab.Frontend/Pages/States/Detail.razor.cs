using CurrieTechnologies.Razor.SweetAlert2;
using Delab.Shared.Entities;
using Delab.Frontend.Repositories;
using Microsoft.AspNetCore.Components;

namespace Delab.Frontend.Pages.States;

public partial class Detail
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    // Variables privadas
    private int CurrentPage = 1;
    private int TotalPages;

    public State? State { get; set; }
    public List<City>? Cities { get; set; }

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
        // Cargar el estado 
        var responseHttpState = await _repository.GetAsync<State>($"/api/states/{Id}");
        if (responseHttpState.Error)
        {
            if (responseHttpState.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await _sweetAlert.FireAsync("Error", "Registro No Encontrado", SweetAlertIcon.Error);
            }
            else
            {
                var messageError = await responseHttpState.GetErrorMessageAsync();
                await _sweetAlert.FireAsync("Error", messageError, SweetAlertIcon.Error);
            }
            await SelectedPage(CurrentPage);
        }
        else
        {
            State = responseHttpState.Response;
        }

        // Cargar las ciudades del estado
        var responseHttpCities = await _repository.GetAsync<List<City>>($"/api/cities?Id={Id}&page={page}");
        if (responseHttpCities.Error)
        {
            if (responseHttpCities.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await _sweetAlert.FireAsync("Error", "Registro No Encontrado", SweetAlertIcon.Error);
            }
            else
            {
                var messageError = await responseHttpCities.GetErrorMessageAsync();
                await _sweetAlert.FireAsync("Error", messageError, SweetAlertIcon.Error);
            }
            await SelectedPage(CurrentPage);
        }
        else
        {
            Cities = responseHttpCities.Response;
            TotalPages = int.Parse(responseHttpCities.HttpResponseMessage.Headers.GetValues("TotalPages").FirstOrDefault()!);
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

        var responseHTTP = await _repository.DeleteAsync($"/api/cities/{id}");
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