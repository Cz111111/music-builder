package com.example.demo.model;


import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.IdClass;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;

@Entity
@Data
@IdClass(SongId.class)
@AllArgsConstructor
@NoArgsConstructor
public class Song {

    public Song(String username){
        this.username = username;
        this.songname=null;
        this.songword=null;
        this.address=null;
    }

    @Id
    private String username;
    private String songname;
    @Id
    private String songword;
    private String address;
}
