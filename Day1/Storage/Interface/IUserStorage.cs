﻿using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Interface
{
    public interface IUserStorage : IStorage<User>
    {
    }
}
