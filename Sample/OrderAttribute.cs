using Greatbone.Core;

namespace Greatbone.Samp
{
    public class OrderAttribute : StateAttribute
    {
        readonly char state;

        public OrderAttribute(char state)
        {
            this.state = state;
        }

        public override bool Check(ActionContext ac, object obj)
        {
            var o = obj as Order;
            if (state == 'A')
                return o.addr != null;
            else if (state == 'P') // payable
                return o.total >= o.min;
            return false;
        }
    }
}