using SAP.Middleware.Connector;
using StajProject.Helpers;
using StajProject.Models;
using System.Web.Mvc;

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

                    // Kullanıcı bilgilerini Session'a ata!
                    Session["Username"] = model.Username;
                    Session["Role"] = model.Role;
                    Session["Password"] = model.Password; // Bunu EKELE! (user dashboard için lazım)

                    if (model.Role == "A")
                        return RedirectToAction("Dashboard", "Admin");
                    else if (model.Role == "U")
                        return RedirectToAction("UserDashboard", "Account");
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
                func.SetValue("IV_ROLE", "U"); // Hep User olarak kaydedilir

                func.Invoke(dest);

                string result = func.GetString("EV_RESULT");
                if (result.Contains("başarıyla"))
                    return RedirectToAction("Login");
                else
                    ModelState.AddModelError("", result);
            }
            catch (RfcAbapException ex)
            {
                ModelState.AddModelError("", "SAP Hatası: " + ex.Message);
            }
        }
        return View(model);
    }

    // Kullanıcı Dashboard'u
    [HttpGet]
    public ActionResult UserDashboard()
    {
        if (Session["Role"] == null || Session["Role"].ToString() != "U")
            return RedirectToAction("Login");

        var userInfo = new SAPUserModel();

        try
        {
            RfcDestination dest;
            IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_GET_USER", out dest);

            func.SetValue("IV_USERNAME", Session["Username"].ToString());
            func.SetValue("IV_PASSWORD", Session["Password"].ToString()); // Şifre burada Session'dan okunuyor!
            func.Invoke(dest);

            IRfcTable etUserInfo = func.GetTable("ET_USER_INFO");

            if (etUserInfo.Count > 0)
            {
                var userRow = etUserInfo[0];
                userInfo.Username = userRow.GetString("USERNAME");
                userInfo.Role = userRow.GetString("ROLE");
            }
        }
        catch (RfcAbapException ex)
        {
            ViewBag.ErrorMessage = "SAP Hatası: " + ex.Message;
        }

        return View(userInfo);
    }

    public ActionResult Logout()
    {
        Session.Clear();
        return RedirectToAction("Login");
    }
}
