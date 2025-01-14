﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class GroupModel
    {
        public Guid id {  get; set; }
        public string name { get; set; } = null!;
        public string leaderName { get; set; } = null!;
        public int amountWorker { get; set; }
    }

    public class CreateGroupModel
    {
        public string name { get; set; } = null!;
        public Guid leaderId { get; set; } 
    }

    public class UpdateGroupModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
        public Guid leaderId { get; set; }
    }

    public class DeleteGroupModel
    {
        public Guid id { get; set; }
    }
}
