using SAP.Middleware.Connector;

public class SAPConfig : IDestinationConfiguration
{
    public bool ChangeEventsSupported() => false;

    public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

    public RfcConfigParameters GetParameters(string destinationName)
    {
        if ("SAP_DEST".Equals(destinationName))
        {
            RfcConfigParameters parms = new RfcConfigParameters();
            parms.Add(RfcConfigParameters.Name, "SAP_DEST");
            parms.Add(RfcConfigParameters.AppServerHost, "172.31.2.10");
            parms.Add(RfcConfigParameters.SystemNumber, "00");
            parms.Add(RfcConfigParameters.User, "xdeveloper");
            parms.Add(RfcConfigParameters.Password, "Sf5687!pl@");
            parms.Add(RfcConfigParameters.Client, "100");
            parms.Add(RfcConfigParameters.Language, "EN");
            parms.Add(RfcConfigParameters.PoolSize, "5");
        }
        return null;
    }
}
