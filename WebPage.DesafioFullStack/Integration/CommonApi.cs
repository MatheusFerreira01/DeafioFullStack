namespace WebPage.DesafioFullStack.Integration;

public class CommonApi
{
    
    public static string DFSApiUrl;

    #region Rotas dos Dispositivos

    public static readonly string DFSGetListDevicesRoute = "/Device";
    public static readonly string DFSPostDeviceRoute = "/Device";
    public static readonly string DFSGetDeviceRoute = "/Device";
    public static readonly string DFSPutDeviceRoute = "/Device";
    public static readonly string DFSDeleteDeviceRoute = "/Device";

    #endregion

    public static void GetApiUlr(string? apiUrl)
    {
        DFSApiUrl = apiUrl;
    }
}
