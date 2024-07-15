using System.Linq.Expressions;

namespace SingScore
{
    public class AudioService
    {
        private readonly AudioContext context;

        public AudioService(AudioContext context)
        {
            this.context = context;
        }


        public bool AddAudio(Audio audio)
        {

            if (context.Audios.Any(a => a.Title == audio.Title))
            {
                //throw new InvalidOperationException("The audio already exists.");
                return false;
            }
            context.Audios.Add(audio); // 将音频添加到 DbSet 中
                context.SaveChanges(); // 保存更改到数据库
            return true;
        }

        public int GetMaxNum()
        {

            return context.Audios.Any() ? context.Audios.Max(a => a.Num) : 0;
        }

    }
}
