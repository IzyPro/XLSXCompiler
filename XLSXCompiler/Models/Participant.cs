﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
    }
}
