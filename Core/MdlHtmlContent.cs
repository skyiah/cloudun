using System;

namespace Greatbone.Core
{

    ///
    /// <summary>
    /// A dynamical HTML content that encapsulates Material-Design-Lite (MDL) components
    /// </summary> 
    ///
    public abstract class MdlHtmlContent : HtmlContent<MdlHtmlContent>, ISink<MdlHtmlContent>
    {
        public MdlHtmlContent(int capacity) : base(capacity)
        {
        }

        public void Tabs(string id = "tab", params Tab[] specs)
        {
            T("<div class=\"mdl-tabs mdl-js-tabs\">");

            // tab bar
            T("<div class=\"mdl-tabs__tab-bar\">");
            for (int i = 0; i < specs.Length; i++)
            {
                Tab tab = specs[i];
                T("<a href=\"#").T(id).T(SEX[i]).T("\" class=\"mdl-tabs__tab\">Tab One</a>");
            }
            T("</div>");

            for (int i = 0; i < specs.Length; i++)
            {
                Tab tab = specs[i];
                T("<div class=\"mdl-tabs__panel");
                if (i == 0) T(" is-active");
                T("\" id=\"").T(id).T(SEX[i]).T("\">");

                tab.Panel(this);
                T("</div>");
            }

            T("</div>");
        }

        public void Footer(string h, Action content)
        {
            T("<dialog class=\"mdl-dialog\">");
            T("<h4 class=\"mdl-dialog__title\">").T(h).T("</h4>");

            T("<div class=\"mdl-dialog__content\">");

            T("<div class=\"mdl-dialog__actions\">");
            T("<button type=\"button\" class=\"mdl-button\">Agree</button>");
            T("<button type=\"button\" class=\"mdl-button close\">Disagree</button>");
            T("</div>");

            T("</div>");
            T("</dialog>");

            T("<script>");


            T("</script>");
        }

        public void Button(string label)
        {
            T("<button class=\"mdl-button mdl-js-button mdl-button--raised mdl-button--colored\">");
            T(label);
            T("</button>");
        }

        public void Buttons(WebInterface wi)
        {
            for (int i = 0; i < wi.Count; i++)
            {
                WebAction wa = wi[i];
                ButtonAttribute btn = wa.Button;
                if (btn == null) continue;
                T("<button class=\"mdl-button mdl-js-button mdl-button--raised mdl-button--colored\">");
                T(wa.Key);
                T("</button>");
            }
        }

        public void Dialog(string h, Action content)
        {
            T("<dialog class=\"mdl-dialog\">");
            T("<h4 class=\"mdl-dialog__title\">").T(h).T("</h4>");

            T("<div class=\"mdl-dialog__content\">");

            T("<div class=\"mdl-dialog__actions\">");
            T("<button type=\"button\" class=\"mdl-button\">Agree</button>");
            T("<button type=\"button\" class=\"mdl-button close\">Disagree</button>");
            T("</div>");

            T("</div>");
            T("</dialog>");

            T("<script>");


            T("</script>");
        }


        public void Table<M>(M[] arr, uint x = 0) where M : IPersist
        {
            T("<table class=\"mdl-data-table mdl-js-data-table mdl-shadow--2dp\">");
            T("<thead>");

            M obj = arr[0];


            T("<th class=\"mdl-data-table__cell--non-numeric\">Material</th>");
            T("<th>Quantity</th>>");

            T("</thead>");
            T("<tbody>");

            for (int i = 0; i < arr.Length; i++)
            {
                obj = arr[i];
                T("<tr>");
                T("<td class=\"mdl-data-table__cell--non-numeric\">Acrylic (Transparent)</td>");
                T("<td>25</td>");

                obj.Save(this, x);

                T("</tr>");
            }

            T("</tbody>");

            T("</table>");
        }


        public void InputText()
        {
            T("</tbody>");

        }

        public void InputTextarea()
        {
            T("</tbody>");

        }

        public void Form<P>(Action a) where P : IParent
        {
            T("<form>");

            T("</form>");
        }

        public void Form<P>(P obj, uint x = 0) where P : IParent
        {
            T("<form>");

            T("</form>");
        }

        //
        // ISINK
        //

        public MdlHtmlContent PutNull(string name)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, bool v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, short v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, int v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, long v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, decimal v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, Number v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, DateTime v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, char[] v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, string v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, byte[] v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, ArraySegment<byte> v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put<V>(string name, V v, uint x = 0) where V : IPersist
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, JObj v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, JArr v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, short[] v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, int[] v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, long[] v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put(string name, string[] v)
        {
            throw new NotImplementedException();
        }

        public MdlHtmlContent Put<V>(string name, V[] v, uint x = 0) where V : IPersist
        {
            throw new NotImplementedException();
        }
    }
}