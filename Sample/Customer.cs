﻿using System;
using Greatbone.Core;

namespace Greatbone.Sample
{
    /// 
    /// A buyer data object.
    ///
    public class Customer : IData
    {
        internal string shopid;
        internal string id; // wechat id
        internal string name; // wechat name
        internal DateTime time;
        internal string buyerid; // wechat id
        internal string tel;
        internal int status;

        public void Load(ISource s, byte bits = 0)
        {
            s.Get(nameof(id), ref id);
            s.Get(nameof(shopid), ref shopid);
            s.Get(nameof(time), ref time);
            s.Get(nameof(buyerid), ref buyerid);
            s.Get(nameof(name), ref name);
            s.Get(nameof(tel), ref tel);
            s.Get(nameof(status), ref status);
        }

        public void Dump<R>(ISink<R> s, byte bits = 0) where R : ISink<R>
        {
            s.Put(nameof(id), id);
            s.Put(nameof(shopid), shopid);
            s.Put(nameof(time), time);
            s.Put(nameof(buyerid), buyerid);
            s.Put(nameof(tel), tel);
            s.Put(nameof(status), status);
        }
    }
}