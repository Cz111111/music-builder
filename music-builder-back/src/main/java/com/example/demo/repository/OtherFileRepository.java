package com.example.demo.repository;

import com.example.demo.model.OtherFile;
import com.example.demo.model.Song;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;


import java.util.List;

public interface OtherFileRepository extends JpaRepository<OtherFile, Integer>{

        OtherFile findByName(String name);
        public List<OtherFile> findByUsername(String username);

}
