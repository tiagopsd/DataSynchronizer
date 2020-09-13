using System;
using System.Collections.Generic;
using System.Text;

namespace DataSynchronizer.Domain.Models
{
    public class Result
    {
        public bool Sucesso { get; set; }
        public string Message { get; set; }

        public static Result BuildError(string message, Exception error = null)
            => new Result 
            { 
                Sucesso = false, Message = message + 
                ((error != null) ? " " +
                $"Error: {error.GetBaseException().Message} " +
                $"StackTrace: {error.GetBaseException().StackTrace}" 
                : "") 
            };

        public static Result BuildSucess(string message = "")
            => new Result { Sucesso = true, Message = message };
    }
}
