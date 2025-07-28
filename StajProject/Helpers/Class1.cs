using SAP.Middleware.Connector;

namespace StajProject.Helpers
{
    public static class SapConnectorBase
    {
        public static RfcConfigParameters GetSapParameters()
        {
            var parms = new RfcConfigParameters();
            parms.Add(RfcConfigParameters.Name, "SAP_DEST");
            parms.Add(RfcConfigParameters.AppServerHost, "172.31.2.10");
            parms.Add(RfcConfigParameters.SystemNumber, "00");
            parms.Add(RfcConfigParameters.User, "xdeveloper");
            parms.Add(RfcConfigParameters.Password, "Sf5687!pl@");
            parms.Add(RfcConfigParameters.Client, "100");
            parms.Add(RfcConfigParameters.Language, "EN");
            parms.Add(RfcConfigParameters.PoolSize, "5");
            return parms;
        }

        public static IRfcFunction CreateFunction(string functionName, out RfcDestination dest)
        {
            dest = RfcDestinationManager.GetDestination(GetSapParameters());
            var repo = dest.Repository;
            return repo.CreateFunction(functionName);
        }
    }
}
