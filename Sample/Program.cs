﻿using Greatbone.Core;

namespace Greatbone.Sample
{
    /// <summary>
    ///
    /// </summary>
    public class Program
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            JsonContentTest.Test();

            DbConf dat = new DbConf
            {
                host = "60.205.104.239",
                port = 5432,
                username = "postgres",
                password = "Zou###1989"
            };

            var www = new WwwService(new WebServiceConf
            {
                key = "www",
                outer = "127.0.0.1:8080",
                inner = "127.0.0.1:7770",
                foreign = new[] {
                        "localhost:7777"
                    },
                db = dat,
                debug = true
            }.Load("www.json")
            );

            var fame = new FameService(new WebServiceConf
            {
                key = "fame",
                outer = "127.0.0.1:8081",
                inner = "127.0.0.1:7771",
                foreign = new[]
                    {
                        "localhost:7777"
                    },
                db = dat,
                debug = true
            }.Load("fame.json")
            );

            var brand = new BrandService(new WebServiceConf
            {
                key = "biz",
                outer = "127.0.0.1:8082",
                inner = "127.0.0.1:7772",
                foreign = new[]
                    {
                        "127.0.0.1:7777"
                    },
                db = dat,
                debug = true
            }.Load("biz.json")
            );

            var post = new PostService(new WebServiceConf
            {
                key = "post",
                outer = "127.0.0.1:8083",
                inner = "127.0.0.1:7773",
                foreign = new[]
                    {
                        "localhost:7777"
                    },
                db = dat,
                debug = true
            }.Load("post.json")
            );

            var notice = new NoticeService(new WebServiceConf
            {
                key = "notice",
                outer = "127.0.0.1:8084",
                inner = "127.0.0.1:7774",
                foreign = new[]
                    {
                        "localhost:7783"
                    },
                db = dat,
                debug = true
            }.Load("notice.json")
            );

            var user = new UserService(new WebServiceConf
            {
                key = "user",
                outer = "127.0.0.1:8085",
                inner = "127.0.0.1:7775",
                foreign = new[]
                    {
                        "localhost:7783"
                    },
                db = dat,
                debug = true
            }.Load("user.json")
            );

            var chat = new ChatService(new WebServiceConf
            {
                key = "chat",
                outer = "127.0.0.1:8086",
                inner = "127.0.0.1:7776",
                foreign = new[]
                    {
                        "localhost:7783"
                    },
                db = dat,
                debug = true
            }.Load("chat.json")
            );

            WebService.Run(www, fame, brand, post, notice, user, chat);
        }
    }
}