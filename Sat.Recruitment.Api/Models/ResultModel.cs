using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Models
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public string Errors { get; set; }

        public ResultModel(bool IsSuccess, string Errors)
        {
            this.IsSuccess = IsSuccess;
            this.Errors = Errors;
        }
    }
}
