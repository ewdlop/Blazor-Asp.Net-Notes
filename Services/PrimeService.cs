﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public class PrimeService : IPrimeService
    {
        public bool IsPrime(int candidate) {
            if (candidate < 2) {
                return false;
            }
            throw new NotImplementedException("Not fully implemented.");
        }
    }
}
