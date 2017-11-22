using System;
using System.Threading.Tasks;
using Greatbone.Core;
using static Greatbone.Core.UiMode;
using static Greatbone.Sample.Order;

namespace Greatbone.Sample
{
    public class OrderlyAttribute : StateAttribute
    {
        readonly char state;

        public OrderlyAttribute(char state)
        {
            this.state = state;
        }

        public override bool Check(object obj)
        {
            var o = obj as Order;
            switch (state)
            {
                case 'A': return o.addr != null;
            }
            return false;
        }
    }

    public abstract class OrderVarWork : Work
    {
        protected OrderVarWork(WorkContext wc) : base(wc)
        {
        }
    }

    public class MyCartVarWork : OrderVarWork
    {
        public MyCartVarWork(WorkContext wc) : base(wc)
        {
        }

        public async Task addr(ActionContext ac)
        {
            string wx = ac[-2];
            int orderid = ac[this];

            string area = null;
            string place = null;
            string addr = null;
            if (ac.GET)
            {
                ac.GivePane(200, h =>
                {
                    using (var dc = Service.NewDbContext())
                    {
                        if (dc.Query1("SELECT city, areas FROM shops WHERE id = (SELECT shopid FROM orders WHERE id = @1)", p => p.Set(orderid)))
                        {
                            dc.Let(out string city).Let(out string[] areas);
                            h.FORM_();
                            h.SELECT(nameof(area), areas[0], areas, "区域", refresh: true);
                            var places = City.All[city].FindArea(areas[0]).places;
                            h.FIELD_("地址").SELECT(nameof(addr), places[0], places).TEXT(nameof(addr), addr)._FIELD();
                            h._FORM();
                        }
                    }
                });
                return;
            }

            var f = await ac.ReadAsync<Form>();
            f.Let(out area).Let(out addr).Let(out string tel);
            using (var dc = ac.NewDbContext())
            {
                dc.Execute("UPDATE orders SET area = @1, addr = @2, tel = @3 WHERE id = @4", p => p.Set(area).Set(addr).Set(tel).Set(orderid));

                User prin = (User) ac.Principal;
                if (prin.city == null)
                {
                    dc.Execute("INSERT INTO users (wx, name, city, area, addr, tel, created) VALUES (@1, @2, @3, @4, @5, @6, @7) ON CONFLICT (wx) DO UPDATE SET name = @2, city = @3, distr = @4, addr = @5, tel = @6, created = @7", p => p.Set(wx).Set(area).Set(addr).Set(tel).Set(DateTime.Now));
                    prin.city = area;
                    prin.addr = addr;
                    prin.tel = tel;
                    // refresh the client cookie
                    ac.SetTokenCookie(prin, -1 ^ User.CREDENTIAL);
                }
            }
            ac.GivePane(200);
        }

        public async Task item(ActionContext ac, int idx)
        {
            string wx = ac[-2];
            int orderid = ac[this];

            if (ac.GET)
            {
                using (var dc = ac.NewDbContext())
                {
                    dc.Sql("SELECT ").columnlst(Empty).T(" FROM orders WHERE id = @1 AND wx = @2");
                    dc.Query1(p => p.Set(orderid).Set(wx));
                    var order = dc.ToObject<Order>();
                    var o = order.items[idx];

                    ac.GivePane(200, h =>
                    {
                        h.FORM_();
                        h.FIELDSET_("数量");
                        h.HIDDEN(nameof(o.unit), o.unit);
                        h.HIDDEN(nameof(o.price), o.price);
                        h.NUMBER(nameof(o.qty), o.qty, min: (short) 0, max: (short) 20, step: (short) 1);
                        h._FIELDSET();
                        if (o.opts != null)
                        {
                            h.CHECKBOXGROUP(nameof(o.opts), null, o.opts, "要求");
                        }
                        h._FORM();
                    });
                }
                return;
            }

            var f = await ac.ReadAsync<Form>();
            short qty = f[nameof(qty)];
            string[] opts = f[nameof(opts)];
            using (var dc = ac.NewDbContext())
            {
                dc.Sql("SELECT ").columnlst(Empty).T(" FROM orders WHERE id = @1 AND wx = @2");
                dc.Query1(p => p.Set(orderid).Set(wx));
                var o = dc.ToObject<Order>();
                o.UpdItem(idx, qty, opts);
                o.SetTotal();
                dc.Execute("UPDATE orders SET rev = rev + 1, items = @1, total = @2 WHERE id = @3", p => p.Set(o.items).Set(o.total).Set(o.id));
            }
            ac.GivePane(200);
        }

