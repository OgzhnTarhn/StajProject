using SAP.Middleware.Connector;
using StajProject.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using StajProject.Models;

public class SapController : Controller
{
    public ActionResult GetUsers()
    {
        var users = new List<SAPUserModel>();

        // Bağlantı parametreleri:
        RfcConfigParameters parms = new RfcConfigParameters();
        parms.Add(RfcConfigParameters.Name, "SAP_DEST");
        parms.Add(RfcConfigParameters.AppServerHost, "172.31.2.10");
        parms.Add(RfcConfigParameters.SystemNumber, "00");
        parms.Add(RfcConfigParameters.User, "xdeveloper");
        parms.Add(RfcConfigParameters.Password, "Sf5687!pl@");
        parms.Add(RfcConfigParameters.Client, "100");
        parms.Add(RfcConfigParameters.Language, "EN");
        parms.Add(RfcConfigParameters.PoolSize, "5");

        // Bağlantıyı oluştur:
        RfcDestination dest = RfcDestinationManager.GetDestination(parms);
        RfcRepository repo = dest.Repository;

        IRfcFunction func = repo.CreateFunction("ZUSR_GET_USER");

        // Eğer input parametre varsa, burada set edebilirsin:
        // func.SetValue("IV_USERNAME", "admin");
        // func.SetValue("IV_PASSWORD", "1234");

        func.Invoke(dest);

        IRfcTable etUserInfo = func.GetTable("ET_USER_INFO");

        foreach (IRfcStructure row in etUserInfo)
        {
            users.Add(new SAPUserModel
            {
                Username = row.GetString("USERNAME"),
                Password = row.GetString("PASSWORD"),
                Role = row.GetString("ROLE")
            });
        }

        return View(users);
    }
}
