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

        public void RemoveAudio(int audionum)
        {
            
                var audio = context.Audios.SingleOrDefault(a => a.Num == audionum);
                if (audio == null)
                {
                    throw new InvalidOperationException("The audio does not exist.");
                }
                context.Audios.Remove(audio); // 从 DbSet 中移除音频
                context.SaveChanges(); // 保存更改到数据库
            
        }

        public void ModifyAudio(Audio audio)//更新audio的信息
        {
            
                var existingAudio = context.Audios.SingleOrDefault(a => a.Num == audio.Num);
                if (existingAudio == null)
                {
                    throw new InvalidOperationException("The audio does not exist.");
                }

                // 更新现有音频信息
                context.Audios.Remove(existingAudio);
                context.Audios.Add(audio);
                context.SaveChanges(); // 保存更改到数据库
            
        }
        // 查询音频
        public List<Audio> QueryAudios(Func<Audio, bool> condition)
        {

                //var audios = context.Audios.Where(condition).OrderBy(a => a.CreatedAt).ToList();
                var audios = context.Audios.Where(condition).ToList();
                return audios;
        }

        // 排序
        public List<Audio> SortAudios(Expression<Func<Audio, object>> sortExpression)
        {
            
                var sortedAudios = context.Audios.OrderBy(sortExpression).ToList();
                return sortedAudios;
           
        }

        public int GetMaxNum()
        {
           
                return context.Audios.Any() ? context.Audios.Max(a => a.Num) : 0;
        }

        public void UpdateAudio(int audioId, string newTitle)
        {
           
                var existingAudio = context.Audios.SingleOrDefault(a => a.Id == audioId);
                if (existingAudio != null)
                {
                    existingAudio.Title = newTitle; // 更新音频的标题
                    context.SaveChanges(); // 保存更改到数据库
                }
                else
                {
                    throw new InvalidOperationException("No audio found with the specified ID.");
                }
            
        }

        public void RefreshAudioSerialNumbers()
        {
           
                // 获取所有音频并根据创建时间或其他属性排序
                var allAudios = context.Audios.OrderBy(a => a.CreatedAt).ToList();

                // 重新分配序号
                int newNum = 1;
                foreach (var audio in allAudios)
                {
                    audio.Num = newNum++;
                }

                // 保存更改
                context.SaveChanges();
            
        }

        public void TopAudio(int audioId)
        {

                var audioToTop = context.Audios.SingleOrDefault(a => a.Id == audioId);
                if (audioToTop != null)
                {
                    // 获取除了要置顶的音频之外的所有音频，并按照序号排序
                    var otherAudios = context.Audios
                        .Where(a => a.Id != audioId)
                        .OrderBy(a => a.Num)
                        .ToList();

                    // 将要置顶的音频序号设置为1
                    audioToTop.Num = 1;

                    // 更新其他音频的序号
                    int newNum = 2;
                    foreach (var audio in otherAudios)
                    {
                        audio.Num = newNum++;
                    }

                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Audio not found for topping.");
                }
            
        }

        public void DeleteAudio(int audioId)
        {

                var audioToDelete = context.Audios.SingleOrDefault(a => a.Id == audioId);
                if (audioToDelete != null)
                {
                    // 删除音频
                    context.Audios.Remove(audioToDelete);
                    context.SaveChanges();

                    // 获取所有剩余音频，并按照序号排序
                    var allOtherAudios = context.Audios.OrderBy(a => a.Num).ToList();

                    // 重新分配序号
                    int newNum = 1;
                    foreach (var audio in allOtherAudios)
                    {
                        audio.Num = newNum++;
                    }

                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Audio not found for deletion.");
                }
            
        }

        public List<Audio> GetSortedAudios()
        {

                // 获取所有音频的列表，并按Num字段排序
                return context.Audios.OrderBy(a => a.Num).ToList();
            
        }


    }
}
