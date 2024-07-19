package com.example.demo.service;

import com.example.demo.model.Song;
import com.example.demo.repository.SongRepository;
import io.lettuce.core.dynamic.annotation.Param;
import jakarta.annotation.Resource;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class JpaSongServiceImpl implements JpaSongService {

    @Resource
    private SongRepository songRepository;

    public List<Song> findByUsername(Song song){
        List<Song> songs=songRepository.findByUsername(song.getUsername());
        if(songs.size()>0) {
            return songs;
        }
        return null;
    }

    @Override
    public Song findBySongname(String songname) {
        return songRepository.findBySongname(songname);
    }

    public Song findSong(Song song){
        return songRepository.findByUsernameAndSongnameAndSongwordAndAddress(song.getUsername(),song.getSongname(),song.getSongword(),song.getAddress());
    }

    public Song insertSong(Song song){
        return songRepository.save(song);
    }
    public Song updateSong(Song song){
        return songRepository.save(song);
    }
    public void deleteSong(Song song){
        songRepository.delete(song);
    }
    public List<Song> findByAddressLike(@Param("extension") String extension){
        return songRepository.findByAddressLike(extension);
    }
}
