﻿using Storage.Interface;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class SimpleUserValidator : IUserValidator
    {
        public bool Validate(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            if (user.FirstName == null || user.LastName == null)
                return false;
            return true;
        }
    }
}