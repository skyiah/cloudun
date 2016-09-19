﻿using System;
using System.Collections.Generic;

namespace Greatbone.Core
{
    /// <summary>
    /// JjavaScript Object Binary
    /// </summary>
    public class JsobContent : DynamicContent, ISerialReader, ISerialWriter
    {
        JsonLR[] stack = new JsonLR[8];

        // current level, start with 0
        int level = -1;

        int pos;


        public JsobContent(ArraySegment<byte> seg) : base(seg)
        {
        }

        public override string Type => "application/json";


        internal void SkipLevel()
        {
        }

        internal bool SeekInKnot(string name)
        {
            // in between quotation marks 
            bool inq;

            int qstart, qend;
            int p = pos;
            for (;;) // find the start quotation
            {
                byte c = buffer[p];
                if (c == '"')
                {
                    inq = true;
                    qstart = p;
                }
                else if (c == '}')
                {
                    stack[level].end = p; // mark down the level end
                }
                p++;
                break;
            }

            for (;;)
            {
                byte c = buffer[p];
                if (c != '"')
                {
                    p++;
                }
                if (c == '}')
                {
                    stack[level].end = p; // mark down the level end
                }
                break;
            }

            SkipWs();

            // seek two quotations and a colon
            while (buffer[pos] != ':')
            {
                pos++;
            }


            return false;
        }


        public bool Read(out bool value)
        {
            throw new NotImplementedException();
        }

        internal bool Read(out decimal value)
        {
            value = 0;
            return false;
        }

        public bool ReadArray(Action a)
        {
            while (buffer[pos] != '[')
            {
                pos++;
            }
            level++;

            a();

            level--;
            while (buffer[pos] != ']')
            {
                pos++;
            }
            return true;
        }

        public bool ReadObject(Action a)
        {
            while (buffer[pos] != '{')
            {
                pos++;
            }
            level++;

            a();

            level--;
            while (buffer[pos] != '}')
            {
                pos++;
            }
            return true;
        }

        public bool Read<T>(out List<T> list)
        {
            List<T> lst = new List<T>();
            ReadArray(() =>
            {
                if (typeof(T) == typeof(int))
                {
                    string value = null;
                    if (Read(out value))
                    {
                    }
                }
            });
            list = null;
            return false;
        }

        public bool Read<T>(out T value) where T : ISerial, new()
        {
            T o = new T();
            ReadArray(() => { o.ReadFrom(this); });
            value = o;
            return true;
        }

        public bool Read(out short value)
        {
            value = 0;
            return false;
        }

        void SkipWs()
        {
            for (;;)
            {
                byte c = buffer[pos];
                if (c == ' ' || c == '\t' || c == '\r' || c == '\n')
                {
                    pos++;
                    continue;
                }
                return;
            }
        }

        char[] seg = new char[1024];
        int segc = 0;

        void Token()
        {
            for (;;)
            {
                byte c = buffer[pos];
                if (c == ' ' || c == '\t')
                {
                    pos++;
                    continue;
                }
                break;
            }
        }

        public bool Read(out int value)
        {
            SkipWs();

            for (;;)
            {
                byte c = buffer[pos];
                if (c != ',' && c == '}')
                {
                }
                break;
            }
            value = 0;
            return false;
        }

        public bool Read(out DateTime value)
        {
            value = default(DateTime);
            return false;
        }

        public bool Read(out string value)
        {
            value = null;
            return false;
        }


        public bool Read(string name, out short value)
        {
            if (SeekInKnot(name))
            {
                return Read(out value);
            }
            value = 0;
            return false;
        }

        public bool Read(string name, out int value)
        {
            if (SeekInKnot(name))
            {
                return Read(out value);
            }
            value = 0;
            return false;
        }

        public bool Read(string name, out decimal value)
        {
            if (SeekInKnot(name))
            {
                return Read(out value);
            }
            value = 0;
            return false;
        }

        public bool Read(string name, out DateTime value)
        {
            throw new NotImplementedException();
        }

        public bool Read(string name, out string value)
        {
            throw new System.NotImplementedException();
        }

        public bool Read<T>(string name, out T value) where T : ISerial, new()
        {
            throw new System.NotImplementedException();
        }

        public bool Read<T>(string name, List<T> value) where T : ISerial, new()
        {
            SeekInKnot(name);

            ReadArray(() =>
            {
                T o = new T();
                while (Read(out o))
                {
                    value.Add(o);
                }
            });
            value = null;
            return false;
        }

        public bool Read(string name, ref List<string> value)
        {
            throw new System.NotImplementedException();
        }

        public bool Read(string name, out bool value)
        {
            throw new System.NotImplementedException();
        }

        public bool Read<K, V>(string name, ref Dictionary<K, V> value)
        {
            throw new System.NotImplementedException();
        }

        public bool Read(string name, ref string[] value)
        {
            throw new System.NotImplementedException();
        }

        public bool Read(string name, out char[] value)
        {
            throw new NotImplementedException();
        }

        public bool Read(out long value)
        {
            throw new NotImplementedException();
        }

        bool ISerialReader.Read(out decimal value)
        {
            throw new NotImplementedException();
        }

