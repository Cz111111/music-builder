using Microsoft.EntityFrameworkCore;

namespace SingScore
{
    public class AudioContext:DbContext
    {
        public AudioContext(DbContextOptions<AudioContext> options)
          : base(options)
        {
            this.Database.EnsureCreated(); //自动建库建表
        }
        public DbSet<Audio> Audios { get; set; }
    }
}
