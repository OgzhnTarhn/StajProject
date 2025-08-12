namespace StajProject.Models
{
    public class BlockHeaderModel
    {
        public string Mandt { get; set; }
        public string BlockId { get; set; }
        public string Title { get; set; }
        public string IlKodu { get; set; }  // SAP'deki IL_KODU alanı ile uyumlu
        public string Erdat { get; set; }   // DATS
        public string Aedat { get; set; }   // DATS
    }
}