using System.Collections.Generic;

namespace iGMS.Controllers
{
    public class Root
    {
        public string reader_name { get; set; }
        public string mac_address { get; set; }
        public List<TagRead> tag_reads { get; set; }
    }

    public class TagRead
    {
        public string epc { get; set; }
    }
}