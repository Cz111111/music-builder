package com.example.demo.service;


import com.example.demo.model.Song;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

@Service
public class FileUploadService {

    @Autowired
    JpaSongService jpaSongService;
    public boolean uploadFile(byte[] fileData,String fileName, String username) {
        try {
            // 获取当前登录用户的用户名

            // 创建基于用户名的目录路径
            Path userDir = Paths.get("uploads", username);

            // 检查目录是否存在，如果不存在则创建
            if (!Files.exists(userDir)) {
                Files.createDirectories(userDir);
            }

            // 获取文件的原始文件名

            // 生成新的文件名：原始文件名_时间戳
            long timestamp = System.currentTimeMillis();
            String newFilename = fileName + "_" + timestamp;

            // 获取完整的文件保存路径
            Path filePath = userDir.resolve(newFilename);
            Files.write(filePath, fileData);
            Song song = new Song(username,fileName,null,"uploads/"+username+"/"+newFilename);
            jpaSongService.insertSong(song);


            return true;
        } catch (IOException e) {
            return false;
        }
    }

}
