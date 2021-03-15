using System.Threading.Tasks;
using Indice.AspNetCore.Fido.Models;

namespace Indice.AspNetCore.Fido.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public class RegistrationChallengeStoreInMemory : IRegistrationChallengeStore
    {
        /// <inheritdoc />
        public Task<PublicKeyCredentialCreationOptionsBase64> GetByKey(string key) {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task Persist(string key, PublicKeyCredentialCreationOptionsBase64 publicKeyCredentialCreationOptions) {
            throw new System.NotImplementedException();
        }
    }
}
