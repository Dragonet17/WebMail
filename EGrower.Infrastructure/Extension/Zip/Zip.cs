using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using EGrower.Core.Domains;

namespace EGrower.Infrastructure.Extension.Zip {
    public class Zip {
        public static async Task<MemoryStream> GetZipStream (IEnumerable<Atachment> atachments) {
            if (atachments != null || atachments.Count () == 0) {
                using (MemoryStream ms = new MemoryStream ()) {
                    using (var archive = new ZipArchive (ms, ZipArchiveMode.Create, true)) {
                        foreach (var item in atachments) {
                            var zipArchiveEntry = archive.CreateEntry (item.Name, CompressionLevel.Fastest);
                            using (var zipStream = zipArchiveEntry.Open ()) await zipStream.WriteAsync (item.Data, 0, item.Data.Length);
                        }
                    }
                    return await Task.FromResult (ms);
                }
            } else
                return null;
        }

        public static async Task<MemoryStream> GetZipStream (IEnumerable<SendedAtachment> atachments) {
            if (atachments != null || atachments.Count () == 0) {
                using (MemoryStream ms = new MemoryStream ()) {
                    using (var archive = new ZipArchive (ms, ZipArchiveMode.Create, true)) {
                        foreach (var item in atachments) {
                            var zipArchiveEntry = archive.CreateEntry (item.Name, CompressionLevel.Fastest);
                            using (var zipStream = zipArchiveEntry.Open ()) await zipStream.WriteAsync (item.Data, 0, item.Data.Length);
                        }
                    }
                    return await Task.FromResult (ms);
                }
            } else
                return null;
        }
    }
}