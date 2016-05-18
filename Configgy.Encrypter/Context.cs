using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using Configgy.Utilities;
using System.Security.Cryptography;

namespace Configgy.Encrypter
{
    public class Context : INotifyPropertyChanged
    {
        private string _input;
        private string _output;
        private CertificateWithSource _certificate;

        public event PropertyChangedEventHandler PropertyChanged;

        public IList<CertificateWithSource> Certificates { get; set; }

        public CertificateWithSource Certificate
        {
            get { return _certificate; }
            set
            {
                if (_certificate == value) return;
                _certificate = value;
                NotifyPropertyChanged();
                SetOutput(_input, _certificate);
            }
        }

        public string Input
        {
            get { return _input; }
            set
            {
                if (_input == value) return;
                _input = value;
                NotifyPropertyChanged();
                SetOutput(_input, _certificate);
            }
        }

        public string Output
        {
            get { return _output; }
            set
            {
                if (_output == value) return;
                _output = value;
                NotifyPropertyChanged();
            }
        }

        public Context()
        {
#if DEBUG
            Debugger.Launch();
#endif
            Certificates = GetCertificates();
            Certificate = Certificates.FirstOrDefault();
            Input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
        }

        private void NotifyPropertyChanged([CallerMemberName] string property = null)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(property));
        }

        private async void SetOutput(string input, CertificateWithSource certificate)
        {
            if (input == null || certificate == null)
            {
                return;
            }

            try
            {
                Output = EncryptionUtility.Encrypt(input, certificate.Certifcate.Thumbprint, certificate.StoreName, certificate.StoreLocation);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Encryption failed: ({ex.GetType().Name}) {ex.Message}");
                //  TODO: Tell the user what went wrong
            }
        }

        private static IList<CertificateWithSource> GetCertificates()
        {
            var stores = Enum.GetValues(typeof(StoreName)).Cast<StoreName>().ToArray();
            var locations = Enum.GetValues(typeof(StoreLocation)).Cast<StoreLocation>().ToArray();

            var certificates = new List<CertificateWithSource>();

            foreach (var store in stores)
            {
                foreach (var location in locations)
                {
                    X509Store source = null;
                    try
                    {
                        source = new X509Store(store, location);
                        source.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
                        certificates.AddRange(source.Certificates
                            .Cast<X509Certificate2>()
                            .Where(x => x.HasPrivateKey)
                            .Where(x => x.PrivateKey is RSACryptoServiceProvider)
                            .Select(x => new CertificateWithSource
                            {
                                Certifcate = x,
                                StoreName = store,
                                StoreLocation = location
                            })
                        );
                    }
                    catch
                    {
                        // Just try the next one
                    }
                    finally
                    {
                        if (source != null)
                        {
                            source.Close();
                        }
                    }
                }
            }

            return certificates;
        }
    }
}
