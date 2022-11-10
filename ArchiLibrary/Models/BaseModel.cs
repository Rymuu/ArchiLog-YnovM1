﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiLibrary.Models
{
    public abstract class BaseModel
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; } = true;
        public DateTime? DeletedAt { get; set; }
        [StringLength(50)]
        [Required()]
        public string? Name { get; set; }
    }
}
