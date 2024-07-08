# 导入Spleeter的Separator类用于音频分离
from spleeter.separator import Separator
# 导入sys模块用于访问命令行参数
import sys
# 导入os模块用于处理文件和目录
import os


# 定义一个函数用于分离音频中的人声和伴奏
def separate_vocals(audio_file_name):
    # 创建Separator对象，使用2stems模型来分离人声和伴奏
    separator = Separator('spleeter:2stems')
    
    # 运行分离
    separator.separate_to_file(audio_file_name, os.getcwd())

if __name__ == '__main__':
    # 确保用户提供了文件名
    if len(sys.argv) < 2:
        print("Usage: python separate_vocals.py <audio_file_name>")
        sys.exit(1)

    audio_file_name = sys.argv[1]
    
    # 检查文件是否存在
    if not os.path.isfile(audio_file_name):
        print(f"File not found: {audio_file_name}")
        sys.exit(1)
    
    separate_vocals(audio_file_name)
    print(f"Processing completed. Vocals and accompaniment have been separated for '{audio_file_name}'.")