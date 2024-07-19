package com.example.demo.service;


import com.example.demo.model.OtherFile;
import com.example.demo.model.Song;
import com.example.demo.repository.OtherFileRepository;
import com.example.demo.repository.SongRepository;
import jakarta.annotation.Resource;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class JpaOtherFileService {
    @Resource
    private OtherFileRepository otherFileRepository;

    public List<OtherFile> findByUsername(OtherFile otherFile){
        List<OtherFile> files=otherFileRepository.findByUsername(otherFile.getUsername());
        if(files.size()>0) {
            return files;
        }
        return null;
    }

    public OtherFile findByFilename(String name) {
        return otherFileRepository.findByName(name);
    }
    public OtherFile insertFile(OtherFile otherFile){
        return otherFileRepository.save(otherFile);
    }
}
