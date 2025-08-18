using SAP.Middleware.Connector;

namespace StajProject.Helpers
{
    public static class SapConnectorBase
    {
        public static RfcConfigParameters GetSapParameters()
        {
            var parms = new RfcConfigParameters();
            parms.Add(RfcConfigParameters.Name, "SAP_DEST");
            parms.Add(RfcConfigParameters.AppServerHost, "");
            parms.Add(RfcConfigParameters.SystemNumber, "");
            parms.Add(RfcConfigParameters.User, "");
            parms.Add(RfcConfigParameters.Password, "");
            parms.Add(RfcConfigParameters.Client, "");
            parms.Add(RfcConfigParameters.Language, "");
            parms.Add(RfcConfigParameters.PoolSize, "");
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
