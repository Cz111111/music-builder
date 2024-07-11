package com.example.demo.repository;

import com.example.demo.model.Song;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import java.util.List;

public interface SongRepository extends JpaRepository<Song, Integer> {
    List<Song> findAll();

    @Query("select s from Song s where s.username=?1")
    public List<Song> findByUsername(String username);
    public Song findByUsernameAndSongnameAndSongwordAndAddress(String username,String songname,String songword,String address);
}
