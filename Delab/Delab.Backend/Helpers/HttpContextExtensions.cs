using Microsoft.EntityFrameworkCore;

namespace Delab.Backend.Helpers;

public static class HttpContextExtensions
{
    public async static Task InsertParameterPagination<T>(this HttpContext httpContext, IQueryable<T> queryable, int cantidadRegistrosAMostrar)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        double conteo = await queryable.CountAsync();
        double totalPaginas = Math.Ceiling(conteo / cantidadRegistrosAMostrar);

        // Se pasa la cantidad de registros y total de páginas en el encabezado http
        httpContext.Response.Headers.Append("Counting", conteo.ToString());
        httpContext.Response.Headers.Append("TotalPages", totalPaginas.ToString());
    }
}
