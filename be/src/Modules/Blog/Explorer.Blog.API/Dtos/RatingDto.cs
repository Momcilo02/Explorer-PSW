﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class RatingDto
    {
        public bool Grade { get; set; }
        public int UserId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
