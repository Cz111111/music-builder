import json
from tqdm import tqdm
import requests


# 加载原json数据文件 保存在songs
with open('songs.json', 'r', encoding='utf-8') as f:
    songs = json.load(f)

api_key = 'sk-c6d665660efc400cbc8190063d7325d2'
url = 'https://dashscope.aliyuncs.com/api/v1/services/aigc/text-generation/generation'
headers = {'Content-Type': 'application/json',
               'Authorization': f'Bearer {api_key}'}

songs_with_progress = tqdm(songs[4194:], desc="Processing songs")

for song in songs_with_progress:
    input = song['lyric'].copy()
    input.insert(0, "给你一段歌词，你帮我总结它的风格，表达的感情、心情，用连续的文字给出，严格要求一共不超过30个字")
    body = {
        'model': 'qwen-turbo',
        "input": {
            "messages": [
                {
                    "role": "system",
                    "content": "You are a helpful assistant."
                },
                {
                    "role": "user",
                    "content": "".join(song['lyric'])
                }
            ]
        },
        "parameters": {
            "result_format": "message"
        }
    }

    response = requests.post(url, headers=headers, json=body)
    data = response.json()
    content_value = data['output']['choices'][0]['message']['content']
    content_value = content_value.replace('这首歌', '')
    # 使用第一个歌曲的lyric内容和content_value创建新的JSON键值对
    res = {
        "instruction": content_value,
        "input": "",
        "output": song['lyric']
    }
    with open('raw_data.json', 'a', encoding='utf-8') as f:
        json.dump(res, f, ensure_ascii=False, indent=4)
        f.write(',')
        f.write('\n')



#
#
# # 打印新的JSON对象
# print(json.dumps(res, ensure_ascii=False, indent=4))