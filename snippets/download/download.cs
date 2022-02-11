class Downloader {

        /// <summary>
        /// Downloads a file from the given source address and saves the file at dest
        /// </summary>
        /// <param name="src">The source address.</param>
        /// <param name="dest">The path to the cache.</param>
        /// <param name="useCache">If true and size of cache file and remote file are equal the download is not performed.</param>
        /// <param name="checkFileSize">If true and there is a file in cache the file size of remote and local will be compared.</param>
        /// <param name="retry">Workaround for underlying connection closed. if retry < 0 the default retry value is used. if retry == 0 and dest does not exist the download will fail on first exception.</param>
        private static bool Download(Uri src, FileInfo dest, bool useCache = false, bool checkFileSize = true, int retry = -1)
        {
            // create request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(src);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            long range = 0;
            if(retry >= 0 && File.Exists(dest.FullName))
            {
                dest = new FileInfo(dest.FullName);
                range = dest.Length;
                Console.WriteLine("Continue from: {0}", range);
                request.AddRange(range);
            }
            // add auth header
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(Credentials.UserName + ":" + Credentials.Password)));
            try
            {
                if (useCache && dest.Exists && !checkFileSize)
                {
                    return true;
                }
                Console.WriteLine("Requesting {0}", src.ToString());
                // Assign the response object of HttpWebRequest to a HttpWebResponse variable.
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    // check if cache shall be used and if true then check whether file size is equals.
                    if (useCache && dest.Exists && dest.Length == webResponse.ContentLength)
                    {
                        // is local available
                        return true;
                    }

                    Console.WriteLine("Downloading {0}", src.ToString());
                    if (dest.Exists && retry < 0)
                    {
                        dest.Delete();
                    }
                    if (!dest.Directory.Exists)
                    {
                        dest.Directory.Create();
                    }

                    // save to file
                    using (Stream responseStream = webResponse.GetResponseStream())
                    {
                        try
                        {
                            FileMode openMode = retry >= 0 && dest.Exists ? FileMode.Append : FileMode.Create;
                            using (FileStream fileStream = new FileStream(dest.FullName, openMode, FileAccess.Write, FileShare.None))
                            {
                                try
                                {
                                    // use CopyToAsync instead of CopyTo because copy to won' throw exception if connection closed before download is done.
                                    Task task = responseStream.CopyToAsync(fileStream);
                                    task.Wait();
                                    if (webResponse.ContentLength >= 0 && fileStream.Length - range != webResponse.ContentLength)
                                    {
                                        retry = 0;
                                        throw new Exception("Failed to download complete file. " + src.ToString());
                                    }
                                }
                                catch(Exception e)
                                {
                                    if(retry != 0 && fileStream.Length < webResponse.ContentLength)
                                    {
                                        fileStream.Close();
                                        fileStream.Dispose();
                                        Console.WriteLine(src.ToString() + ": " + e.ToString());
                                        return Download(src, dest, useCache, checkFileSize, retry < 0 ? 5 : retry - 1);
                                    }
                                    else
                                    {
                                        throw;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            if (File.Exists(dest.FullName))
                                File.Delete(dest.FullName);
                            throw;
                        }
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(src.ToString() + ": " + e.ToString());
            }
            return false;
        }
    }
}
