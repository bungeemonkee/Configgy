using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Configgy.Utilities;

namespace Configgy.Encrypter
{
    public class Context : INotifyPropertyChanged
    {
        private string _input;
        private string _output;
        private DisplayCertificate _certificate;

        public event PropertyChangedEventHandler PropertyChanged;

        public IList<DisplayCertificate> Certificates { get; set; }

        public DisplayCertificate Certificate
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
            handler?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void SetOutput(string input, DisplayCertificate certificate)
        {
            if (input == null || certificate == null)
            {
                return;
            }

            try
            {
                Output = EncryptionUtility.Encrypt(input, certificate.Certificate);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Encryption failed: ({ex.GetType().Name}) {ex.Message}");
                //  TODO: Tell the user what went wrong
            }
        }

        private static IList<DisplayCertificate> GetCertificates()
        {
            return EncryptionUtility
                .FindCertificates(x => true)
                .Select(x => new DisplayCertificate(x))
                .Distinct()
                .OrderByDescending(x => x.Certificate.HasPrivateKey)
                .ThenBy(x => x.Certificate.FriendlyName)
                .ToList();
        }
    }
}
