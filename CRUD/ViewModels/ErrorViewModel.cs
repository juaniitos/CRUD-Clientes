﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD.Validations
{
    public class ErrorViewModel
    {
        public string RequestId { get; set;}

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}