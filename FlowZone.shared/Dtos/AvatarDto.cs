using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.shared.Dtos
{
    public record AvatarDto
    {
        public byte[] ImageData { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

}
