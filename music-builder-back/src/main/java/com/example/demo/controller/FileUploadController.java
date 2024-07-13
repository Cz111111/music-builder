package com.example.demo.controller;


import com.example.demo.common.R;
import org.springframework.web.bind.annotation.*;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.multipart.MultipartFile;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

@RestController
public class FileUploadController {


    @PostMapping("/upload")
    public R uploadFile(@RequestParam("file") MultipartFile file, @AuthenticationPrincipal UserDetails userDetails) {
        try {
            // 获取当前登录用户的用户名
            String username = userDetails.getUsername();

            // 创建基于用户名的目录路径
            Path userDir = Paths.get("uploads", username);

            // 检查目录是否存在，如果不存在则创建
            if (!Files.exists(userDir)) {
                Files.createDirectories(userDir);
            }

            // 获取文件的原始文件名
            String originalFilename = file.getOriginalFilename();

            // 生成新的文件名：原始文件名_时间戳
            long timestamp = System.currentTimeMillis();
            String newFilename = originalFilename + "_" + timestamp;

            // 获取完整的文件保存路径
            Path filePath = userDir.resolve(newFilename);

            // 将文件内容写入到用户目录中
            Files.write(filePath, file.getBytes());

            return R.ok().message("上传成功");
        } catch (IOException e) {
            return R.error().message(e.getMessage());
        }
    }

}
