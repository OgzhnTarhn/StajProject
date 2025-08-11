using System.ComponentModel.DataAnnotations;

namespace StajProject.Models
{
    public class EditBlockVm
    {
        [Required] public string BlockId { get; set; }
        [Required, StringLength(70)]
        public string Title { get; set; }
        public string DetailLines { get; set; } // replace modunda kullanılacak
        public bool ReplaceDetails { get; set; }
    }
}