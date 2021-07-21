using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleScraping.Models
{
    public class ResponseViewModel
    {
        public ResponseViewModel()
        {
            Errors = new List<Error>();
            Data = new object();
            Message = string.Empty;
        }

        public ResponseViewModel(List<Error> errors, object data, string message)
        {
            Errors = errors;
            Data = data;
            Message = message;
        }

        public List<Error> Errors { get; set; }
        public Object Data { get; set; }
        public string Message { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public Error(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public Error()
        {
            this.Code = string.Empty;
            this.Message = string.Empty;
        }
    }
}
