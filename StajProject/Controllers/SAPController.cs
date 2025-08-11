using SAP.Middleware.Connector;
using StajProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web;

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
        public List<SAPUserModel> GetUsers()
        {
            try
            {
                var users = new List<SAPUserModel>();

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

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("SAP Hatası: " + ex.Message);
            }
        }

        // Block verileri (ZBLOCK_GET)
        public BlockVm GetBlocks(string blockId = null)
        {
            try
            {
                var headers = new List<BlockHeaderModel>();
                var details = new List<BlockDetailModel>();

                var dest = GetDestination();
                var repo = dest.Repository;

                IRfcFunction func = repo.CreateFunction("ZBLOCK_GET");

                if (!string.IsNullOrWhiteSpace(blockId))
                    func.SetValue("IV_BLOCK_ID", blockId);

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

        // Block oluşturma metodu (ZBLOCK_INSERT)
        public string InsertBlock(string title, IList<string> detailLines = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    throw new ArgumentException("Başlık boş olamaz");
                }

                var dest = GetDestination();
                var repo = dest.Repository;
                IRfcFunction func = repo.CreateFunction("ZBLOCK_INSERT");

                // importing
                func.SetValue("IV_TITLE", title.Trim());

                // tables (sadece LINE_TEXT dolduracağız; SEQ_NO istemezsen boş bırak)
                IRfcTable itDtl = func.GetTable("IT_DTL");
                if (detailLines != null && detailLines.Count > 0)
                {
                    int seq = 1;
                    foreach (var line in detailLines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        itDtl.Append();
                        itDtl.SetValue("LINE_TEXT", line.Trim());
                        itDtl.SetValue("SEQ_NO", seq++);
                    }
                }

                func.Invoke(dest);

                // exporting
                string newId = func.GetString("EV_BLOCK_ID");
                
                if (string.IsNullOrWhiteSpace(newId))
                {
                    throw new Exception("Block ID döndürülemedi");
                }

                return newId;
            }
            catch (Exception ex)
            {
                throw new Exception("Block oluşturulurken hata: " + ex.Message);
            }
        }
        public bool UpdateBlock(string blockId, string title, bool replaceDetails, IList<string> detailLines = null)
        {
            var dest = GetDestination();
            var repo = dest.Repository;
            var func = repo.CreateFunction("ZBLOCK_UPDATE");

            func.SetValue("IV_BLOCK_ID", blockId ?? "");
            if (!string.IsNullOrWhiteSpace(title))
                func.SetValue("IV_TITLE", title);

            func.SetValue("IV_REPLACE_DTL", replaceDetails ? "X" : "");

            IRfcTable itDtl = func.GetTable("IT_DTL");
            if (replaceDetails && detailLines != null)
            {
                foreach (var line in detailLines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    itDtl.Append();
                    itDtl.SetValue("LINE_TEXT", line.Trim());
                }
            }

            func.Invoke(dest);
            return func.GetString("EV_UPDATED") == "X";
        }
        public bool DeleteBlock(string blockId)
        {
            var dest = GetDestination();
            var repo = dest.Repository;
            var func = repo.CreateFunction("ZBLOCK_DELETE");

            func.SetValue("IV_BLOCK_ID", blockId ?? "");
            func.Invoke(dest);

            return func.GetString("EV_DELETED") == "X";
        }

    }
}
