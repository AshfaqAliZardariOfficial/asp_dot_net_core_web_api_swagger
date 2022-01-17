using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testing.Utils
{
    public sealed class DataProtectionUtils
    {
        /// <summary>
        /// IDataProtector interface declaration.
        /// </summary>
        private IDataProtector _protector;

        /// <summary>
        ///  The 'provider' parameter is provided by DI
        /// </summary>
        /// <param name="provider"></param>
        public DataProtectionUtils(IDataProtectionProvider provider)
        {
            // IDataProtector Initialization.
            _protector = provider.CreateProtector("Testing.DataProtectionUtils.v1");
        }

        /// <summary>
        /// Protect data method, will return encrypted string.
        /// </summary>
        /// <param name="Data">Data string that will be protected.</param>
        /// <returns>Protected data string.</returns>
        public string Protect(string Data)
        {
            return _protector.Protect(Data);
        }

        /// <summary>
        /// Unprotect data method, will return decrypted string.
        /// </summary>
        /// <param name="ProtectedData">ProtectedData data string.</param>
        /// <returns>Unprotected data string.</returns>
        public string Unprotect(string ProtectedData)
        {
            return _protector.Unprotect(ProtectedData);
        }

        /// <summary>
        /// VerifyData method. It will return true if both matched, otherwise false.
        /// </summary>
        /// <param name="Data">User input data string.</param>
        /// <param name="DatabaseEncryptedData">Database encrypted data string.</param>
        /// <returns></returns>
        public bool VerifyData(string Data, string DatabaseEncryptedData)
        {
            return Unprotect(DatabaseEncryptedData).ToString().Equals(Data);
        }
    }
}
