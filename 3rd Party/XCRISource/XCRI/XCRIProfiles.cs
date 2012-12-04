﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCRI
{
	/// <summary>
	/// Enumerates the available XCRI profiles
	/// </summary>
	public enum XCRIProfiles
	{

        None = 0,
		/// <summary>
		/// The XCRI-CAP 1.1 profile
		/// </summary>
		XCRI_v1_1 = 1,
        /// <summary>
        /// The XCRI-CAP 1.2 profile
        /// </summary>
        XCRI_v1_2 = 2,
        /// <summary>
        ///  All available profiles
        /// </summary>
        All = XCRI_v1_1 | XCRI_v1_2
	}
}
