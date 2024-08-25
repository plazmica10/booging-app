using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Command
{
    public class FunctionCommand : CommandBase
    {
        public delegate void Function();
        private readonly Function _function;

        public FunctionCommand(Function function) { _function = function; }
        public override void Execute(object? parameter)
        {
            _function();
        }
    }
}
