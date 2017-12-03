﻿using Greatbone.Core;

namespace Greatbone.Sample
{
    public class Shop : IData
    {
        public static readonly Shop Empty = new Shop();

        public const short ID = 1, LATER = 2;

        public static readonly Map<short, string> Statuses = new Map<short, string>
        {
            {0, null},
            {1, "休息中"},
            {2, "营业中"}
        };

        internal string id;
        internal string name;
        internal string city;
        internal string addr;
        internal string schedule;
        internal Area[] areas;
        internal decimal min;
        internal decimal notch;
        internal decimal off;
        internal string mgrwx;
        internal string mgrtel;
        internal string mgrname;
        internal string oprwx;
        internal string oprtel;
        internal string oprname;

        internal Supply[] supplies;

        internal short status;

        public void Read(IDataInput i, short proj = 0x00ff)
        {
            if ((proj & ID) == ID)
            {
                i.Get(nameof(id), ref id);
            }
            i.Get(nameof(name), ref name);
            i.Get(nameof(city), ref city);
            i.Get(nameof(addr), ref addr);
            i.Get(nameof(schedule), ref schedule);
            i.Get(nameof(areas), ref areas);
            i.Get(nameof(min), ref min);
            i.Get(nameof(notch), ref notch);
            i.Get(nameof(off), ref off);
            i.Get(nameof(supplies), ref supplies);
            if ((proj & LATER) == LATER)
            {
                i.Get(nameof(mgrwx), ref mgrwx);
                i.Get(nameof(mgrtel), ref mgrtel);
                i.Get(nameof(mgrname), ref mgrname);
                i.Get(nameof(oprwx), ref oprwx);
                i.Get(nameof(oprtel), ref oprtel);
                i.Get(nameof(oprname), ref oprname);
                i.Get(nameof(status), ref status);
            }
        }

        public void Write<R>(IDataOutput<R> o, short proj = 0x00ff) where R : IDataOutput<R>
        {
            if ((proj & ID) == ID)
            {
                o.Put(nameof(id), id);
            }
            o.Put(nameof(name), name);
            o.Put(nameof(city), city);
            o.Put(nameof(addr), addr);
            o.Put(nameof(schedule), schedule);
            o.Put(nameof(areas), areas);
            o.Put(nameof(min), min);
            o.Put(nameof(notch), notch);
            o.Put(nameof(off), off);
            o.Put(nameof(supplies), supplies);
            if ((proj & LATER) == LATER)
            {
                o.Put(nameof(mgrwx), mgrwx);
                o.Put(nameof(mgrtel), mgrtel);
                o.Put(nameof(mgrname), mgrname);
                o.Put(nameof(oprwx), oprwx);
                o.Put(nameof(oprtel), oprtel);
                o.Put(nameof(oprname), oprname);
                o.Put(nameof(status), status);
            }
        }
    }

    public struct Area : IData
    {
        internal string id;

        internal double x1, y1, x2, y2;

        internal string[] spots;

        internal string wx;
        internal string name;
        internal string tel;

        public void Read(IDataInput i, short proj = 0x00ff)
        {
            i.Get(nameof(id), ref id);
            i.Get(nameof(x1), ref x1);
            i.Get(nameof(y1), ref y1);
            i.Get(nameof(x2), ref x2);
            i.Get(nameof(y2), ref y2);
            i.Get(nameof(spots), ref spots);
        }

        public void Write<R>(IDataOutput<R> o, short proj = 0x00ff) where R : IDataOutput<R>
        {
            o.Put(nameof(id), id);
            o.Put(nameof(x1), x1);
            o.Put(nameof(y1), y1);
            o.Put(nameof(x2), x2);
            o.Put(nameof(y2), y2);
            o.Put(nameof(spots), spots);
        }

        public override string ToString()
        {
            return id;
        }
    }

    public struct Supply : IData
    {
        internal string name;

        internal short qty;

        internal string unit;

        public void Read(IDataInput i, short proj = 0x00ff)
        {
            i.Get(nameof(name), ref name);
            i.Get(nameof(qty), ref qty);
            i.Get(nameof(unit), ref unit);
        }

        public void Write<R>(IDataOutput<R> o, short proj = 0x00ff) where R : IDataOutput<R>
        {
            o.Put(nameof(name), name);
            o.Put(nameof(qty), qty);
            o.Put(nameof(unit), unit);
        }

        public override string ToString()
        {
            return name;
        }
    }
}