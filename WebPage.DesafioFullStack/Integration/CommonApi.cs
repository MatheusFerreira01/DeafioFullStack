namespace WebPage.DesafioFullStack.Integration;

public class CommonApi
{

    public static readonly string DFSApiUrl = "https://localhost:7078";

    #region Rotas dos Dispositivos
    public static readonly string DFSGetListDevicesRoute = "/Device";
    public static readonly string DFSPostDeviceRoute = "/Device";
    public static readonly string DFSGetDeviceRoute = "/Device{id}";
    public static readonly string DFSPutDeviceRoute = "/Device{id}";
    public static readonly string DFSDeleteDeviceRoute = "/Device{id}";

    #endregion
}
