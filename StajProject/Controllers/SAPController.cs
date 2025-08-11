using SAP.Middleware.Connector;
using StajProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace StajProject.Controllers
{
    public class SapController : Controller
    {
        // SAP bağlantısını döndüren yardımcı metot
        private RfcDestination GetDestination()
        {
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

            return RfcDestinationManager.GetDestination(parms);
        }

        // Kullanıcı listesi (ZUSR_GET_USER)
        public ActionResult GetUsers()
        {
            var users = new List<SAPUserModel>();

            try
            {
                var dest = GetDestination();
                var repo = dest.Repository;

                IRfcFunction func = repo.CreateFunction("ZUSR_GET_USER");
                // Eğer input parametre varsa buradan set edebilirsin:
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
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "SAP Hatası: " + ex.Message);
            }
        }

        // Block verileri (ZBLOCK_GET)
        public BlockVm GetBlocks(string blockId = null)
        {
            var headers = new List<BlockHeaderModel>();
            var details = new List<BlockDetailModel>();

            try
            {
                var dest = GetDestination();
                var repo = dest.Repository;

                IRfcFunction func = repo.CreateFunction("ZBLOCK_GET");

                if (!string.IsNullOrWhiteSpace(blockId))
                    func.SetValue("IV_BLOCK_ID", blockId);

                func.Invoke(dest);

                // HEADER tablosu
                IRfcTable etHdr = func.GetTable("ET_HDR");
                System.Diagnostics.Debug.WriteLine($"SAP Header Table Count: {etHdr.Count}");
                
                foreach (IRfcStructure row in etHdr)
                {
                    var erdat = row.GetString("ERDAT");
                    var aedat = row.GetString("AEDAT");
                    
                    // Debug: Log the actual values received from SAP
                    System.Diagnostics.Debug.WriteLine($"SAP Header - ERDAT: '{erdat}', AEDAT: '{aedat}'");
                    
                    headers.Add(new BlockHeaderModel
                    {
                        Mandt = row.GetString("MANDT"),
                        BlockId = row.GetString("BLOCK_ID"),
                        Title = row.GetString("TITLE"),
                        Erdat = erdat,
                        Aedat = aedat
                    });
                }

                // DETAIL tablosu
                IRfcTable etDtl = func.GetTable("ET_DTL");
                System.Diagnostics.Debug.WriteLine($"SAP Detail Table Count: {etDtl.Count}");
                
                if (etDtl.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("WARNING: ET_DTL table is empty! Checking available tables...");
                    
                    // Check what tables are available in the function
                    var availableTables = func.GetTableNames();
                    System.Diagnostics.Debug.WriteLine($"Available tables: {string.Join(", ", availableTables)}");
                    
                    // Check if there are any other detail-related tables
                    foreach (var tableName in availableTables)
                    {
                        var table = func.GetTable(tableName);
                        System.Diagnostics.Debug.WriteLine($"Table {tableName}: {table.Count} rows");
                    }
                }
                
                foreach (IRfcStructure row in etDtl)
                {
                    var seqNo = row.GetString("SEQ_NO");
                    var lineText = row.GetString("LINE_TEXT");
                    
                    System.Diagnostics.Debug.WriteLine($"SAP Detail - SEQ_NO: '{seqNo}', LINE_TEXT: '{lineText}'");
                    
                    details.Add(new BlockDetailModel
                    {
                        Mandt = row.GetString("MANDT"),
                        DetailId = row.GetString("DETAIL_ID"),
                        BlockId = row.GetString("BLOCK_ID"),
                        SeqNo = seqNo,
                        LineText = lineText,
                        Erdat = row.GetString("ERDAT"),
                        Aedat = row.GetString("AEDAT")
                    });
                }

                return new BlockVm
                {
                    Headers = headers,
                    Details = details
                };
            }
            catch (Exception ex)
            {
                throw new Exception("SAP Hatası: " + ex.Message);
            }
        }
    }
}
