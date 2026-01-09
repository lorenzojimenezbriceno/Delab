namespace Delab.Frontend.Repositories;

public class HttpResponseWrapper<T>
{    
    public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage) 
    {
        Error = error;
        Response = response;
        HttpResponseMessage = httpResponseMessage;
    }

    public bool Error { get; set; }

    public T? Response { get; set; }

    public HttpResponseMessage HttpResponseMessage { get; set; }

    public async Task<string?> GetErrorMessageAsync()
    {
        if (!Error)
        {
            return null;
        }

        var statusCode = HttpResponseMessage.StatusCode;
        if (statusCode == System.Net.HttpStatusCode.NotFound)
        {
            return "Recurso no encontrado";
        }
        else if (statusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }
        else if (statusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return "Tienes que loguearte para hacer esta operación";
        }
        else if (statusCode == System.Net.HttpStatusCode.Forbidden)
        {
            return "No tienes permisos para hacer esta operación";
        }

        return "Ha ocurrido un error inesperado";
    }
}

