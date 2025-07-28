using SAP.Middleware.Connector;
using StajProject.Models;
using System.Web.Mvc;
using StajProject.Models;

public class AccountController : Controller
{
    [HttpGet]
    public ActionResult Login()
    {
        return View(new LoginModel());
    }

    [HttpPost]
    public ActionResult Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            // SAP Bağlantı parametreleri
            RfcConfigParameters parms = new RfcConfigParameters();
            parms.Add(RfcConfigParameters.Name, "SAP_DEST");
            parms.Add(RfcConfigParameters.AppServerHost, "172.31.2.10");
            parms.Add(RfcConfigParameters.SystemNumber, "00");
            parms.Add(RfcConfigParameters.User, "xdeveloper");
            parms.Add(RfcConfigParameters.Password, "Sf5687!pl@");
            parms.Add(RfcConfigParameters.Client, "100");
            parms.Add(RfcConfigParameters.Language, "EN");
            parms.Add(RfcConfigParameters.PoolSize, "5");

            try
            {
                RfcDestination dest = RfcDestinationManager.GetDestination(parms);
                RfcRepository repo = dest.Repository;
                IRfcFunction func = repo.CreateFunction("ZUSR_GET_USER");

                func.SetValue("IV_USERNAME", model.Username);
                func.SetValue("IV_PASSWORD", model.Password);
                func.Invoke(dest);

                IRfcTable etUserInfo = func.GetTable("ET_USER_INFO");

                if (etUserInfo.Count > 0)
                {
                    // Kullanıcı bulundu
                    var userRow = etUserInfo[0];
                    model.Role = userRow.GetString("ROLE");

                    // Kullanıcı rolüne göre yönlendir
                    if (model.Role == "A")
                        return RedirectToAction("AdminIndex", "Home");
                    else if (model.Role == "U")
                        return RedirectToAction("UserIndex", "Home");
                    else
                        ModelState.AddModelError("", "Geçersiz rol.");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
                }
            }
            catch (RfcAbapException ex)
            {
                ModelState.AddModelError("", "SAP Hatası: " + ex.Message);
            }
        }
        return View(model);
    }
}
