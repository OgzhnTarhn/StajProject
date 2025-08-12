using System.ComponentModel.DataAnnotations;

namespace StajProject.Models
{
    public class EditBlockVm
    {
        [Required] public string BlockId { get; set; }
        [Required, StringLength(70)]
        public string Title { get; set; }
        [Required]
        public string IlKodu { get; set; }  // SAP'deki IL_KODU alanı ile uyumlu
        public string DetailLines { get; set; } // replace modunda kullanılacak
        public bool ReplaceDetails { get; set; }
    }
}