package com.example.demo.controller;


import com.example.demo.common.R;
import com.example.demo.model.Song;
import com.example.demo.repository.SongRepository;
import com.example.demo.service.JpaSongService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.FileSystemResource;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;

import java.io.File;
import java.util.List;

@RestController
@RequestMapping("/song")
public class SongController {

    @Autowired
    private JpaSongService jpaSongService;

    @PostMapping("/getUserSongs")
    public List<Song> getUserSongs(@RequestBody Song song){
        List<Song> l =jpaSongService.findByUsername(song);
        return l;
    }

    @PostMapping("/insert")
    public R insertSong(@RequestBody Song song){
        Song song1 = jpaSongService.insertSong(song);
        if(song1 != null){
            return R.ok().message("新增歌曲成功");
        }
        return R.error().message("新增歌曲失败");
    }

    @PostMapping("/update")
    public R updateSong(@RequestBody Song song){
        Song song1 = jpaSongService.updateSong(song);
        if(song1 != null){
            return R.ok().message("更新歌曲成功");
        }
        return R.error().message("更新歌曲失败");
    }

    @PostMapping("/download")
    public ResponseEntity<FileSystemResource> downloadSong(@RequestParam("songname") String songname) {
        // 指定文件路径
        Song song=jpaSongService.findBySongname(songname);
        String path = song.getAddress();
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

    @PostMapping("/delete")
    public R deleteSong(@RequestParam("songname") String songname){
        Song song=jpaSongService.findBySongname(songname);
        jpaSongService.deleteSong(song);
        Song song1 = jpaSongService.findSong(song);
        if(song1 == null){
            return R.ok().message("删除歌曲成功");
        }
        return R.error().message("删除歌曲失败");
    }
}
