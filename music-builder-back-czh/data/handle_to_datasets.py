import json

with open("./raw_data.json", 'r', encoding='utf-8') as file:
    data = json.load(file)

for item in data:
    new_output = '，'.join(item['output'])
    item['output'] = new_output

# 将处理后的数据写入新的JSON文件
with open('./lyrics_data.json', 'w', encoding='utf-8') as file:
    json.dump(data, file, ensure_ascii=False, indent=4)

print("处理完成")