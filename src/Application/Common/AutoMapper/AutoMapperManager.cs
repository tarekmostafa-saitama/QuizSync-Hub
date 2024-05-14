using AutoMapper;
using CleanArchitecture.Application.Common.AutoMapper.Profiles;

namespace CleanArchitecture.Application.Common.AutoMapper
{
    public static class AutoMapperManager
    {
        private static readonly Lazy<IMapper> LazyMapper = new(() =>
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<MainProfiler>(); });

            var mapper = config.CreateMapper();

            return mapper;
        });

        public static IMapper Mapper => LazyMapper.Value;
    }
}