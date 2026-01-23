using CurrieTechnologies.Razor.SweetAlert2;
using Delab.Shared.Entities;
using Delab.Frontend.Repositories;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Delab.Frontend.Pages.Entities.Corporations;

public partial class Details
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;

    private Corporation? Corporation;
    private SoftPlan? SoftPlan;
    private Country? Country;

    private Form? Form { get; set; }

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Cargar la corporación
        var responseHTTP = await _repository.GetAsync<Corporation>($"/api/corporations/{Id}");
        if (responseHTTP.Error)
        {
            var message = await responseHTTP.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            _navigationManager.NavigateTo("/corporations");
            return;
        }
        Corporation = responseHTTP.Response;

        // Cargar el Plan asociado
        var responseHTTP2 = await _repository.GetAsync<SoftPlan>($"/api/softplans/{Corporation!.SoftPlanId}");
        if (responseHTTP2.Error)
        {
            var message = await responseHTTP2.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            _navigationManager.NavigateTo("/corporations");
            return;
        }
        SoftPlan = responseHTTP2.Response;

        // Cargar el Pais asociado
        var responseHTTP3 = await _repository.GetAsync<Country>($"/api/countries/{Corporation.CountryId}");
        if (responseHTTP3.Error)
        {
            var message = await responseHTTP3.GetErrorMessageAsync();
            await _sweetAlert.FireAsync("Error", message, SweetAlertIcon.Error);
            _navigationManager.NavigateTo("/corporations");
            return;
        }
        Country = responseHTTP3.Response;
    }

    private string GetDisplayName<T>(Expression<Func<T>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            var property = memberExpression.Member as PropertyInfo;
            if (property != null)
            {
                var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    return displayAttribute.Name!;
                }
            }
        }
        return "Texto no definido";
    }
}