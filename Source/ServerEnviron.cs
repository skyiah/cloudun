﻿using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SkyChain.Db;
using SkyChain.Web;

namespace SkyChain
{
    /// <summary>
    /// The application scope that holds global states.
    /// </summary>
    public class ServerEnviron : ChainEnviron
    {
        public const string APP_JSON = "app.json";

        public const string CERT_PFX = "cert.pfx";

        internal static readonly WebLifetime Lifetime = new WebLifetime();

        internal static readonly ITransportFactory TransportFactory = new SocketTransportFactory(Options.Create(new SocketTransportOptions()), Lifetime, NullLoggerFactory.Instance);

        //
        // configuration processing
        //

        // logging level
        static int logging;

        static string crypto;

        static uint[] privatekey;

        static string certpasswd;

        public static JObj webcfg, dbcfg, extcfg; // various config parts


        internal static ServerLogger logger;

        static readonly Map<string, WebService> services = new Map<string, WebService>(4);


        public static uint[] PrivateKey => privatekey;

        public static ServerLogger Logger => logger;

        /// <summary>
        /// Load & process the configuration file.
        /// </summary>
        public static void Configure(string file = "app.json")
        {
            // load configuration
            //
            var bytes = File.ReadAllBytes(file);
            var parser = new JsonParser(bytes, bytes.Length);
            var cfg = (JObj) parser.Parse();

            logging = cfg[nameof(logging)];
            crypto = cfg[nameof(crypto)];
            privatekey = CryptoUtility.HexToKey(crypto);
            certpasswd = cfg[nameof(certpasswd)];

            // setup logger first
            string logf = DateTime.Now.ToString("yyyyMM") + ".log";
            logger = new ServerLogger(logf)
            {
                Level = logging
            };

            dbcfg = cfg["db"];
            if (dbcfg != null) // setup the db source
            {
                ConfigureDb(dbcfg);
            }

            webcfg = cfg["web"];

            extcfg = cfg["ext"];

            // create cert
            // var cert = BuildSelfSignedCertificate("144000.tv", "47.100.96.253", "144000.tv", "721004");
            // var bytes = cert.Export(X509ContentType.Pfx);
            // File.WriteAllBytes("samp/$cert.pfx", bytes);
        }

        public static T CreateService<T>(string name) where T : WebService, new()
        {
            if (webcfg == null)
            {
                throw new ServerException("Missing 'web' in config");
            }

            string addr = webcfg[name];
            if (addr == null)
            {
                throw new ServerException("missing web '" + name + "' in config");
            }

            // create service
            var svc = new T
            {
                Name = name,
                Address = addr
            };
            services.Add(name, svc);

            svc.OnCreate();
            return svc;
        }


        //
        // logging methods
        //

        public static void TRC(string msg, Exception ex = null)
        {
            if (msg != null)
            {
                Logger.Log(LogLevel.Trace, 0, msg, ex, null);
            }
        }

        public static void DBG(string msg, Exception ex = null)
        {
            if (msg != null)
            {
                Logger.Log(LogLevel.Debug, 0, msg, ex, null);
            }
        }

        public static void INF(string msg, Exception ex = null)
        {
            if (msg != null)
            {
                Logger.Log(LogLevel.Information, 0, msg, ex, null);
            }
        }

        public static void WAR(string msg, Exception ex = null)
        {
            if (msg != null)
            {
                Logger.Log(LogLevel.Warning, 0, msg, ex, null);
            }
        }

        public static void ERR(string msg, Exception ex = null)
        {
            if (msg != null)
            {
                Logger.Log(LogLevel.Error, 0, msg, ex, null);
            }
        }


        static readonly CancellationTokenSource Canceller = new CancellationTokenSource();


        /// <summary>
        /// Runs a number of web services and then block until shutdown.
        /// </summary>
        public static async Task StartWebAsync()
        {
            var exitevt = new ManualResetEventSlim(false);

            // start all services
            //
            for (int i = 0; i < services.Count; i++)
            {
                var svc = services.ValueAt(i);
                await svc.StartAsync(Canceller.Token);
            }

            // handle SIGTERM and CTRL_C 
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            {
                Canceller.Cancel(false);
                exitevt.Set(); // release the Main thread
            };
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Canceller.Cancel(false);
                exitevt.Set(); // release the Main thread
                // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                eventArgs.Cancel = true;
            };
            Console.WriteLine("CTRL + C to shut down");

            Lifetime.NotifyStarted();

            // wait on the reset event
            exitevt.Wait(Canceller.Token);

            Lifetime.StopApplication();

            for (int i = 0; i < services.Count; i++)
            {
                var svc = services.ValueAt(i);
                await svc.StopAsync(Canceller.Token);
            }

            Lifetime.NotifyStopped();
        }

        public static X509Certificate2 BuildSelfSignedCertificate(string dns, string ipaddr, string issuer, string password)
        {
            var sanb = new SubjectAlternativeNameBuilder();
            // sanb.AddIpAddress(IPAddress.Parse(ipaddr));
            sanb.AddDnsName(dns);

            var subject = new X500DistinguishedName($"CN={issuer}");

            using var rsa = RSA.Create(2048);

            var request = new CertificateRequest(subject, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));

            request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection {new Oid("1.3.6.1.5.5.7.3.1")}, false));

            request.CertificateExtensions.Add(sanb.Build());

            var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
            certificate.FriendlyName = issuer;

            return new X509Certificate2(certificate.Export(X509ContentType.Pfx, password), password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
        }
    }
}