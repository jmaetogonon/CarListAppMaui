using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListApp.Models
{
    public class CarMart
    {
        public int Id { get; set; }
        public List<Car> Cars { get; set; } = [];
    }
}
