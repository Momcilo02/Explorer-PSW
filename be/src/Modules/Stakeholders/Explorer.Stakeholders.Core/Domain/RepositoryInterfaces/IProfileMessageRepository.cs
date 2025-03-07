﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IProfileMessageRepository 
    {
        public ICollection<ProfileMessage> GetAll();
        void Create(ProfileMessage message);
    }
}
