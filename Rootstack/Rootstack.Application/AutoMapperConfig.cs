using AutoMapper;

namespace GoogleScraping.Application
{
    public class AutoMapperConfig
    {
        public static object thisLock = new object();
        public static void Initialize()
        {
            // This will ensure one thread can access to this static initialize call
            // and ensure the mapper is reseted before initialized
            lock (thisLock)
            {
                Mapper.Reset();
                Mapper.Initialize(cfg =>
                {
                });

            }
        }
    }
}
