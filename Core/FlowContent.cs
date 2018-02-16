﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Greatbone.Core
{
    public class FlowContent : HttpContent, IContent, ISink<FlowContent>
    {
        // NOTE: HttpResponseStream doesn't have internal buffer
        byte[] bytebuf;

        // number of bytes or chars
        int count;

        public FlowContent(int capacity)
        {
            bytebuf = BufferUtility.GetByteBuffer(capacity);
            count = 0;
        }

        public string Type => "application/x-flow";

        public byte[] ByteBuffer => bytebuf;

        public char[] CharBuffer => null;

        public string ETag => null;

        public int Size => count;

        public FlowContent PutOpen()
        {
            throw new NotImplementedException();
        }

        public FlowContent PutClose()
        {
            throw new NotImplementedException();
        }

        public FlowContent PutStart()
        {
            throw new NotImplementedException();
        }

        public FlowContent PutEnd()
        {
            throw new NotImplementedException();
        }

        public FlowContent PutNull(string name)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, JNumber v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, ISource v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, bool v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, short v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, int v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, long v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, double v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, decimal v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, DateTime v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, string v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, ArraySegment<byte> v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, short[] v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, int[] v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, long[] v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, string[] v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, Map<string, string> v)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put(string name, IData v, byte proj = 15)
        {
            throw new NotImplementedException();
        }

        public FlowContent Put<D>(string name, D[] v, byte proj = 15) where D : IData
        {
            throw new NotImplementedException();
        }

        //
        // CLIENT CONTENT
        //
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return stream.WriteAsync(bytebuf, 0, count);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = count;
            return true;
        }

        public ArraySegment<byte> ToByteAs()
        {
            return new ArraySegment<byte>(bytebuf, 0, count);
        }
    }
}