        public bool Read(out char[] value)
        {
            throw new NotImplementedException();
        }

        public bool Read<T>(out T[] value)
        {
            throw new NotImplementedException();
        }

        public bool Read<T>(out Dictionary<string, T> value)
        {
            throw new NotImplementedException();
        }

        public bool Read(string name, out long value)
        {
            throw new NotImplementedException();
        }

        public bool Read<T>(string name, out T[] value)
        {
            throw new NotImplementedException();
        }

        public bool Read<T>(string name, out List<T> value)
        {
            throw new NotImplementedException();
        }

        public bool Read<T>(string name, out Dictionary<string, T> value)
        {
            throw new NotImplementedException();
        }


        public JsobContent(int capacity) : base(capacity)
        {
        }


        //
        // WRITES
        //

        public void WriteArray(Action inner)
        {
            level++;
            Put('[');

            stack[level].array = true;

            Put(']');
            level--;
        }

        public void WriteObject(Action inner)
        {
            Put('{');

            level++;
            stack[level].array = false;

            Put('}');

            level--;
        }


        public void Write(string name, short value)
        {
            if (stack[level].ordinal > 0)
            {
                Put(','); // precede a comma
            }

            Put('"');
            Put(name);
            Put('"');
            Put(':');
            Put(value);

            stack[level].ordinal++;
        }

        public void Write(string name, int value)
        {
            if (stack[level].ordinal > 0)
            {
                Put(','); // precede a comma
            }

            Put('"');
            Put(name);
            Put('"');
            Put(':');
            Put(value);

            stack[level].ordinal++;
        }

        public void Write(string name, decimal value)
        {
            if (stack[level].ordinal > 0)
            {
                Put(','); // precede a comma
            }

            Put('"');
            Put(name);
            Put('"');
            Put(':');
            Put(value);

            stack[level].ordinal++;
        }

        public void Write(string name, DateTime value)
        {
            if (stack[level].ordinal > 0)
            {
                Put(','); // precede a comma
            }

            Put('"');
            Put(name);
            Put('"');
            Put(':');
            Put(value);

            stack[level].ordinal++;
        }

        public void Write(string name, string value)
        {
            if (stack[level].ordinal > 0)
            {
                Put(','); // precede a comma
            }

            Put('"');
            Put(name);
            Put('"');
            Put(':');

            if (value == null)
            {
                Put("null");
            }
            else
            {
                Put('"');
                Put(value);
                Put('"');
            }

            stack[level].ordinal++;
        }

        public void Write<T>(string name, T value) where T : ISerial
        {
            if (stack[level].ordinal > 0)
            {
                Put(','); // precede a comma
            }

            Put('"');
            Put(name);
            Put('"');
            Put(':');

            if (EqualityComparer<T>.Default.Equals(value, default(T)))
            {
                Put("null");
            }
            else
            {
                WriteObject(() => { value.WriteTo(this); });
            }

            stack[level].ordinal++;
        }

        public void Write(string name, List<ISerial> list)
        {
            if (stack[level].ordinal > 0)
            {
                Put(','); // precede a comma
            }

            Put('"');
            Put(name);
            Put('"');
            Put(':');

            Put('[');
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (i > 0)
                    {
                        Put(',');
                    }

                    ISerial obj = list[i];

                    Put('{');
                    obj.WriteTo(this);
                    Put('}');
                }
            }
            Put(']');

            stack[level].ordinal++;
        }

        public void Write(string name, bool value)
        {
        }

        public void Write<T>(string name, List<T> list)
        {
        }

        public void Write<V>(string name, Dictionary<string, V> dict)
        {
            Put('"');
            Put(name);
            Put('"');
            Put(':');

            Put('{');
            foreach (var pair in dict)
            {
                Put('"');
                Put(pair.Key);
                Put('"');
                Put(':');

                //				PutValue(pair.Value);
            }

            Put('}');
        }

        public void Write(string name, params string[] array)
        {
            throw new System.NotImplementedException();
        }

        public void Write(string name, params ISerial[] array)
        {
            Put('"');
            Put(name);
            Put('"');
            Put(':');

            Put('[');
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                    {
                        Put(',');
                    }

                    ISerial obj = array[i];

                    Put('{');
                    obj.WriteTo(this);
                    Put('}');
                }
            }
            Put(']');
        }

        public bool Read(string name, char[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(string name, long value)
        {
            throw new NotImplementedException();
        }

        public void Write(string name, char[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(bool value)
        {
            throw new NotImplementedException();
        }

        public void Write(short value)
        {
            throw new NotImplementedException();
        }

        public void Write(int value)
        {
            throw new NotImplementedException();
        }

        public void Write(long value)
        {
            throw new NotImplementedException();
        }

        public void Write(decimal value)
        {
            throw new NotImplementedException();
        }

        public void Write(DateTime value)
        {
            throw new NotImplementedException();
        }

        public void Write(char[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(string value)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(T value) where T : ISerial
        {
            throw new NotImplementedException();
        }

        public void Write<T>(T[] value)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(List<T> value)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(Dictionary<string, T> value)
        {
            throw new NotImplementedException();
        }

        public void Write<T>(string name, T[] value)
        {
            throw new NotImplementedException();
        }
    }
}