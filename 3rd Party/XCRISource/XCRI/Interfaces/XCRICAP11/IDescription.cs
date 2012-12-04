﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCRI.Interfaces.XCRICAP11
{
    /// <summary>
    /// Represents the Description node in the XCRI definition.
    /// http://www.xcri.org/wiki/index.php/Description.
    /// </summary>
    public interface IDescription : IElementWithSingleValue<string>
    {
        Uri Href { get; set; }
        DescriptionContentTypes ContentType { get; set; }

    }
}
