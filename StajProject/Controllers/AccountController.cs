using System.Web.Mvc;
using SAP.Middleware.Connector;
using StajProject.Helpers;
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
            try
            {
                RfcDestination dest;
                IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_GET_USER", out dest);

                func.SetValue("IV_USERNAME", model.Username);
                func.SetValue("IV_PASSWORD", model.Password);
                func.Invoke(dest);

                IRfcTable etUserInfo = func.GetTable("ET_USER_INFO");

                if (etUserInfo.Count > 0)
                {
                    var userRow = etUserInfo[0];
                    model.Role = userRow.GetString("ROLE");

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

    [HttpGet]
    public ActionResult Register()
    {
        return View(new RegisterModel());
    }

    [HttpPost]
    public ActionResult Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                RfcDestination dest;
                IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_INSERT_USER", out dest);

                func.SetValue("IV_USERNAME", model.Username);
                func.SetValue("IV_PASSWORD", model.Password);
                func.SetValue("IV_ROLE", "U"); // Kayıt olan kullanıcı hep User olur

                func.Invoke(dest);

                model.Message = func.GetString("EV_RESULT");
            }
            catch (RfcAbapException ex)
            {
                model.Message = "SAP Hatası: " + ex.Message;
            }
        }
        return View(model);
    }

    // Admin panelinde kullanılacak örnek altyapı:
    [HttpGet]
    public ActionResult AddUser()
    {
        return View(new RegisterModel());
    }

    [HttpPost]
    public ActionResult AddUser(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                RfcDestination dest;
                IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_INSERT_USER", out dest);

                func.SetValue("IV_USERNAME", model.Username);
                func.SetValue("IV_PASSWORD", model.Password);
                func.SetValue("IV_ROLE", model.Role); // Admin panelinde seçilen rol gönderilir

                func.Invoke(dest);

                model.Message = func.GetString("EV_RESULT");
            }
            catch (RfcAbapException ex)
            {
                model.Message = "SAP Hatası: " + ex.Message;
            }
        }
        return View(model);
    }

    // Diğer fonksiyonlar için (update/delete) de bu altyapıyı kullanabilirsin!
}
