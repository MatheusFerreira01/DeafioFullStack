namespace WebPage.DesafioFullStack.Integration;

public static class HttpClientHandlerFactory
{
    public static HttpClientHandler GetHandler()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        return handler;
    }
}