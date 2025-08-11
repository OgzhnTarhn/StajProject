using System.Collections.Generic;

namespace StajProject.Models
{
    public class BlockVm
    {
        public List<BlockHeaderModel> Headers { get; set; }
        public List<BlockDetailModel> Details { get; set; }
    }
}
