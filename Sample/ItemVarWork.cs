using Greatbone.Core;

namespace Greatbone.Sample
{
    public abstract class ItemVarWork : Work
    {
        public ItemVarWork(WorkContext wc) : base(wc)
        {
        }

        public void my(ActionContext ac)
        {
        }

        public void @default(ActionContext ac)
        {
            string shopid = ac[0];
            int id = ac[this];

            using (var dc = Service.NewDbContext())
            {
                dc.Sql("SELECT ").columnlst(Order.Empty)._("FROM orders WHERE id = @1 AND shopid = @2");
                if (dc.Query(p => p.Set(id).Set(shopid)))
                {
                    var order = dc.ToArray<Order>();
                }
                else
                {

                }
            }
        }

        public void _icon_(ActionContext ac)
        {
            string shopid = ac[typeof(ShopVarWork)];
            string name = ac[this];

            using (var dc = Service.NewDbContext())
            {
                if (dc.Query1("SELECT icon FROM items WHERE shopid = @1 AND name = @2", p => p.Set(shopid).Set(name)))
                {
                    var byteas = dc.GetByteAs();
                    if (byteas.Count == 0) ac.Give(204); // no content 
                    else
                    {
                        StaticContent cont = new StaticContent(byteas);
                        ac.Give(200, cont);
                    }
                }
                else ac.Give(404); // not found           
            }
        }

        public void cannel(ActionContext ac)
        {
            string shopid = ac[0];
            int orderid = ac[this];

            using (var dc = Service.NewDbContext())
            {
                dc.Sql("SELECT ").columnlst(Order.Empty)._("FROM orders WHERE id = @1 AND shopid = @2");
                if (dc.Query(p => p.Set(orderid).Set(shopid)))
                {
                    var order = dc.ToArray<Order>();
                }
                else
                {

                }
            }
        }
    }

    public class PubItemVarWork : ItemVarWork
    {
        public PubItemVarWork(WorkContext wc) : base(wc) { }

        public void add(ActionContext ac)
        {
            string shopid = ac[typeof(ShopVarWork)];
            string name = ac[this];
            if (ac.GET)
            {
                using (var dc = Service.NewDbContext())
                {
                    if (dc.Query1("SELECT price, min, step FROM items WHERE shopid = @1 AND name = @2", p => p.Set(shopid).Set(name)))
                    {
                        var price = dc.GetDecimal();
                        var min = dc.GetShort();
                        short qty = min;
                        var step = dc.GetShort();
                        string note = null;
                        ac.GiveFormPane(200, f =>
                        {
                            f.NUMBER(nameof(qty), qty, label: "����", min: min, step: step);
                            f.TEXTAREA(nameof(note), note, label: "����˵��");
                        });
                    }
                    else ac.Give(404); // not found           
                }
            }
            else
            {

            }
        }
    }

    public class MgrItemVarWork : ItemVarWork
    {
        public MgrItemVarWork(WorkContext wc) : base(wc)
        {
        }
    }
}