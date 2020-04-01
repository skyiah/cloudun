﻿using System;
using System.Text;
using System.Threading.Tasks;

namespace CloudUn.Web
{
    /// <summary>
    /// To determine principal identity based on current web context. The interaction with user, however, is not included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public abstract class AuthenticateAttribute : Attribute
    {
        readonly bool async;

        protected AuthenticateAttribute(bool async)
        {
            this.async = async;
        }

        public bool IsAsync => async;

        /// <summary>
        /// The synchronous version of authentication check.
        /// </summary>
        /// <remarks>The method only tries to establish principal identity within current web context, not responsible for any related user interaction.</remarks>
        /// <param name="wc">current web request/response context</param>
        /// <returns>true to indicate the web context should continue with processing; false otherwise</returns>
        public virtual bool Do(WebContext wc) => throw new NotImplementedException();

        /// <summary>
        /// The asynchronous version of authentication check.
        /// </summary>
        /// <remarks>The method only tries to establish principal identity within current web context, not responsible for any related user interaction.</remarks>
        /// <param name="wc">current web request/response context</param>
        /// <returns>true to indicate the web context should continue with processing; false otherwise</returns>
        public virtual Task<bool> DoAsync(WebContext wc) => throw new NotImplementedException();


        public static string Encrypt<P>(P prin, byte proj) where P : IData
        {
            var cnt = new JsonContent(4096);
            try
            {
                cnt.PutToken(prin, proj); // use the special putting method to append time stamp

                var bytebuf = cnt.Buffer;
                int count = cnt.Count;

                CryptionUtility.Encrypt(bytebuf, count, Framework.privatekey);
                string cs = TextUtility.BytesToHex(bytebuf, count);
                return cs;
            }
            finally
            {
                BufferUtility.Return(cnt.Buffer);
            }
        }

        public static P Decrypt<P>(string token) where P : IData, new()
        {
            var bytes = TextUtility.HexToBytes(token);
            CryptionUtility.Decrypt(bytes, bytes.Length, Framework.privatekey);
            string str = Encoding.UTF8.GetString(bytes);
            // deserialize
            try
            {
                var jo = (JObj) new JsonParser(str).Parse();
                // check time expiry
                DateTime stamp = jo["$"];
                if ((DateTime.Now - stamp).Hours > 2)
                {
                    return default;
                }
                // construct a principal object
                P prin = new P();
                prin.Read(jo, 0xff);
                return prin;
            }
            catch
            {
                return default;
            }
        }
    }
}