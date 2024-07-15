package com.example.demo.model;


import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;


@Entity
@Data
@AllArgsConstructor
@NoArgsConstructor
public class Song {

    public Song(String username,String songname){
        this.username = username;
        this.songname=songname;
        this.songword=null;
        this.address=null;
    }

    private String username;
    @Id
    private String songname;
    private String songword;
    private String address;
}