        [Ui("付款"), Style(ButtonScript), Orderly('A')]
        public async Task Prepay(ActionContext ac)
        {
            string wx = ac[-2];
            int orderid = ac[this];
            short rev;

            decimal total;
            using (var dc = ac.NewDbContext())
            {
                dc.Query1("SELECT rev, total FROM orders WHERE id = @1 AND wx = @2", p => p.Set(orderid).Set(wx));
                dc.Let(out rev).Let(out total);
            }
            var (prepay_id, _) = await WeiXinUtility.PostUnifiedOrderAsync(orderid + "-" + rev, total, wx, ac.RemoteAddr, "http://144000.tv/paynotify");
            if (prepay_id != null)
            {
                ac.Give(200, WeiXinUtility.BuildPrepayContent(prepay_id));
            }

            else
            {
                ac.Give(500);
            }
        }
    }

    public class MyOrderVarWork : OrderVarWork
    {
        public MyOrderVarWork(WorkContext wc) : base(wc)
        {
        }

        [Ui("建议"), Style(ButtonShow)]
        public async Task kick(ActionContext ac)
        {
            int orderid = ac[this];
            string kick = null;
            if (ac.GET)
            {
                ac.GivePane(200, m =>
                {
                    m.FORM_();
                    m.FIELDSET_("我的建议");
                    m.TEXTAREA(nameof(kick), kick, null, max: 40, required: true);
                    m._FIELDSET();
                    m._FORM();
                });
            }
            else
            {
                var f = await ac.ReadAsync<Form>();
                kick = f[nameof(kick)];
                using (var dc = ac.NewDbContext())
                {
                    dc.Execute("UPDATE orders SET kick = @1 WHERE id = @2", p => p.Set(kick).Set(orderid));
                }
                ac.GivePane(200);
            }
        }
    }


    public class OprNewVarWork : OrderVarWork
    {
        public OprNewVarWork(WorkContext wc) : base(wc)
        {
        }

        [Ui("撤单"), Style(ButtonShow)]
        public async Task abort(ActionContext ac)
        {
            int orderid = ac[this];

            if (ac.GET)
            {
                ac.GivePane(200, m => { m.FORM_().CALLOUT("确定要撤销此单，实收金额退回给买家?")._FORM(); });
            }
            else
            {
                decimal total = 0, cash = 0;
                using (var dc = ac.NewDbContext())
                {
                    if (dc.Query1("SELECT total, cash FROM orders WHERE id = @1", p => p.Set(orderid)))
                    {
                        dc.Let(out total).Let(out cash);
                    }
                }
                string err = await WeiXinUtility.PostRefundAsync(orderid, total, cash);
                if (err == null) // success
                {
                    using (var dc = ac.NewDbContext())
                    {
                        dc.Execute("UPDATE orders SET status = @1 WHERE id = @2", p => p.Set(ABORTED).Set(orderid));
                    }
                    ac.GivePane(200);
                }
                else // err
                {
                    ac.GivePane(200, m => { m.FORM_().CALLOUT(err).CALLOUT("确定重复操作吗？")._FORM(); });
                }
            }
        }
    }

    public class OprGoVarWork : OrderVarWork
    {
        public OprGoVarWork(WorkContext wc) : base(wc)
        {
        }

        [Ui("送完"), Style(ButtonConfirm)]
        public async Task got(ActionContext ac)
        {
            string shopid = ac[-2];
            int orderid = ac[this];
            string wx;
            decimal total;
            using (var dc = ac.NewDbContext())
            {
                dc.Query1("UPDATE orders SET delivered = localtimestamp, status = " + DELIVERED + " WHERE id = @1 AND shopid = @2 AND status = " + PREPARED + " RETURNING wx, total", p => p.Set(orderid).Set(shopid));
                dc.Let(out wx).Let(out total);
            }
            if (wx != null)
            {
                await WeiXinUtility.PostSendAsync(wx, "【通知】您的订单已收货（单号" + orderid + "，金额¥" + total + ")");
            }
            ac.GiveRedirect("../");
        }
    }

    public class OprPastVarWork : OrderVarWork
    {
        public OprPastVarWork(WorkContext wc) : base(wc)
        {
        }

        [Ui("退款核查", "实时核查退款到账情况"), Style(AnchorOpen)]
        public async Task refundq(ActionContext ac)
        {
            int orderid = ac[this];

            string err = await WeiXinUtility.PostRefundQueryAsync(orderid);
            if (err == null) // success
            {
                ac.GivePane(200, m =>
                {
                    m.FORM_();
                    m.CALLOUT("退款成功", false);
                    m._FORM();
                });
            }
            else
            {
                ac.GivePane(200, m =>
                {
                    m.FORM_();
                    m.CALLOUT(err);
                    m.CHECKBOX("ok", false, "重新提交退款请求", required: true);
                    m.BUTTON("", 1, "确认");
                    m._FORM();
                });
            }
        }
    }

    public class AdmKickVarWork : OrderVarWork
    {
        public AdmKickVarWork(WorkContext wc) : base(wc)
        {
        }
    }
}