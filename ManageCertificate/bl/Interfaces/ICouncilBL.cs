﻿using Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface ICouncilBL
    {
        Task<IEnumerable<RefCouncil>> GetAllcouncil();

    }
}
