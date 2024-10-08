﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.shared.Dtos
{
    public record ToDoDto(Guid ToDoId, string Title, string Description, DateTime Created, DateTime DueDate, string Priority)
    {
        public static implicit operator ToDoDto(ObservableCollection<ToDoDto> v)
        {
            throw new NotImplementedException();
        }
    }
}
