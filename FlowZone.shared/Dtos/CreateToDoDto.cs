using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.shared.Dtos
{
    public record CreateToDoDto(string Title, string Description,DateTime DueDate, string Priority);
}
