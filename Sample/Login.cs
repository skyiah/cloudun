﻿using Greatbone.Core;
using static Greatbone.Core.XUtility;

namespace Greatbone.Sample
{

    /// <summary>
    /// A login internal user.
    /// </summary>
    public class Login : IPrincipal, IPersist
    {
        internal string id;
        internal string name;
        internal string credential;
        internal string[] roles;

        public string Key => id;

        public string Name => name;

        public string Credential => credential;

        public void Load(ISource s, byte x = 0xff)
        {
            s.Got(nameof(id), ref id);
            s.Got(nameof(name), ref name);
            s.Got(nameof(credential), ref credential);
            s.Got(nameof(roles), ref roles);
        }

        public void Dump<R>(ISink<R> s, byte x = 0xff) where R : ISink<R>
        {
            s.Put(nameof(id), id);
            s.Put(nameof(name), name);
            if (x.On(RESV))
                s.Put(nameof(credential), credential);
            s.Put(nameof(roles), roles);
        }

    }

}