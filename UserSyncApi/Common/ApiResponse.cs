﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserSyncApi.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }  
        public string Status { get; set; }
        public string Message { get; set; }  
        public T Data { get; set; }          
        public Guid CorrelationId { get; set; }
        public string Error { get; set; }
    }                                                                            
}