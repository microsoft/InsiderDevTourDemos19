using Microsoft.Graph.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MyGraph
{
    public class FileBasedTokenStorageProvider : ITokenStorageProvider
    {
        private const string CacheFilePath = @"./tokencache.bin";

        public Task<byte[]> GetTokenCacheAsync(string cacheId)
        {
            if (File.Exists(CacheFilePath))
            {
                return Task.FromResult(
                    ProtectedData.Unprotect(File.ReadAllBytes(CacheFilePath),
                                            null,
                                            DataProtectionScope.CurrentUser));
            }
            else
            {
                return Task.FromResult(new Byte[0]);
            }
        }

        public Task SetTokenCacheAsync(string cacheId, byte[] tokenCache)
        {
            File.WriteAllBytes(CacheFilePath, 
                ProtectedData.Protect(tokenCache,
                                      null,
                                      DataProtectionScope.CurrentUser));

            return Task.FromResult<object>(null);
        }
    }
}
