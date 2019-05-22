using Microsoft.Graph.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MyGraph
{
    public sealed class FileBasedTokenStorageProvider : ITokenStorageProvider
    {
        private const string CacheFilePath = @"./tokencache.bin";

        public static readonly FileBasedTokenStorageProvider Instance = new FileBasedTokenStorageProvider();

        FileBasedTokenStorageProvider() { }

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

        public void ClearCache()
        {
            if (File.Exists(CacheFilePath))
            {
                File.Delete(CacheFilePath);
            }
        }
    }
}
