using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Command
{
    public class FunctionArgCommand : CommandBase
    {
        private readonly Action<object?> _action;

        public FunctionArgCommand(Action<object?> action) { _action = action; }
        public override void Execute(object? parameter)
        {
            _action(parameter);
        }
    }
}
