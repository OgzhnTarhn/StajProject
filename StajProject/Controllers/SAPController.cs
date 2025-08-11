using SAP.Middleware.Connector;
using StajProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;


public class SapController : Controller
{
    public ActionResult GetUsers()
    {
        var users = new List<SAPUserModel>();

        var parms = new RfcConfigParameters
{
    { RfcConfigParameters.Name, "SAP_DEST" },
    { RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP.AppServerHost"] },
    { RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP.SystemNumber"] },
    { RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP.User"] },
    { RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP.Password"] },
    { RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP.Client"] },
    { RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP.Language"] },
    { RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP.PoolSize"] }
};

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
    public ActionResult GetBlocks(string blockId = null)
    {
        var headers = new List<BlockHeaderModel>();
        var details = new List<BlockDetailModel>();

        try
        {
            // Web.config'ten ayarları oku
            var parms = new RfcConfigParameters
                {
                    { RfcConfigParameters.Name, "SAP_DEST" },
                    { RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP.AppServerHost"] },
                    { RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP.SystemNumber"] },
                    { RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP.User"] },
                    { RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP.Password"] },
                    { RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP.Client"] },
                    { RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP.Language"] },
                    { RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP.PoolSize"] }
                };

            // Bağlantı ve repository
            RfcDestination dest = RfcDestinationManager.GetDestination(parms);
            RfcRepository repo = dest.Repository;

            // FM: ZBLOCK_GET
            IRfcFunction func = repo.CreateFunction("ZBLOCK_GET");

            if (!string.IsNullOrWhiteSpace(blockId))
                func.SetValue("IV_BLOCK_ID", blockId);

            // FM'i çalıştır
            func.Invoke(dest);

            // HEADER tablosu
            IRfcTable etHdr = func.GetTable("ET_HDR");
            foreach (IRfcStructure row in etHdr)
            {
                headers.Add(new BlockHeaderModel
                {
                    Mandt = row.GetString("MANDT"),
                    BlockId = row.GetString("BLOCK_ID"),
                    Title = row.GetString("TITLE"),
                    Erdat = row.GetString("ERDAT"),
                    Aedat = row.GetString("AEDAT")
                });
            }

            // DETAIL tablosu
            IRfcTable etDtl = func.GetTable("ET_DTL");
            foreach (IRfcStructure row in etDtl)
            {
                details.Add(new BlockDetailModel
                {
                    Mandt = row.GetString("MANDT"),
                    DetailId = row.GetString("DETAIL_ID"),
                    BlockId = row.GetString("BLOCK_ID"),
                    SeqNo = row.GetString("SEQ_NO"),
                    LineText = row.GetString("LINE_TEXT"),
                    Erdat = row.GetString("ERDAT"),
                    Aedat = row.GetString("AEDAT")
                });
            }

            // View veya JSON döndür
            var vm = new BlockVm
            {
                Headers = headers,
                Details = details
            };

            return Json(vm, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            return new HttpStatusCodeResult(500, "SAP Hatası: " + ex.Message);
        }
    }
}
