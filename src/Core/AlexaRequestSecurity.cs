using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Cascade.Alexa.Core
{
    public class AlexaRequestSecurity : IValidatableObject
    {
        public X509Certificate2 Certificate { get; private set; }
        public readonly Uri SignatureUrl;

        private RSA _cryptoServiceProvider;
        private DateTime _effectiveDate;
        private DateTime _expiryDate;
        private byte[]  _signature;
        private byte[] _requestBody;

        public AlexaRequestSecurity(string signatureCertChainUrl, string signature, string requestBody)
        {
            if (string.IsNullOrEmpty(signatureCertChainUrl))
                throw new ArgumentNullException(nameof(signatureCertChainUrl));

            _expiryDate = DateTime.MinValue;
            _effectiveDate = DateTime.MinValue;
            _signature = Convert.FromBase64String(signature);

            SignatureUrl = new Uri(signatureCertChainUrl);
            DownloadCertificate(signatureCertChainUrl);

            using (var sha = new SHA1Managed())
            {
                _requestBody = sha.ComputeHash(Encoding.UTF8.GetBytes(requestBody));
            }
        }

        /// <summary>
        /// Validation rules from Amazons guidelines on acceptable request data:
        /// https://developer.amazon.com/docs/custom-skills/host-a-custom-skill-as-a-web-service.html#check-request-signature
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SignatureUrl.Port != 443)
                yield return new ValidationResult("Certificate must come from port 443");

            if (SignatureUrl.Scheme != "https")
                yield return new ValidationResult("Certificate must be from a secure connection");

            if (!SignatureUrl.Host.Equals("s3.amazonaws.com", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("Certificate must come from s3.amazonaws.com");

            if (!SignatureUrl.LocalPath.StartsWith("/echo.api/"))
                yield return new ValidationResult("Certificate path must originate from echo.api");

            if (!(DateTime.TryParse(Certificate.GetExpirationDateString(), out _expiryDate) && _expiryDate > DateTime.UtcNow))
                yield return new ValidationResult("Certificate has expired");

            if (!(DateTime.TryParse(Certificate.GetEffectiveDateString(), out _effectiveDate) && _effectiveDate < DateTime.UtcNow))
                yield return new ValidationResult("Certificate is not yet valid");

            if (!Certificate.Subject.Contains("CN=echo-api.amazon.com"))
                yield return new ValidationResult("required subject alternative name is missing: 'CN=echo-api.amazon.com'");

            if (_cryptoServiceProvider == null || !_cryptoServiceProvider.VerifyHash(_requestBody, _signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1))
                yield return new ValidationResult("Could not verify the request signature");

            if (!new X509Chain().Build(Certificate))
                yield return new ValidationResult("Chain could not be built");
        }

        private void DownloadCertificate(string certificateUrl)
        {
            using (var client = new WebClient())
            {
                var byteResponse = client.DownloadData(certificateUrl);
                Certificate = new X509Certificate2(byteResponse);
                _cryptoServiceProvider = Certificate.GetRSAPublicKey();
            }
        }
    }

    public static class AlexaRequestSecurityExtension
    {
        public static IEnumerable<ValidationResult> RunValidation(this AlexaRequestSecurity requestValidator)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(requestValidator, null, null);
            Validator.TryValidateObject(requestValidator, context, results, true);

            return results;
        }
    }
}
