﻿using System;
using Greatbone.Core;

namespace Greatbone.Sample
{
    /// <summary>Represent a chat session.</summary>
    ///
    public struct Chat : IPersist
    {
        private int status;
        public string partner;
        private Message[] msgs;
        private long lasttime;
        WebContext wctx;

        internal void Put(string msg)
        {
            // msgs.Add(new Message());
        }

        public void Load(ISource s, byte x = 0xff)
        {
            s.Get(nameof(status), ref status);
            s.Get(nameof(partner), ref partner);
            s.Get(nameof(msgs), ref msgs);
            s.Get(nameof(lasttime), ref lasttime);
        }

        public void Dump<R>(ISink<R> s, byte x = 0xff) where R : ISink<R>
        {
            s.Put(nameof(status), status);
            s.Put(nameof(partner), partner);
            s.Put(nameof(msgs), msgs);
            s.Put(nameof(lasttime), lasttime);
        }
        
    }

    struct Message : IPersist
    {
        DateTime time;

        string text;

        public void Load(ISource sc, byte x = 0xff)
        {
            throw new NotImplementedException();
        }

        public void Dump<R>(ISink<R> sk, byte x = 0xff) where R : ISink<R>
        {
            throw new NotImplementedException();
        }

    }

    public struct Msg : IPersist
    {
        internal int id;

        internal short subtype;

        internal string from;

        internal string to;

        internal string content;

        internal DateTime time;

        public void Load(ISource sc, byte x = 0xff)
        {
            sc.Get(nameof(id), ref id);
            sc.Get(nameof(subtype), ref subtype);
            sc.Get(nameof(@from), ref @from);
            sc.Get(nameof(to), ref to);
            sc.Get(nameof(content), ref content);
            sc.Get(nameof(time), ref time);
        }

        public void Dump<R>(ISink<R> sk, byte x = 0xff) where R : ISink<R>
        {
            sk.Put(nameof(id), id);
            sk.Put(nameof(subtype), subtype);
            sk.Put(nameof(from), from);
            sk.Put(nameof(to), to);
            sk.Put(nameof(content), content);
            sk.Put(nameof(time), time);
        }

    }

}