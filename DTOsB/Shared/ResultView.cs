using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DTOsB.Shared
{
    public class ResultView<T>
    {
        public T Entity { get; set; }

        public bool IsSuccess { get; set; }

        public string Msg { get; set; }

        public ResultView()
        {
        }

        private ResultView(T data, bool success, string error)
        {
            Entity = data;
            IsSuccess = success;
            Msg = error;
        }

        public static ResultView<T> Success(T data) => new ResultView<T>(data, true, null);
        public static ResultView<T> Failure(string error) => new ResultView<T>(default, false, error);
    }
}
