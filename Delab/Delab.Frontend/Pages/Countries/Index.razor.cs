using CurrieTechnologies.Razor.SweetAlert2;
using Daleb.Shared.Entities;
using Delab.Frontend.Repositories;
using Microsoft.AspNetCore.Components;

namespace Delab.Frontend.Pages.Countries;

public partial class Index
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private int CurrentPage = 1;
    private int TotalPages;

    public List<Country>? Countries { get; set; }

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
        var responseHttp = await _repository.GetAsync<List<Country>>($"/api/countries?page={page}");
        Countries = responseHttp.Response;

        TotalPages = int.Parse(responseHttp.HttpResponseMessage.Headers.GetValues("TotalPages").FirstOrDefault()!);
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

        var responseHTTP = await _repository.DeleteAsync($"/api/countries/{id}");
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