package com.example.demo.controller;


import com.example.demo.common.R;
import com.example.demo.model.Midi;
import com.example.demo.model.Song;
import com.example.demo.repository.SongRepository;
import com.example.demo.service.JpaSongService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.FileSystemResource;
import org.springframework.core.io.Resource;
import org.springframework.core.io.UrlResource;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.io.File;
import java.io.IOException;
import java.net.MalformedURLException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
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
    public R updateSong(@RequestParam("songname") String songname,@RequestParam("songword") String songword,@RequestParam("originName") String oriName){
        System.out.println(oriName);
        Song song = jpaSongService.findBySongname(oriName);
        System.out.println(song);
        if(song == null){
            return R.error().message("音乐不存在");
        }
        Song song1 = new Song(song.getUsername(),songname,songword,song.getAddress());
        jpaSongService.deleteSong(song);
        jpaSongService.insertSong(song1);
        return R.ok().message("更新歌曲成功");
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
    public static String extractFileName(String filePath) {
        Path path = Paths.get(filePath);
        return path.getFileName().toString();
    }
    public static String extractParentPath(String filePath) {
        Path path = Paths.get(filePath);
        Path parentPath = path.getParent();
        return parentPath != null ? parentPath.toString() : null;
    }
    @PostMapping("/getUserMidi")
    public List<Midi> getMidi(@RequestBody Song song) throws IOException {
        List<Midi> songs= new ArrayList<>();
        List<Song> songs1= jpaSongService.findByAddressLike(".midi");
        for(Song s:songs1) {
            if (s.getUsername().equals(song.getUsername())) {
                Path fileStorageLocation = Paths.get(extractParentPath(s.getAddress())).toAbsolutePath().normalize();
                Path filePath = fileStorageLocation.resolve(extractFileName(s.getAddress())).normalize();
                Resource resource = new UrlResource(filePath.toUri());

                if (resource.exists()) {
                    byte[] fileContent = Files.readAllBytes(filePath);
                    Midi midi = new Midi();
                    midi.setUsername(s.getUsername());
                    midi.setSongname(s.getSongname());
                    midi.setSongword(s.getSongword());
                    midi.setFile(fileContent);
                    songs.add(midi);
                }
            }
        }
        System.out.println(songs);
        return songs;
    }
}
