﻿namespace Greatbone.Core
{
    public class WebTie : ISetting
    {
        internal string key;

        public string Key => key;

        public bool Authenticate { get; internal set; }

        public bool IsVar { get; internal set; }

        public WebSub Parent { get; internal set; }

        public WebService Service { get; internal set; }
    }
}