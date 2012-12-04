﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCRI.Interfaces.XCRICAP12
{
    public interface IProvider : ICommonElements
    {

        IList<Interfaces.ICourse> Courses { get; }
        IList<Interfaces.ILocation> Locations { get; }

    }
}
