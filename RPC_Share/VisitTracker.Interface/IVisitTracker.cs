﻿using System;
using System.Threading.Tasks;
using Orleans;

namespace VisitTracker.Interface
{
    public interface IVisitTracker : IGrainWithStringKey
    {
        Task<int> GetNumberOfVisits();
        Task VisitAsync();
    }
}