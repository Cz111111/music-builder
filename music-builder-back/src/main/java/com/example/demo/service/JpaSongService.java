package com.example.demo.service;

import com.example.demo.model.Song;
import io.lettuce.core.dynamic.annotation.Param;

import java.util.List;

public interface JpaSongService {
    List<Song> findByUsername(Song song);
    Song findBySongname(String songname);
    Song insertSong(Song song);
    Song updateSong(Song song);
    void deleteSong(Song song);
    Song findSong(Song song);
    List<Song> findByAddressLike(String extension);
}
