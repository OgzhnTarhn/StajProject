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

                    if (model.Role == "A" || model.Role == "U")
                        return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendir, oradan role göre yönlendirilecek
                    else
                        ModelState.AddModelError("", "Invalid role.");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            catch (RfcAbapException ex)
            {
                ModelState.AddModelError("", "SAP Error: " + ex.Message);
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
                if (result.Contains("successfully") || result.Contains("başarıyla"))
                    return RedirectToAction("Login");
                else
                    ModelState.AddModelError("", result);
            }
            catch (RfcAbapException ex)
            {
                ModelState.AddModelError("", "SAP Error: " + ex.Message);
            }
        }
        return View(model);
    }

    // Kullanıcı Dashboard'u
    [HttpGet]
    public ActionResult UserDashboard()
    {
        if (Session["Role"] == null || (Session["Role"].ToString() != "U" && Session["Role"].ToString() != "A"))
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
            ViewBag.ErrorMessage = "SAP Error: " + ex.Message;
        }

        return View(userInfo);
    }
    [HttpGet]
    public ActionResult EditProfile()
    {
        // Session kontrolü - Admin ve User her ikisi de erişebilir
        if (Session["Role"] == null || (Session["Role"].ToString() != "U" && Session["Role"].ToString() != "A"))
            return RedirectToAction("Login");

        var model = new EditUserProfileModel { Username = Session["Username"].ToString() };
        return View(model);
    }

    [HttpPost]
    public ActionResult EditProfile(EditUserProfileModel model)
    {
        if (Session["Role"] == null || (Session["Role"].ToString() != "U" && Session["Role"].ToString() != "A"))
            return RedirectToAction("Login");

        if (ModelState.IsValid)
        {
            try
            {
                RfcDestination dest;
                IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_UPDATE_USER", out dest);

                func.SetValue("IV_USERNAME", model.Username);
                func.SetValue("IV_OLD_PASSWORD", model.OldPassword);
                func.SetValue("IV_NEW_USERNAME", model.NewUsername ?? "");
                func.SetValue("IV_NEW_PASSWORD", model.NewPassword ?? "");
                func.SetValue("IV_ROLE", ""); // User rol değiştiremez

                func.Invoke(dest);

                model.Message = func.GetString("EV_RESULT");

                // Kullanıcı adı değiştiyse session güncelle
                if (!string.IsNullOrEmpty(model.NewUsername) && (model.Message.Contains("güncellendi") || model.Message.Contains("updated")))
                    Session["Username"] = model.NewUsername;
            }
            catch (RfcAbapException ex)
            {
                model.Message = "SAP Error: " + ex.Message;
            }
        }
        return View(model);
    }

    public ActionResult Logout()
    {
        Session.Clear();
        return RedirectToAction("Login");
    }
}

