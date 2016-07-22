using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    [Serializable]
    public class BllVisaRecord
    {
        public string Country { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
