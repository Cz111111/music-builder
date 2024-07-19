package com.example.demo.controller;

import com.example.demo.model.OtherFile;
import com.example.demo.model.Song;
import com.example.demo.service.JpaOtherFileService;
import com.example.demo.service.JpaSongService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.FileSystemResource;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.io.File;
import java.util.List;

@RestController
@RequestMapping("/download")
public class OtherFileController {

    @Autowired
    private JpaOtherFileService jpaOtherFileService;

    @PostMapping("/getUserFile")
    public List<OtherFile> getUserFile(@RequestBody OtherFile otherFile){
        return jpaOtherFileService.findByUsername(otherFile);
    }

    @PostMapping("/download")
    public ResponseEntity<FileSystemResource> downloadSong(@RequestParam("name") String name) {
        // 指定文件路径
        OtherFile otherFile=jpaOtherFileService.findByFilename(name);
        String path = otherFile.getAddress();
        String filePath = path;
        File file = new File(filePath).getAbsoluteFile();
        if (!file.exists()) {
            // 如果文件不存在，返回错误信息
            return ResponseEntity.notFound().build();
        }
        FileSystemResource resource = new FileSystemResource(file);
        return ResponseEntity.ok()
                .header("Content-Disposition", "attachment; filename=" + resource.getFilename())
                .body(resource);
    }
}
