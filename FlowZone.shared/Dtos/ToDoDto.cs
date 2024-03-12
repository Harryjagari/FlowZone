using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.shared.Dtos
{
	public record ToDoDto(Guid ToDoId, String Title, string Description, DateTime Created, DateTime LastUpdated, DateTime DueDate, string Priority);
}
