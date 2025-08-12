// Models/CreateBlockVm.cs
using System.ComponentModel.DataAnnotations;

namespace StajProject.Models
{
    public class CreateBlockVm
    {
        [Required, StringLength(70)]
        public string Title { get; set; }

        [Required]
        public string IlKodu { get; set; }  // SAP'deki IL_KODU alanı ile uyumlu

        // Her satır bir detail
        public string DetailLines { get; set; }
    }
}
